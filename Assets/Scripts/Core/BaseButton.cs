using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : Button
{
    public Action OnClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        OnClick?.Invoke();
    }

    protected override void OnDestroy()
    {
        OnClick = null;
    }
}
