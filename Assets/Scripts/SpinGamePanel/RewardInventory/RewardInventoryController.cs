using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.RewardInventory
{
    public class RewardInventoryController : BaseMonoBehaviour
    {
        #region Variable Field

        [Header("INVENTORY CELL")]
        [SerializeField] private RewardInventoryCell _rewardInventoryCellPrefab;
        [SerializeField] private RectTransform _contentParent;

        [Header("BUTTON")]
        [SerializeField] private GameObject _exitButtonObj;
        private BaseButton _exitButton;

        private Dictionary<string, RewardInventoryCell> _rewardInventoryCells = new Dictionary<string, RewardInventoryCell>();
        
        #endregion // Variable Field

        private void OnValidate()
        {
            _exitButton = _exitButtonObj.GetComponent<BaseButton>();
        }
        
        public override void Initialize(params object[] list)
        {
            _exitButton = _exitButtonObj.GetComponent<BaseButton>();
            
            base.Initialize(list);
        }

        public override void RegisterEvents()
        {
            _exitButton.OnClick += OnClickExitButton;
        }

        public override void UnregisterEvents()
        {
            _exitButton.OnClick -= OnClickExitButton;
        }

        public override void End()
        {
            base.End();
            ClearAllRewardInventoryCell();
        }
        
        /// <summary>
        /// Retrieves all reward data from the reward inventory cells in the inventory.
        /// </summary>
        /// <returns>An array containing all reward data in the inventory.</returns>
        public RewardData[] GetAllRewardDataInInventory()
        {
            List<RewardData> allRewardData = new List<RewardData>();
            
            foreach (RewardInventoryCell inventoryCell in _rewardInventoryCells.Values)
            {
                allRewardData.Add(inventoryCell.RewardData);
            }

            return allRewardData.ToArray();
        }

        /// <summary>
        /// Adds the collected reward to the reward inventory. If the reward with the same sprite name
        /// already exists in the inventory, increases its amount. Otherwise, creates a new inventory cell.
        /// </summary>
        /// <param name="collectedRewardData">Data associated with the collected reward.</param>
        public void AddRewardToInventory(RewardData collectedRewardData)
        {
            // Check if a reward with the same sprite name already exists in the inventory.
            if (_rewardInventoryCells.TryGetValue(collectedRewardData.Sprite.name, out RewardInventoryCell rewardCell))
            {
                rewardCell.AddAmount(collectedRewardData.Amount);
                return;
            }

            CreateRewardInventoryCell(collectedRewardData);
        }

        /// <summary>
        /// Instantiates a new RewardInventoryCell prefab, initializes it with the collected reward data and sprite,
        /// and adds it to the reward inventory dictionary using the sprite name as the key.
        /// </summary>
        /// <param name="collectedRewardData">Data associated with the collected reward.</param>
        private void CreateRewardInventoryCell(RewardData collectedRewardData)
        {
            RewardInventoryCell rewardInventoryCell = Instantiate(_rewardInventoryCellPrefab, _contentParent);

            rewardInventoryCell.Initialize(collectedRewardData);
        
            _rewardInventoryCells.Add(collectedRewardData.Sprite.name, rewardInventoryCell);
        }

        /// <summary>
        /// Clears and removes all reward inventory cells from the dictionary and scene.
        /// </summary>
        private void ClearAllRewardInventoryCell()
        {
            foreach (RewardInventoryCell inventoryCell in _rewardInventoryCells.Values)
            {
                if(inventoryCell == null)
                    continue;
                
                inventoryCell.End();
                Destroy(inventoryCell.gameObject);
            }

            _rewardInventoryCells.Clear();
        }

        #region BUTTON LISTENERS

        /// <summary>
        /// Handles the exit button click event. Shakes the button if the wheel is spinning,
        /// stops the game if the inventory is empty, or invokes the exit button event otherwise.
        /// </summary>
        private void OnClickExitButton()
        {
            bool isWheelSpinning = (bool) SpinGameEventLib.Instance.GetIsWheelSpinning?.Invoke();

            if (isWheelSpinning)
            {
                _exitButton.transform.DOShakePosition(1f);
                return;
            }
            
            // If the reward inventory is empty, stop the spinning game.
            if (_rewardInventoryCells.Count == 0)
            {
                GameManager.Instance.StopSpinGame();
                return;
            }
            
            // If there are rewards in the inventory, invoke the ClickedExitButton event.
            SpinGameEventLib.Instance.ClickedExitButton?.Invoke();
        }
        
        #endregion
    }
}
