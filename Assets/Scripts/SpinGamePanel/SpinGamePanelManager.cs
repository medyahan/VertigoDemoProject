using Core;
using UnityEngine;
using ZoneType = SpinGameData.ZoneType;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel
{
    public class SpinGamePanelManager : BaseMonoBehaviour
    {
        [SerializeField] private SpinGameData _spinGameData;

        [SerializeField] private SpinWheel.SpinWheel _spinWheel;
        [SerializeField] private CollectedRewardInventory.CollectedRewardInventory _collectedRewardInventory;
        [SerializeField] private ZoneProgressBar.ZoneProgressBar _zoneProgressBar;
        [SerializeField] private NextZoneInfo _nextZoneInfo;

        [Header("Pop Up")] 
        [SerializeField] private BombExplodedPopUp _bombExplodedPopUp;
        [SerializeField] private ExitPopUp _exitPopUp;

        [SerializeField] private int _totalZoneCount;
        private int _currentZoneIndex = 1;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _spinWheel.Initialize(_currentZoneIndex, _spinGameData);
            _collectedRewardInventory.Initialize();
            _zoneProgressBar.Initialize(_totalZoneCount);
            _nextZoneInfo.Initialize(_totalZoneCount);
            
            _bombExplodedPopUp.Initialize();
            _exitPopUp.Initialize();
        }

        public override void RegisterEvents()
        {
            _spinWheel.RewardCollected += OnCollectedReward;
            _spinWheel.BombExploded += OnBombExploded;
            _collectedRewardInventory.ClickedExitButton += OnClickExitButton;

            _bombExplodedPopUp.ClickedReviveButton += OnClickedReviveButton;
            _bombExplodedPopUp.ClickedGiveUpButton += OnClickedGiveUpButton;

            _exitPopUp.ClickedCollectRewardsButton += OnClickedCollectRewardsButton;
        }

        public override void UnregisterEvents()
        {
            _spinWheel.RewardCollected -= OnCollectedReward;
            _spinWheel.BombExploded -= OnBombExploded;
            _collectedRewardInventory.ClickedExitButton -= OnClickExitButton;
            
            _bombExplodedPopUp.ClickedReviveButton -= OnClickedReviveButton;
            _bombExplodedPopUp.ClickedGiveUpButton -= OnClickedGiveUpButton;
            
            _exitPopUp.ClickedCollectRewardsButton -= OnClickedCollectRewardsButton;
        }

        public override void End()
        {
            base.End();
            
            _spinWheel.End();
            _collectedRewardInventory.End();
            _zoneProgressBar.End();
            _nextZoneInfo.End();
        }
        
        private void OnBombExploded()
        {
            _bombExplodedPopUp.Open();
        }
        
        private void OnCollectedReward(RewardData collectedRewardData)
        {
            _collectedRewardInventory.AddCollectedReward(collectedRewardData);

            IncreaseCurrentZoneIndex();
            
            _zoneProgressBar.OnChangeCurrentZoneIndex(_currentZoneIndex, _spinGameData.GetZoneTypeOfZoneIndex(_currentZoneIndex));
            _spinWheel.UpdateWheelByZoneIndex(_currentZoneIndex);
            _nextZoneInfo.OnChangeCurrentZoneIndex(_currentZoneIndex);
        }

        private void IncreaseCurrentZoneIndex()
        {
            _currentZoneIndex++;
        }
        
        private void OnClickExitButton()
        {
            _exitPopUp.Open();
        }
        
        private void OnClickedReviveButton()
        {
            // Para eksiltme işlemleri
            // Oyun devam eder
            
            IncreaseCurrentZoneIndex();
            
            _zoneProgressBar.OnChangeCurrentZoneIndex(_currentZoneIndex, _spinGameData.GetZoneTypeOfZoneIndex(_currentZoneIndex));
            _spinWheel.UpdateWheelByZoneIndex(_currentZoneIndex);
            _nextZoneInfo.OnChangeCurrentZoneIndex(_currentZoneIndex);
        }
        
        private void OnClickedGiveUpButton()
        {
            // Oyun resetlenir
            // GameManager.Instance.Restart();
        }
        
        private void OnClickedCollectRewardsButton()
        {
            // Oyuncu ödülleri alır ve oyunu bitir.
        }
    }
}