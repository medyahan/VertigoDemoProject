using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using RewardData = SpinGameData.RewardData;
using ZoneType = SpinGameData.ZoneType;

namespace SpinGamePanel.SpinWheel
{
    public class SpinWheelController : BaseMonoBehaviour
    {
        #region Variable Field

        [SerializeField] private List<SpinWheelData> _spinWheelDataList;
        
        [Header("WHEEL")] 
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private Image _wheelImage;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private int _wheelSliceCount;
        
        [Header("BUTTONS")]
        [SerializeField] private GameObject _spinButtonObj;
        private BaseButton _spinButton;

        [Header("REWARD CELL VALUES")]
        [SerializeField] private Transform _rewardContentParent;
        [SerializeField] private RewardCell _rewardCellPrefab;
        
        private List<RewardCell> _rewardCellList = new List<RewardCell>();

        [Header("SPINNING VALUES")] 
        [SerializeField] private AnimationCurve _spinCurve;
        [SerializeField] private float _spinDuration;
        [SerializeField] private float _numberCircleRotate;

        [Header("INDICATED REWARD VALUES")] 
        [SerializeField] private float _indicatedRewardAnimateDuration;

        private const float CIRCLE_ANGLE = 360f;
        private float _angelOfOneRewardCell;
        private float _currentTime;
        private int _currentZoneIndex;
        private bool _isSpinning;

        private SpinGameData _spinGameData;

        #endregion // Variable Field

        private void OnValidate()
        {
            if (_spinButton != null)
                _spinButton.OnClick = null;
            
            _spinButton = _spinButtonObj.GetComponent<BaseButton>();
            _spinButton.OnClick += OnClickSpinButton;
        }

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _currentZoneIndex = (int) list[0];
            _spinGameData = list[1] as SpinGameData;
            
            _wheelTransform.eulerAngles = Vector3.zero;
            _angelOfOneRewardCell = CIRCLE_ANGLE / _wheelSliceCount;
            
            CreateRewardCells();

            UpdateWheelByZoneIndex(_currentZoneIndex);
        }

        public override void End()
        {
            base.End();

            ClearAllRewardCell();
        }

        /// <summary>
        /// Instantiates and initializes a specified number of RewardCell prefabs in a circular arrangement.
        /// </summary>
        private void CreateRewardCells()
        {
            ClearAllRewardCell();
            
            for (int i = 0; i < _wheelSliceCount; i++)
            {
                RewardCell rewardCell = Instantiate(_rewardCellPrefab, Vector3.zero,
                    Quaternion.Euler(0, 0, -_angelOfOneRewardCell * i), _rewardContentParent);
                rewardCell.transform.localPosition = Vector3.zero;
                _rewardCellList.Add(rewardCell);
            }
        }

        private void ClearAllRewardCell()
        {
            foreach (var rewardCell in _rewardCellList)
            {
                rewardCell.End();
                Destroy(rewardCell.gameObject);
            }

            _rewardCellList.Clear();
        }

        #region BUTTON LISTENERS

        private void OnClickSpinButton()
        {
            _spinButton.enabled = false;
            _isSpinning = true;
            
            StartCoroutine(SpinCoroutine());
        }
        
        #endregion

        #region SPIN TRANSACTIONS

        /// <summary>
        /// Coroutine that rotates the wheel to a randomly selected reward cell and triggers the collection of the indicated reward.
        /// </summary>
        private IEnumerator SpinCoroutine()
        {
            // Get the starting angle of the wheel.
            float startAngle = _wheelTransform.eulerAngles.z;
            _currentTime = 0;
            
            // Select a random reward index for the spin.
            int randomRewardIndex = Random.Range(0, _wheelSliceCount);

            // Calculate the angle to rotate the wheel to the randomly selected reward.
            float angleWant = (_numberCircleRotate * CIRCLE_ANGLE) + _angelOfOneRewardCell * randomRewardIndex - startAngle;

            while (_currentTime < _spinDuration)
            {
                yield return new WaitForEndOfFrame();

                // Update the current time and calculate the current rotation angle.
                _currentTime += Time.deltaTime;
                float angleCurrent = angleWant * _spinCurve.Evaluate(_currentTime / _spinDuration);
                
                // Apply the rotation to the wheel.
                _wheelTransform.eulerAngles = new Vector3(0, 0, angleCurrent + startAngle);
            }

            // Get the indicated reward cell based on the random index.
            RewardCell indicatedRewardCell = _rewardCellList[randomRewardIndex];

            // Start the coroutine to collect the indicated reward.
            StartCoroutine(CollectRewardCoroutine(indicatedRewardCell));
        }

        /// <summary>
        /// Coroutine that animates the collection of the indicated reward cell, triggers events based on the collected reward,
        /// and enables the spin button after the collection is complete.
        /// </summary>
        /// <param name="indicatedRewardCell">The RewardCell to be collected.</param>
        private IEnumerator CollectRewardCoroutine(RewardCell indicatedRewardCell)
        {
            indicatedRewardCell.Animate(_indicatedRewardAnimateDuration);

            yield return new WaitForSeconds(_indicatedRewardAnimateDuration);

            // Trigger events based on the collected reward.
            if (indicatedRewardCell.IsBomb)
                SpinGameEventLib.Instance.BombExploded?.Invoke();
            else
                SpinGameEventLib.Instance.RewardCollected.Invoke(indicatedRewardCell.RewardData);
            
            // Enable the spin button and mark the spinning as complete.
            _spinButton.enabled = true;
            _isSpinning = false;
        }
        
        #endregion
        
        /// <summary>
        /// Updates the spin wheel visual elements and reward cells based on the specified game zone index.
        /// </summary>
        /// <param name="zoneIndex">The index of the current game zone.</param>
        public void UpdateWheelByZoneIndex(int zoneIndex)
        {
            SpinWheelData spinWheelData = GetSpinWheelDataByZoneIndex(zoneIndex);

            // Update the spin wheel and indicator sprites based on the spin wheel data.
            _wheelImage.sprite = spinWheelData.WheelSprite;
            _indicatorImage.sprite = spinWheelData.IndicatorSprite;

            for (int i = 0; i < _rewardCellList.Count; i++)
            {
                RewardData rewardData = spinWheelData.Rewards[Random.Range(0, spinWheelData.Rewards.Length)];
                _rewardCellList[i].SetData(rewardData);
            }

            // Add a bomb to a random reward cell if the spin wheel data specifies it.
            if (spinWheelData.HasBomb)
            {
                _rewardCellList[Random.Range(0, _wheelSliceCount)].MakeBomb(_spinGameData.BombSprite);
            }
        }

        private SpinWheelData GetSpinWheelDataByZoneIndex(int zoneIndex)
        {
            ZoneType zoneType = SpinGameEventLib.Instance.GetZoneTypeOfZoneIndex(zoneIndex);

            return _spinWheelDataList.FirstOrDefault(x => x.ZoneType == zoneType);
        }
        
        public bool GetIsWheelSpinning()
        {
            return _isSpinning;
        }
    }
}