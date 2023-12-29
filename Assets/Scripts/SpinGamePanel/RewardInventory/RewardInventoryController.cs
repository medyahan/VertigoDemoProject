using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using RewardType = SpinGameData.RewardType;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.RewardInventory
{
    public class RewardInventoryController : BaseMonoBehaviour
    {
        #region Variable Field
        
        [SerializeField] private SpriteAtlas _rewardSpriteAtlas;
        
        [Header("INVENTORY CELL")]
        [SerializeField] private RewardInventoryCell _rewardInventoryCellPrefab;
        [SerializeField] private RectTransform _contentParent;

        [Header("BUTTON")]
        [SerializeField] private GameObject _exitButtonObj;
        private BaseButton _exitButton;

        private Dictionary<string, RewardInventoryCell> _rewardInventoryCells;
        
        #endregion // Variable Field

        private void OnValidate()
        {
            if (_exitButton != null)
                _exitButton.OnClick = null;
            
            _exitButton = _exitButtonObj.GetComponent<BaseButton>();
            _exitButton.OnClick += OnClickExitButton;
        }
        
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        
            _rewardInventoryCells = new Dictionary<string, RewardInventoryCell>();
        }

        public override void End()
        {
            base.End();
            
            foreach (RewardInventoryCell inventoryCell in _rewardInventoryCells.Values)
            {
                inventoryCell.End();
                Destroy(inventoryCell.gameObject);
            } 

            _rewardInventoryCells.Clear();
        }

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
        /// Handles the click event of the exit button. If the wheel is currently spinning, shakes the exit button
        /// to indicate that it cannot be pressed. If the wheel is not spinning and the reward inventory is empty,
        /// stops the spinning game. Otherwise, invokes the ClickedExitButton event.
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
            if(_rewardInventoryCells.Count == 0)
                GameManager.Instance.StopSpinGame();
            
            // If there are rewards in the inventory, invoke the ClickedExitButton event.
            SpinGameEventLib.Instance.ClickedExitButton?.Invoke();
        }
    }
}
