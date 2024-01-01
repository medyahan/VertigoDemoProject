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

    /// <summary>
    /// Updates the specified currency type with the given value and updates the corresponding UI text.
    /// </summary>
    /// <param name="currencyType">Type of the currency (Gold or Cash).</param>
    /// <param name="value">The value to add to the currency.</param>
    public void UpdateCurrency(CurrencyType currencyType, int value)
    {
        int oldValue;
        
        if (currencyType == CurrencyType.Gold)
        {
            oldValue = GameManager.Instance.GoldCurrency;
            GameManager.Instance.GoldCurrency = oldValue + value;

            _goldCurrencyText.text = GameManager.Instance.GoldCurrency.ToString();
            return;
        }

        if (currencyType == CurrencyType.Cash)
        {
            oldValue = GameManager.Instance.CashCurrency;
            GameManager.Instance.CashCurrency = oldValue + value;
        
            _cashCurrencyText.text = GameManager.Instance.CashCurrency.ToString();
        }
    }

    public enum CurrencyType
    {
        Cash,
        Gold,
    }
}
