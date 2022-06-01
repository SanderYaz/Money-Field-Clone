using System.Collections.Generic;
using Data;
using UnityEngine;
using Util;

namespace Managers
{
    public class EconomyManager : MonoBehaviour
    {
        public Currency[] currencies;

        public readonly Dictionary<CurrencyType, int> CurrencyAmounts = new Dictionary<CurrencyType, int>();


        public void Initialize()
        {
            foreach (var t in currencies)
            {
                var value = GameManager.Instance.PlayerData.currentMoney;
                
                CurrencyAmounts.Add(t.currencyType, value);
                GameManager.Instance.UIManager.economyUI.UpdateCurrencyText(t.currencyType, value, value);
            }
        }
        
        public void EarnCurrency(CurrencyType currencyType, int amount)
        {
            var previousAmount = CurrencyAmounts[currencyType];
            CurrencyAmounts[currencyType] += amount;
            GameManager.Instance.SavePlayerMoney(CurrencyAmounts[currencyType]);
            GameManager.Instance.UIManager.economyUI.UpdateCurrencyText(currencyType, previousAmount, CurrencyAmounts[currencyType]);
        }
        
        public void ForegoCurrency(CurrencyType currencyType, int amount)
        {
            var previousAmount = CurrencyAmounts[currencyType];
            CurrencyAmounts[currencyType] -= CurrencyAmounts[currencyType] - amount >= 0 ? amount : 0;
            GameManager.Instance.SavePlayerMoney(CurrencyAmounts[currencyType]);
            GameManager.Instance.UIManager.economyUI.UpdateCurrencyText(currencyType, previousAmount, CurrencyAmounts[currencyType]);
        }
        
        public bool CheckForCurrency(CurrencyType currencyType, int amount)
        {
            return CurrencyAmounts[currencyType] >= amount;
        }
    }
}