using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using Util;

namespace UI.UIs
{
    public class EconomyUI : UIBase
    {
        [SerializeField] private TextMeshPro[] currencyTexts;
        private readonly Dictionary<CurrencyType, TextMeshPro> _currencyTypesTexts = new Dictionary<CurrencyType, TextMeshPro>();

        public void Initialize()
        {
            for (var i = 0; i < GameManager.Instance.EconomyManager.currencies.Length; i++)
            {
                var economyManager = GameManager.Instance.EconomyManager;
                var currencyType = GameManager.Instance.EconomyManager.currencies[i].currencyType;
                _currencyTypesTexts.Add(currencyType, currencyTexts[i]);
                UpdateCurrencyText(currencyType, economyManager.CurrencyAmounts[currencyType], economyManager.CurrencyAmounts[currencyType]);
            }


            if (showMaxRoutine != null)
            {
                StopCoroutine(showMaxRoutine);
                showMaxRoutine = null;
            }


            showMaxRoutine = StartCoroutine(ShowMaxRoutine());
        }

        private Coroutine showMaxRoutine;

        /// <summary>
        /// Updates currency text by given currency type.
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="previousAmount"></param>
        /// <param name="currencyAmount"></param>
        public void UpdateCurrencyText(CurrencyType currencyType, float previousAmount, float currencyAmount)
        {
            var displayAmount = previousAmount;

            DOTween.To(() => displayAmount, x => displayAmount = x, currencyAmount, 1)
                .OnUpdate(() => { _currencyTypesTexts[currencyType].text = "$" + Extensions.AbbreviationUtility.AbbreviateNumber(displayAmount); });
        }

        private IEnumerator ShowMaxRoutine()
        {
            var showingMax = false;

            yield return new WaitForSeconds(0.5f);

            for (;;)
            {
                
                
                if (!GameManager.Instance.currentPlayer.Stacker.IsMax())
                {
                    _currencyTypesTexts[CurrencyType.Money].transform.DOKill();
                    _currencyTypesTexts[CurrencyType.Money].transform.localScale = Vector3.one;
                    _currencyTypesTexts[CurrencyType.Money].text = "$" + Extensions.AbbreviationUtility.AbbreviateNumber(GameManager.Instance.EconomyManager.CurrencyAmounts[CurrencyType.Money]);
                    _currencyTypesTexts[CurrencyType.Money].color = Color.white;
                }
                yield return new WaitUntil(() => GameManager.Instance.currentPlayer.Stacker.IsMax());
                yield return new WaitForSeconds(!showingMax ? 0.5f : 1.5f);
                _currencyTypesTexts[CurrencyType.Money].transform.DOKill();
                _currencyTypesTexts[CurrencyType.Money].transform.localScale = Vector3.one;
                var amount = GameManager.Instance.EconomyManager.CurrencyAmounts[CurrencyType.Money];
                if (!showingMax)
                {
                    _currencyTypesTexts[CurrencyType.Money].text = "$" + Extensions.AbbreviationUtility.AbbreviateNumber(amount);
                    _currencyTypesTexts[CurrencyType.Money].color = Color.white;
                }
                else
                {
                    _currencyTypesTexts[CurrencyType.Money].text = "MAX";
                    _currencyTypesTexts[CurrencyType.Money].transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 5, 1);
                    _currencyTypesTexts[CurrencyType.Money].color = Color.red;
                }

                showingMax = !showingMax;
            }
        }
    }
}