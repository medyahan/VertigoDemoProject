using System;
using Core;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : BaseMonoBehaviour, IPointerClickHandler
{
    public Action OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    private void OnDestroy()
    {
        OnClick = null;
    }
}
