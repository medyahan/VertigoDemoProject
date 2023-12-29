using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

public class CurrencyController : BaseMonoBehaviour
{
    [SerializeField] private TMP_Text _cashCurrencyText;
    [SerializeField] private TMP_Text _goldCurrencyText;

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _cashCurrencyText.text = GameManager.Instance.CashCurrency.ToString();
        _goldCurrencyText.text = GameManager.Instance.GoldCurrency.ToString();
    }

    public void UpdateGoldCurrency(int value)
    {
        int oldValue = GameManager.Instance.GoldCurrency;
        GameManager.Instance.GoldCurrency = oldValue + value;
        
        _goldCurrencyText.text = GameManager.Instance.GoldCurrency.ToString();
    }

    public void UpdateCashCurrency(int value)
    {
        int oldValue = GameManager.Instance.CashCurrency;
        GameManager.Instance.CashCurrency = oldValue + value;
        
        _cashCurrencyText.text = GameManager.Instance.CashCurrency.ToString();
    }
}
