using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanelManager : BaseMonoBehaviour
{
    [SerializeField] private Button _spinGameButton;

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);
        
        _spinGameButton.onClick.RemoveAllListeners();
        _spinGameButton.onClick.AddListener(OnClickSpinGameButton);
    }

    private void OnClickSpinGameButton()
    {
        GameManager.Instance.StartSpinGame();
    }
}
