using System;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.SpinWheel
{
    public class RewardCell : BaseMonoBehaviour
    {
        #region Variable Field
        
        [Header("COMPONENTS")]
        [SerializeField] private Transform _rewardTransform;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _amountText;

        [Header("DATA")]
        [SerializeField] private RewardData _rewardData;
        public RewardData RewardData => _rewardData;

        private bool _isBomb;
        public bool IsBomb => _isBomb;
        
        #endregion // Variable Field

        private void OnValidate()
        {
            if(_rewardData != null)
                SetData(_rewardData);
        }

        public void SetData(RewardData rewardDataData)
        {
            _isBomb = false;
            _rewardData = rewardDataData;

            _rewardImage.sprite = _rewardData.Sprite;
            _amountText.text = "x" + _rewardData.Amount;

            _rewardImage.preserveAspect = true;
        }

        /// <summary>
        /// Animates the reward cell by scaling it up and then back to its original size over the specified duration.
        /// </summary>
        /// <param name="duration">The total duration of the animation.</param>
        public void Animate(float duration)
        {
            _rewardTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), duration/2f).OnComplete((() =>
            {
                _rewardTransform.DOScale(Vector3.one, duration/2f);
            }));
        }

        /// <summary>
        /// Converts the reward cell into a bomb by updating its visual elements with the specified bomb sprite.
        /// </summary>
        /// <param name="bombSprite">The sprite to be displayed for the bomb.</param>
        public void MakeBomb(Sprite bombSprite)
        {
            _isBomb = true;
            _rewardImage.sprite = bombSprite;
            _amountText.text = "Bomb";
        }
    }
}
