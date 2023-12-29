using Core;
using SpinGamePanel.RewardInventory;
using SpinGamePanel.SpinWheel;
using SpinGamePanel.ZoneProgressBar;
using UnityEngine;
using RewardData = SpinGameData.RewardData;

namespace SpinGamePanel
{
    public class SpinGamePanelManager : BaseMonoBehaviour
    {
        #region Variable Field
        
        [SerializeField] private SpinGameData _spinGameData;

        [SerializeField] private GameObject _gamePanel;

        [Header("CONTROLLERS")] 
        [SerializeField] private CurrencyController _currencyController;
        [SerializeField] private SpinWheelController _spinWheelController;
        [SerializeField] private RewardInventoryController _rewardInventoryController;
        [SerializeField] private ZoneProgressBarController _zoneProgressBarController;
        [SerializeField] private NextZoneInfoController _nextZoneInfoController;

        [Header("POP UPS")] 
        [SerializeField] private BombExplodedPopUp _bombExplodedPopUp;
        [SerializeField] private ExitPopUp _exitPopUp;
        [SerializeField] private CollectedRewardsPopUp _collectedRewardsPopUp;

        [SerializeField] private int _totalZoneCount;
        
        private int _currentZoneIndex = 1;

        #endregion //Variable Field
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _currentZoneIndex = 1;
            _gamePanel.SetActive(true);
            
            _currencyController.Initialize();
            _spinWheelController.Initialize(_currentZoneIndex, _spinGameData);
            _rewardInventoryController.Initialize();
            _zoneProgressBarController.Initialize(_totalZoneCount);
            _nextZoneInfoController.Initialize(_totalZoneCount, _spinGameData.EverySafeZoneFactor, _spinGameData.EverySuperZoneFactor);
        }

        public override void RegisterEvents()
        {
            SpinGameEventLib.Instance.RewardCollected += OnCollectedReward;
            SpinGameEventLib.Instance.BombExploded += OnBombExploded;
            SpinGameEventLib.Instance.ClickedExitButton += OnClickExitButton;
            SpinGameEventLib.Instance.GetIsWheelSpinning += _spinWheelController.GetIsWheelSpinning;
            SpinGameEventLib.Instance.GetZoneTypeOfZoneIndex += _spinGameData.GetZoneTypeOfZoneIndex;
            SpinGameEventLib.Instance.GetAllRewardInInventory += _rewardInventoryController.GetAllRewardDataInInventory;
            
            _bombExplodedPopUp.ClickedReviveButton += OnClickedReviveButton;
            _bombExplodedPopUp.ClickedGiveUpButton += OnClickedGiveUpButton;
            _exitPopUp.ClickedCollectRewardsButton += OnClickedCollectRewardsButton;
            _collectedRewardsPopUp.ClickedClaimButton += OnClickedClaimButton;
        }

        public override void UnregisterEvents()
        {
            SpinGameEventLib.Instance.RewardCollected -= OnCollectedReward;
            SpinGameEventLib.Instance.BombExploded -= OnBombExploded;
            SpinGameEventLib.Instance.ClickedExitButton -= OnClickExitButton;
            SpinGameEventLib.Instance.GetIsWheelSpinning -= _spinWheelController.GetIsWheelSpinning;
            SpinGameEventLib.Instance.GetZoneTypeOfZoneIndex -= _spinGameData.GetZoneTypeOfZoneIndex;
            SpinGameEventLib.Instance.GetAllRewardInInventory -= _rewardInventoryController.GetAllRewardDataInInventory;

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
        
        /// <summary>
        /// Increases the current game zone index and checks if it exceeds the total zone count.
        /// If the current index surpasses the total count, stops the spinning game through the GameManager.
        /// </summary>
        private void IncreaseCurrentZoneIndex()
        {
            _currentZoneIndex++;

            // Check if the current index exceeds the total zone count.
            if (_currentZoneIndex > _totalZoneCount)
            {
                // ödülleri al ve oyunu bitir
                GameManager.Instance.StopSpinGame();
            }
        }

        #region LISTENERS
        
        /// <summary>
        /// Handles the event triggered when a reward is collected. Adds the collected reward to the inventory,
        /// advances the current zone index, and updates various UI elements related to the current game zone.
        /// </summary>
        /// <param name="collectedRewardData">Data associated with the collected reward.</param>
        private void OnCollectedReward(RewardData collectedRewardData)
        {
            _rewardInventoryController.AddRewardToInventory(collectedRewardData);

            IncreaseCurrentZoneIndex();
            
            // Update UI elements to reflect the changes in the current game zone.
            _zoneProgressBarController.OnChangeCurrentZoneIndex(_currentZoneIndex);
            _spinWheelController.UpdateWheelByZoneIndex(_currentZoneIndex);
            _nextZoneInfoController.OnChangeCurrentZoneIndex(_currentZoneIndex);
        }
        
        private void OnBombExploded()
        {
            _bombExplodedPopUp.Initialize(_spinGameData.ReviveCurrencyValue);
        }
        
        private void OnClickExitButton()
        {
            _exitPopUp.Initialize();
        }
        
        private void OnClickedReviveButton()
        {
            _currencyController.UpdateGoldCurrency(-_spinGameData.ReviveCurrencyValue);
            
            IncreaseCurrentZoneIndex();
            
            // Update UI elements to reflect the changes in the current game zone.
            _zoneProgressBarController.OnChangeCurrentZoneIndex(_currentZoneIndex);
            _spinWheelController.UpdateWheelByZoneIndex(_currentZoneIndex);
            _nextZoneInfoController.OnChangeCurrentZoneIndex(_currentZoneIndex);
        }
        
        private void OnClickedGiveUpButton()
        {
            GameManager.Instance.StopSpinGame();
        }
        
        private void OnClickedCollectRewardsButton()
        {
            _gamePanel.SetActive(false);
            _collectedRewardsPopUp.Initialize();
        }
        
        private void OnClickedClaimButton()
        {
            GameManager.Instance.StopSpinGame();
        }
        
        #endregion
    }
}