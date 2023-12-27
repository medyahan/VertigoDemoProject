using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using RewardType = SpinGameData.RewardType;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.CollectedRewardInventory
{
    public class RewardInventoryController : BaseMonoBehaviour
    {
        #region Event Field
        
        public Action ClickedExitButton;
        public Func<bool> GetIsWheelSpinning;

        #endregion // Event Field
        
        #region Variable Field
        
        [SerializeField] private SpriteAtlas _rewardSpriteAtlas;
        
        [SerializeField] private RewardInventoryCell _rewardInventoryCellPrefab;
        [SerializeField] private RectTransform _contentParent;

        [Header("BUTTON")]
        [SerializeField] private Button _exitButton;

        private Dictionary<string, RewardInventoryCell> _rewardInventoryCells;
        
        #endregion // Variable Field
        
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        
            _rewardInventoryCells = new Dictionary<string, RewardInventoryCell>();
            
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(OnClickExitButton);
        }

        public override void End()
        {
            base.End();
            
            foreach (var inventoryCell in _rewardInventoryCells.Values)
            {
                inventoryCell.End();
                Destroy(inventoryCell.gameObject);
            } 

            _rewardInventoryCells.Clear();
        }

        public void AddRewardToInventory(RewardData collectedRewardData)
        {
            if (_rewardInventoryCells.TryGetValue(collectedRewardData.SpriteName, out RewardInventoryCell rewardCell))
            {
                rewardCell.AddAmount(collectedRewardData.Amount);
                return;
            }

            CreateRewardInventoryCell(collectedRewardData);
        }

        private void CreateRewardInventoryCell(RewardData collectedRewardData)
        {
            RewardInventoryCell rewardInventoryCell = Instantiate(_rewardInventoryCellPrefab, _contentParent);

            Sprite rewardSprite = _rewardSpriteAtlas.GetSprite(collectedRewardData.SpriteName);
            rewardInventoryCell.Initialize(collectedRewardData, rewardSprite);
        
            _rewardInventoryCells.Add(collectedRewardData.SpriteName, rewardInventoryCell);
        }
        
        private void OnClickExitButton()
        {
            bool isWheelSpinning = (bool) GetIsWheelSpinning?.Invoke();

            if (isWheelSpinning)
            {
                _exitButton.transform.DOShakePosition(1f);
                return;
            }
            if(_rewardInventoryCells.Count == 0)
                GameManager.Instance.StopSpinGame();
            ClickedExitButton?.Invoke();
        }
    }
}
