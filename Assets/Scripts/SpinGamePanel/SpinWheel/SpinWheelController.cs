using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using RewardData = SpinGameData.RewardData;
using SpinWheelData = SpinGameData.SpinWheelData;

namespace SpinGamePanel.SpinWheel
{
    public class SpinWheelController : BaseMonoBehaviour
    {
        #region Event Field

        public Action<RewardData> RewardCollected;
        public Action BombExploded;

        #endregion // Event Field

        #region Variable Field

        [SerializeField] private SpriteAtlas _spinWheelSpriteAtlas;
        [SerializeField] private SpriteAtlas _rewardSpriteAtlas;
        
        [Header("WHEEL")] 
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private Image _wheelImage;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private Button _spinButton;

        [Header("REWARD CELL VALUES")] 
        [SerializeField] private int _rewardCellCount;
        [SerializeField] private Transform _rewardContentParent;
        [SerializeField] private RewardCell _rewardCellPrefab;
        
        private List<RewardCell> _rewardCellList = new List<RewardCell>();

        [Header("SPINNING VALUES")] 
        [SerializeField] private AnimationCurve _spinCurve;
        [SerializeField] private float _spinDuration;
        [SerializeField] private float _numberCircleRotate;

        private const float CIRCLE_ANGLE = 360f;
        private float _angelOfOneRewardCell;
        private float _currentTime;
        private int _currentZoneIndex;
        private bool _isSpinning;
        
        private SpinGameData _spinGameData;

        #endregion // Variable Field

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _currentZoneIndex = (int) list[0];
            _spinGameData = list[1] as SpinGameData;
            
            _wheelTransform.eulerAngles = Vector3.zero;

            _spinButton.onClick.RemoveAllListeners();
            _spinButton.onClick.AddListener(OnClickSpinButton);

            _angelOfOneRewardCell = CIRCLE_ANGLE / _rewardCellCount;

            CreateRewardCells();

            UpdateWheelByZoneIndex(_currentZoneIndex);
        }

        public override void End()
        {
            base.End();

            foreach (var rewardCell in _rewardCellList)
            {
                rewardCell.End();
                Destroy(rewardCell.gameObject);
            } 
            
            _rewardCellList.Clear();
        }

        private void CreateRewardCells()
        {
            for (int i = 0; i < _rewardCellCount; i++)
            {
                RewardCell rewardCell = Instantiate(_rewardCellPrefab, Vector3.zero,
                    Quaternion.Euler(0, 0, -_angelOfOneRewardCell * i), _rewardContentParent);
                rewardCell.transform.localPosition = Vector3.zero;
                _rewardCellList.Add(rewardCell);
            }
        }

        private void OnClickSpinButton()
        {
            _spinButton.enabled = false;
            _isSpinning = true;
            
            StartCoroutine(SpinCoroutine());
        }

        private IEnumerator SpinCoroutine()
        {
            float startAngle = _wheelTransform.eulerAngles.z;
            _currentTime = 0;

            int randomRewardIndex = Random.Range(0, _rewardCellCount);

            float angelWant = (_numberCircleRotate * CIRCLE_ANGLE) + _angelOfOneRewardCell * randomRewardIndex -
                              startAngle;

            while (_currentTime < _spinDuration)
            {
                yield return new WaitForEndOfFrame();

                _currentTime += Time.deltaTime;

                float angleCurrent = angelWant * _spinCurve.Evaluate(_currentTime / _spinDuration);
                _wheelTransform.eulerAngles = new Vector3(0, 0, angleCurrent + startAngle);
            }

            RewardCell indicatedRewardCell = _rewardCellList[randomRewardIndex];

            StartCoroutine(CollectRewardCoroutine(indicatedRewardCell));
        }

        private IEnumerator CollectRewardCoroutine(RewardCell indicatedRewardCell)
        {
            indicatedRewardCell.Animate(1f);

            yield return new WaitForSeconds(1f);

            if (indicatedRewardCell.IsBomb)
                BombExploded?.Invoke();
            else
                RewardCollected.Invoke(indicatedRewardCell.RewardData);
            
            _spinButton.enabled = true;
            _isSpinning = false;
        }


        public void UpdateWheelByZoneIndex(int zoneIndex)
        {
            SpinWheelData spinWheelData = _spinGameData.GetSpinWheelDataByZoneIndex(zoneIndex);

            _wheelImage.sprite = _spinWheelSpriteAtlas.GetSprite(spinWheelData.WheelSpriteName);
            _indicatorImage.sprite = _spinWheelSpriteAtlas.GetSprite(spinWheelData.IndicatorSpriteName);

            for (int i = 0; i < _rewardCellList.Count; i++)
            {
                RewardData rewardData = spinWheelData.Rewards[Random.Range(0, spinWheelData.Rewards.Length)];
                Sprite rewardSprite = _rewardSpriteAtlas.GetSprite(rewardData.SpriteName);
                _rewardCellList[i].SetData(rewardData, rewardSprite);
            }

            if (spinWheelData.HasBomb)
            {
                _rewardCellList[Random.Range(0, _rewardCellCount)].MakeBomb(_spinGameData.BombSprite);
            }
        }

        public bool GetIsWheelSpinning()
        {
            return _isSpinning;
        }
    }
}