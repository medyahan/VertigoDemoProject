using System;
using System.Collections;
using System.Collections.Generic;
using SpinGamePanel;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SpinGamePanelManager _spinGamePanelManager;
    [SerializeField] private MainMenuPanelManager _mainMenuPanelManager;

    [SerializeField] private int _defaultGoldCurrency;
    [SerializeField] private int _defaultCashCurrency;

    public int GoldCurrency
    {
        get => PlayerPrefs.GetInt("Gold", _defaultGoldCurrency);
        set => PlayerPrefs.SetInt("Gold", value);
    }
    
    public int CashCurrency
    {
        get => PlayerPrefs.GetInt("Cash", _defaultCashCurrency);
        set => PlayerPrefs.SetInt("Cash", value);
    }

    private void Start()
    {
        _mainMenuPanelManager.Initialize();
    }

    private void OnDestroy()
    {
        _mainMenuPanelManager.End();
        _spinGamePanelManager.End();
    }

    public void StartSpinGame()
    {
        _mainMenuPanelManager.gameObject.SetActive(false);
        _mainMenuPanelManager.End();
        
        _spinGamePanelManager.gameObject.SetActive(true);
        _spinGamePanelManager.Initialize();
    }

    public void StopSpinGame()
    {
        _spinGamePanelManager.End();
        _spinGamePanelManager.gameObject.SetActive(false);
        
        _mainMenuPanelManager.gameObject.SetActive(true);
        _mainMenuPanelManager.Initialize();
    }
}
