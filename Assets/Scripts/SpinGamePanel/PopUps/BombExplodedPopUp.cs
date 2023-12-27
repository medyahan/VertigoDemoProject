using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BombExplodedPopUp : BasePopUp
{
    #region Event Field
    
    public Action ClickedReviveButton;
    public Action ClickedGiveUpButton;
    
    #endregion // Event Field
    
    [Header("BUTTONS")]
    [SerializeField] private Button _giveUpButton;
    [SerializeField] private Button _reviveButton;

    public override void Initialize(params object[] list)
    {
        base.Initialize();
        
        _giveUpButton.onClick.RemoveAllListeners();
        _giveUpButton.onClick.AddListener(OnClickGiveUpButton);
        
        _reviveButton.onClick.RemoveAllListeners();
        _reviveButton.onClick.AddListener(OnClickReviveButton);
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
    
    private void OnClickGiveUpButton()
    {
        Close();
        ClickedGiveUpButton?.Invoke();
    }
    
    private void OnClickReviveButton()
    {
        Close();
        ClickedReviveButton?.Invoke();
    }
}
