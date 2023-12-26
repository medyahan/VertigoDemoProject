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
        [SerializeField] private Transform _reward;
        
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _amountText;

        private RewardData _rewardData;
        public RewardData RewardData => _rewardData;

        private bool _isBomb;
        public bool IsBomb => _isBomb;

        public void SetData(RewardData rewardDataData)
        {
            _isBomb = false;
            _rewardData = rewardDataData;

            _rewardImage.sprite = _rewardData.Sprite;
            _amountText.text = "x" + _rewardData.Amount;
        }

        public void Animate(float duration)
        {
            _reward.DOScale(new Vector3(1.5f, 1.5f, 1.5f), duration/2f).OnComplete((() =>
            {
                _reward.DOScale(Vector3.one, duration/2f);
            }));
        }

        public void MakeBomb(Sprite bombSprite)
        {
            _isBomb = true;
            _rewardImage.sprite = bombSprite;
            _amountText.text = "Bomb";
        }
    }
}
