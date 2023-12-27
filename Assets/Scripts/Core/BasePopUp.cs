using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopUp : MonoBehaviour
{
    [SerializeField] protected Transform _panelTransform;
    [SerializeField] protected float _openAnimateDuration = .7f;
    [SerializeField] protected float _closeAnimateDuration = .7f;

    public virtual void Initialize(params object[] list)
    {
        Open();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
