using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedRewardsPopUp : BasePopUp
{
    #region Event Field
    
    public Action ClickedClaimButton;
    
    #endregion // Event Field
    
    [Header("BUTTONS")]
    [SerializeField] private Button _claimButton;

    public override void Initialize(params object[] list)
    {
        base.Initialize();
        
        _claimButton.onClick.RemoveAllListeners();
        _claimButton.onClick.AddListener(OnClickClaimButton);
        
        CreateRewardCards();
    }

    private void CreateRewardCards()
    {
        
    }

    private void OnClickClaimButton()
    {
        Close();
        ClickedClaimButton?.Invoke();
    }
}
