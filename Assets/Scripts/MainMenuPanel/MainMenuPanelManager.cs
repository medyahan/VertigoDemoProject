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
        if (_spinGameButton != null)
            _spinGameButton.OnClick = null;
            
        _spinGameButton = _spinGameButtonObj.GetComponent<BaseButton>();
        _spinGameButton.OnClick += OnClickSpinGameButton;
    }

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);
        
        _currencyController.Initialize();
    }

    private void OnClickSpinGameButton()
    {
        GameManager.Instance.StartSpinGame();
    }
}
