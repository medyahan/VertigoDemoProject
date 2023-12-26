using System;
using System.Collections;
using System.Collections.Generic;
using SpinGamePanel;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SpinGamePanelManager _spinGamePanelManager;
    
    private void Start()
    {
        _spinGamePanelManager.Initialize();
    }

    private void OnDestroy()
    {
        _spinGamePanelManager.End();
    }

    public void Restart()
    {
        _spinGamePanelManager.End();
        _spinGamePanelManager.Initialize();
    }
}
