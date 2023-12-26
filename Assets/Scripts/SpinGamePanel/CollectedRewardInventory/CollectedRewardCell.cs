using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.CollectedRewardInventory
{
    public class CollectedRewardCell : BaseMonoBehaviour
    {
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _amountText;

        private int _amount;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            RewardData rewardData = (RewardData)list[0];

            _amount = rewardData.Amount;
        
            _rewardImage.sprite = rewardData.Sprite;
            _amountText.text = "x" + _amount;
        }

        public void AddAmount(int amount)
        {
            _amount += amount;
            _amountText.text = "x" + _amount;
        }
    }
}
