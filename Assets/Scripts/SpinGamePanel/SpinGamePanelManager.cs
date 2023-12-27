using Core;
using UnityEngine;
using ZoneType = SpinGameData.ZoneType;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel
{
    public class SpinGamePanelManager : BaseMonoBehaviour
    {
        [SerializeField] private SpinGameData _spinGameData;

        [SerializeField] private GameObject _gamePanel;

        [SerializeField] private SpinWheel.SpinWheelController _spinWheelController;
        [SerializeField] private CollectedRewardInventory.RewardInventoryController _rewardInventoryController;
        [SerializeField] private ZoneProgressBar.ZoneProgressBarController _zoneProgressBarController;
        [SerializeField] private NextZoneInfoController _nextZoneInfoController;

        [Header("Pop Up")] 
        [SerializeField] private BombExplodedPopUp _bombExplodedPopUp;
        [SerializeField] private ExitPopUp _exitPopUp;
        [SerializeField] private CollectedRewardsPopUp _collectedRewardsPopUp;

        [SerializeField] private int _totalZoneCount;
        private int _currentZoneIndex = 1;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _currentZoneIndex = 1;
            _gamePanel.SetActive(true);
            
            _spinWheelController.Initialize(_currentZoneIndex, _spinGameData);
            _rewardInventoryController.Initialize();
            _zoneProgressBarController.Initialize(_totalZoneCount);
            _nextZoneInfoController.Initialize(_totalZoneCount);
        }

        public override void RegisterEvents()
        {
            _spinWheelController.RewardCollected += OnCollectedReward;
            _spinWheelController.BombExploded += OnBombExploded;
            _rewardInventoryController.ClickedExitButton += OnClickExitButton;
            _rewardInventoryController.GetIsWheelSpinning += _spinWheelController.GetIsWheelSpinning;

            _bombExplodedPopUp.ClickedReviveButton += OnClickedReviveButton;
            _bombExplodedPopUp.ClickedGiveUpButton += OnClickedGiveUpButton;
            _exitPopUp.ClickedCollectRewardsButton += OnClickedCollectRewardsButton;
            _collectedRewardsPopUp.ClickedClaimButton += OnClickedClaimButton;
        }

        public override void UnregisterEvents()
        {
            _spinWheelController.RewardCollected -= OnCollectedReward;
            _spinWheelController.BombExploded -= OnBombExploded;
            _rewardInventoryController.ClickedExitButton -= OnClickExitButton;
            _rewardInventoryController.GetIsWheelSpinning -= _spinWheelController.GetIsWheelSpinning;
            
            _bombExplodedPopUp.ClickedReviveButton -= OnClickedReviveButton;
            _bombExplodedPopUp.ClickedGiveUpButton -= OnClickedGiveUpButton;
            _exitPopUp.ClickedCollectRewardsButton -= OnClickedCollectRewardsButton;
            _collectedRewardsPopUp.ClickedClaimButton -= OnClickedClaimButton;
        }

        public override void End()
        {
            base.End();
            
            _spinWheelController.End();
            _rewardInventoryController.End();
            _zoneProgressBarController.End();
            _nextZoneInfoController.End();
        }
        
        private void IncreaseCurrentZoneIndex()
        {
            _currentZoneIndex++;

            if (_currentZoneIndex > _totalZoneCount)
            {
                // ödülleri al ve oyunu bitir
                GameManager.Instance.StopSpinGame();
            }
        }

        #region LISTENERS
        
        private void OnCollectedReward(RewardData collectedRewardData)
        {
            _rewardInventoryController.AddRewardToInventory(collectedRewardData);

            IncreaseCurrentZoneIndex();
            
            _zoneProgressBarController.OnChangeCurrentZoneIndex(_currentZoneIndex, _spinGameData.GetZoneTypeOfZoneIndex(_currentZoneIndex));
            _spinWheelController.UpdateWheelByZoneIndex(_currentZoneIndex);
            _nextZoneInfoController.OnChangeCurrentZoneIndex(_currentZoneIndex);
        }
        
        private void OnBombExploded()
        {
            _bombExplodedPopUp.Initialize();
        }
        
        private void OnClickExitButton()
        {
            _exitPopUp.Initialize();
        }
        
        private void OnClickedReviveButton()
        {
            // Para eksiltme işlemleri
            // Oyun devam eder
            
            IncreaseCurrentZoneIndex();
            
            _zoneProgressBarController.OnChangeCurrentZoneIndex(_currentZoneIndex, _spinGameData.GetZoneTypeOfZoneIndex(_currentZoneIndex));
            _spinWheelController.UpdateWheelByZoneIndex(_currentZoneIndex);
            _nextZoneInfoController.OnChangeCurrentZoneIndex(_currentZoneIndex);
        }
        
        private void OnClickedGiveUpButton()
        {
            // Oyun resetlenir
            GameManager.Instance.StopSpinGame();
        }
        
        private void OnClickedCollectRewardsButton()
        {
            // Oyuncu ödülleri gönder
            _gamePanel.SetActive(false);
            _collectedRewardsPopUp.Initialize(); //TODO ödülleri gönder
        }
        
        private void OnClickedClaimButton()
        {
            GameManager.Instance.StopSpinGame();
        }
        
        #endregion
    }
}