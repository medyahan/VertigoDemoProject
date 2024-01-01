using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using RewardData = SpinGameData.RewardData;

public class ShowRewardPopUp : BasePopUp, IPointerClickHandler
{
    #region Event Field
    
    public Action<RewardData> ClosedShowRewardPopUp;
    
    #endregion // Event Field
    
    [SerializeField] private RewardCard _rewardCard;
    private RewardData _rewardData;

    private bool _isClosed;
    
    public override void Initialize(params object[] list)
    {
        base.Initialize(list);
        
        _rewardData = (RewardData) list[0];
        _rewardCard.SetData(_rewardData);
    }

    public override void Open()
    {
        base.Open();
        
        _isClosed = false;
        _panelTransform.localScale = Vector3.one;
        
        Vector3 value = _panelTransform.localScale;
        _panelTransform.DOScale(value, _openAnimateDuration).From(Vector3.zero).SetEase(Ease.OutBounce);
        
        Invoke(nameof(Close), 3f);
    }

    public override void Close()
    {
        if(_isClosed)
            return;

        _isClosed = true;
        _panelTransform.DOScale(Vector3.zero, _closeAnimateDuration).SetEase(Ease.InSine).OnComplete(() =>
        {
            gameObject.SetActive(false);
            ClosedShowRewardPopUp?.Invoke(_rewardData);
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Close();
    }
}
