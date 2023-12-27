using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExitPopUp : BasePopUp
{
    #region Event Field
    
    public Action ClickedCollectRewardsButton;
    
    #endregion // Event Field
    
    [Header("BUTTONS")]
    [SerializeField] private Button _collectRewardsButton;
    [SerializeField] private Button _goBackButton;
    
    public override void Initialize(params object[] list)
    {
        base.Initialize();
        
        _collectRewardsButton.onClick.RemoveAllListeners();
        _collectRewardsButton.onClick.AddListener(OnClickCollectRewardsButton);
        
        _goBackButton.onClick.RemoveAllListeners();
        _goBackButton.onClick.AddListener(OnClickGoBackButton);
    }

    public override void Open()
    {
        base.Open();
        
        _panelTransform.localScale = Vector3.one;
        
        Vector3 value = _panelTransform.localScale;
        _panelTransform.DOScale(value, _openAnimateDuration).From(Vector3.zero).SetEase(Ease.OutBounce);
    }

    public override void Close()
    {
        _panelTransform.DOScale(Vector3.zero, _closeAnimateDuration).SetEase(Ease.InSine).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnClickGoBackButton()
    {
        Close();
    }
    
    private void OnClickCollectRewardsButton()
    {
        Close();
        ClickedCollectRewardsButton?.Invoke();
    }
}
