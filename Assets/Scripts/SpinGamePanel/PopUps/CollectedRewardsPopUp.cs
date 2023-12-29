using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RewardData = SpinGameData.RewardData;

public class CollectedRewardsPopUp : BasePopUp
{
    #region Event Field
    
    public Action ClickedClaimButton;
    
    #endregion // Event Field

    [Header("REWARD CARD")] 
    [SerializeField] private RewardCard _rewardCardPrefab;
    [SerializeField] private Transform _contentParent;
    private List<RewardCard> _rewardCardList = new List<RewardCard>();

    [Header("BUTTONS")]
    [SerializeField] private GameObject _claimButtonObj;
    private BaseButton _claimButton;

    private RewardData[] _collectedRewardDataArray;

    private void OnValidate()
    {
        if (_claimButton != null)
            _claimButton.OnClick = null;
            
        _claimButton = _claimButtonObj.GetComponent<BaseButton>();
        _claimButton.OnClick += OnClickClaimButton;
    }
    
    public override void Initialize(params object[] list)
    {
        base.Initialize();

        _collectedRewardDataArray = SpinGameEventLib.Instance.GetAllRewardInInventory?.Invoke();
        
        CreateRewardCards();
    }

    private void CreateRewardCards()
    {
        for (int i = 0; i < _collectedRewardDataArray.Length; i++)
        {
            RewardData rewardData = _collectedRewardDataArray[i];
            RewardCard rewardCard = Instantiate(_rewardCardPrefab, _contentParent);

            rewardCard.SetData(rewardData);
            _rewardCardList.Add(rewardCard);
        }
    }

    #region BUTTON LISTENERS

    private void OnClickClaimButton()
    {
        Close();
        ClickedClaimButton?.Invoke();
    }
    
    #endregion
}
