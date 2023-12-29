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
    [SerializeField] private GameObject _collectRewardsButtonObj;
    [SerializeField] private GameObject _goBackButtonObj;
    
    private BaseButton _collectRewardsButton;
    private BaseButton _goBackButton;
    
    private void OnValidate()
    {
        if (_collectRewardsButton != null)
            _collectRewardsButton.OnClick = null;
        
        if (_goBackButton != null)
            _goBackButton.OnClick = null;
            
        _collectRewardsButton = _collectRewardsButtonObj.GetComponent<BaseButton>();
        _collectRewardsButton.OnClick += OnClickCollectRewardsButton;
        
        _goBackButton = _goBackButtonObj.GetComponent<BaseButton>();
        _goBackButton.OnClick += OnClickGoBackButton;
    }
    
    public override void Initialize(params object[] list)
    {
        base.Initialize();
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

    #region BUTTON LISTENERS

    private void OnClickGoBackButton()
    {
        Close();
    }
    
    private void OnClickCollectRewardsButton()
    {
        Close();
        ClickedCollectRewardsButton?.Invoke();
    }

    #endregion
}
