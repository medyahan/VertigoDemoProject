using System.Collections;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.RewardInventory
{
    public class RewardInventoryCell : BaseMonoBehaviour
    {
        #region Variable Field
        
        [Header("COMPONENTS")]
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _amountText;

        [Header("VALUES")] 
        [SerializeField] private float _addingAmountAnimateDuration;
        
        private RewardData _rewardData;
        public RewardData RewardData => _rewardData;

        #endregion // Variable Field
        
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            RewardData rewardData = (RewardData)list[0];

            SetData(rewardData);
        }

        /// <summary>
        /// Sets the reward data and updates the visual elements of the RewardInventoryCell.
        /// </summary>
        /// <param name="rewardData">Data associated with the reward.</param>
        private void SetData(RewardData rewardData)
        {
            _rewardData = rewardData;

            // Update the sprite and amount text to reflect the new reward data.
            _rewardImage.sprite = _rewardData.Sprite;
            _amountText.text = "x" + _rewardData.Amount;
        }

        /// <summary>
        /// Adds the specified amount to the existing reward amount, animating the change with DOTween.
        /// </summary>
        /// <param name="amount">The amount to add to the reward.</param>
        public void AddAmount(int amount)
        {
            int oldAmount = _rewardData.Amount;
            int newAmount = oldAmount + amount;

            DOTween.To(x => _rewardData.Amount = (int) x, oldAmount, newAmount, _addingAmountAnimateDuration)
                .SetEase(Ease.Linear).OnUpdate(() => _amountText.text = "x" + _rewardData.Amount);
        }
    }
}
