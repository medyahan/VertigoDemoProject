using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.CollectedRewardInventory
{
    public class RewardInventoryCell : BaseMonoBehaviour
    {
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _amountText;

        private RewardData _rewardData;
        
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            RewardData rewardData = (RewardData)list[0];
            Sprite sprite = (Sprite) list[1];

            SetData(rewardData, sprite);
        }

        private void SetData(RewardData rewardData, Sprite sprite)
        {
            _rewardData = rewardData;

            _rewardImage.sprite = sprite;
            _amountText.text = "x" + _rewardData.Amount;
        }

        public void AddAmount(int amount)
        {
            _rewardData.Amount += amount;
            _amountText.text = "x" + _rewardData.Amount;
        }
    }
}
