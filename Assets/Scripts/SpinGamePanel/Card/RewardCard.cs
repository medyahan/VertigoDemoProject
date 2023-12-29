using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RewardData = SpinGameData.RewardData;

public class RewardCard : BaseMonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private Image _rewardImage;
    [SerializeField] private TMP_Text _amountText;

    private RewardData _rewardData;
    
    public void SetData(RewardData rewardData)
    {
        _rewardData = rewardData;
        
        _rewardImage.sprite = _rewardData.Sprite;
        _amountText.text = "x" + _rewardData.Amount;
    }
}
