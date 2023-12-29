using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombExplodedPopUp : BasePopUp
{
    #region Event Field
    
    public Action ClickedReviveButton;
    public Action ClickedGiveUpButton;
    
    #endregion // Event Field
    
    [Header("BUTTONS")]
    [SerializeField] private GameObject _giveUpButtonObj;
    [SerializeField] private GameObject _reviveButtonObj;

    [Header("TEXTS")] 
    [SerializeField] private TMP_Text _reviveCurrencyValueText;
    
    private BaseButton _giveUpButton;
    private BaseButton _reviveButton;

    private int _reviveCurrencyValue;
    
    private void OnValidate()
    {
        if (_giveUpButton != null)
            _giveUpButton.OnClick = null;
        
        if (_reviveButton != null)
            _reviveButton.OnClick = null;
            
        _giveUpButton = _giveUpButtonObj.GetComponent<BaseButton>();
        _giveUpButton.OnClick += OnClickGiveUpButton;
        
        _reviveButton = _reviveButtonObj.GetComponent<BaseButton>();
        _reviveButton.OnClick += OnClickReviveButton;
    }

    public override void Initialize(params object[] list)
    {
        base.Initialize();

        _reviveCurrencyValue = (int) list[0];
        _reviveCurrencyValueText.text = _reviveCurrencyValue.ToString();
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

    #endregion
}
