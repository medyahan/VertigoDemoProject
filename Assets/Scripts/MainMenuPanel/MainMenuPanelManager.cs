using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanelManager : BaseMonoBehaviour
{
    [SerializeField] private CurrencyController _currencyController;
    [SerializeField] private GameObject _spinGameButtonObj;
    private BaseButton _spinGameButton;

    private void OnValidate()
    {
        _spinGameButton = _spinGameButtonObj.GetComponent<BaseButton>();
    }

    public override void Initialize(params object[] list)
    {
        _spinGameButton = _spinGameButtonObj.GetComponent<BaseButton>();
        
        base.Initialize(list);
        
        _currencyController.Initialize();
    }

    public override void RegisterEvents()
    {
        _spinGameButton.OnClick += OnClickSpinGameButton;
    }

    public override void UnregisterEvents()
    {
        _spinGameButton.OnClick -= OnClickSpinGameButton;
    }

    private void OnClickSpinGameButton()
    {
        GameManager.Instance.StartSpinGame();
    }
}
