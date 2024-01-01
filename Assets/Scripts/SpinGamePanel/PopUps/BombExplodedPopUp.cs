using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

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
        _giveUpButton = _giveUpButtonObj.GetComponent<BaseButton>();
        _reviveButton = _reviveButtonObj.GetComponent<BaseButton>();
    }

    public override void Initialize(params object[] list)
    {
        base.Initialize();

        _reviveCurrencyValue = (int) list[0];

        _giveUpButton = _giveUpButtonObj.GetComponent<BaseButton>();
        _reviveButton = _reviveButtonObj.GetComponent<BaseButton>();
        
        _giveUpButton.OnClick += OnClickGiveUpButton;
        _reviveButton.OnClick += OnClickReviveButton;
        
        PrepareReviveButton();
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
        
        _giveUpButton.OnClick -= OnClickGiveUpButton;
        _reviveButton.OnClick -= OnClickReviveButton;
    }

    private void PrepareReviveButton()
    {
        _reviveCurrencyValueText.text = _reviveCurrencyValue.ToString();

        if (GameManager.Instance.GoldCurrency < _reviveCurrencyValue)
        {
            _reviveButton.enabled = false;
            return;
        }
        _reviveButton.enabled = true;
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
