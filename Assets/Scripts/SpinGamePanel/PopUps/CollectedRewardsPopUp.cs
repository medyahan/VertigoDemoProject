using System;
using System.Collections.Generic;
using UnityEngine;
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
        _claimButton = _claimButtonObj.GetComponent<BaseButton>();
    }
    
    public override void Initialize(params object[] list)
    {
        base.Initialize();
        
        _claimButton = _claimButtonObj.GetComponent<BaseButton>();
        _claimButton.OnClick += OnClickClaimButton;

        _collectedRewardDataArray = SpinGameEventLib.Instance.GetAllRewardInInventory?.Invoke();
        
        CreateRewardCards();
    }

    public override void Close()
    {
        ClearAllRewardCard();
        _claimButton.OnClick -= OnClickClaimButton;
        
        base.Close();
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

    private void ClearAllRewardCard()
    {
        foreach (RewardCard rewardCard in _rewardCardList){
                
            if(rewardCard == null)
                continue;
               
            rewardCard.End();
            Destroy(rewardCard.gameObject);
        }
        _rewardCardList.Clear();
    }

    #region BUTTON LISTENERS

    private void OnClickClaimButton()
    {
        Close();
        ClickedClaimButton?.Invoke();
    }
    
    #endregion
}
