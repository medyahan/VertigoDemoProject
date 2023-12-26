using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;
using RewardType = SpinGameData.RewardType;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel.CollectedRewardInventory
{
    public class CollectedRewardInventory : BaseMonoBehaviour
    {
        public Action ClickedExitButton;
        
        [SerializeField] private CollectedRewardCell _collectedRewardCellPrefab;
        [SerializeField] private RectTransform _contentParent;

        [SerializeField] private Button _exitButton;

        private Dictionary<RewardType, CollectedRewardCell> _collectedRewardCells;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        
            _collectedRewardCells = new Dictionary<RewardType, CollectedRewardCell>();
            
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(OnClickExitButton);
        }

        public override void End()
        {
            base.End();
            
            foreach (var collectedRewardCell in _collectedRewardCells.Values)
            {
                collectedRewardCell.End();
                Destroy(collectedRewardCell);
            } 

            _collectedRewardCells.Clear();
        }

        public void AddCollectedReward(RewardData collectedRewardData)
        {
            if (_collectedRewardCells.TryGetValue(collectedRewardData.Type, out CollectedRewardCell rewardCell))
            {
                rewardCell.AddAmount(collectedRewardData.Amount);
                return;
            }

            CollectedRewardCell collectedRewardCell = Instantiate(_collectedRewardCellPrefab, _contentParent);
            collectedRewardCell.Initialize(collectedRewardData);
        
            _collectedRewardCells.Add(collectedRewardData.Type, collectedRewardCell);
        }
        
        private void OnClickExitButton()
        {
            ClickedExitButton?.Invoke();
        }
    }
}
