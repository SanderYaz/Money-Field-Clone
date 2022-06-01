using System;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.UIs
{
    public class InGameUI : UIBase
    {
        public ButtonLevel capacityButton;
        public ButtonLevel radiusButton;

        protected override void OnShow()
        {
            base.OnShow();
        }

        public void Initialize()
        {
            InitializeCapacityButton(GameManager.Instance.PlayerData);
            InitializeRadiusButton(GameManager.Instance.PlayerData);
        }

        private void InitializeRadiusButton(Structures.PlayerData playerData)
        {
            var player = GameManager.Instance.currentPlayer;
            
            if (playerData.radiusLevel >= player.radiusLevels.Length - 1)
            {
                //disable button
                radiusButton.button.gameObject.SetActive(false);
                return;
            }

            radiusButton.labelText.text = radiusButton.name;
            radiusButton.levelText.text = "LV" + (playerData.radiusLevel + 1) + " " + 
                                          "<color=green>►</color>" + " " + 
                                          "LV" + (playerData.radiusLevel + 2);

            radiusButton.priceText.text = "$" + Extensions.AbbreviationUtility.AbbreviateNumber(player.radiusLevels[playerData.radiusLevel + 1].upgradePrice);
            
            radiusButton.button.onClick.RemoveAllListeners();
            radiusButton.button.onClick.AddListener(RadiusButton_OnClick);
        }

        private void InitializeCapacityButton(Structures.PlayerData playerData)
        {
            var player = GameManager.Instance.currentPlayer;
            
            if (playerData.stackCapacityLevel >= player.capacityLevels.Length - 1)
            {
                //disable button
                capacityButton.button.gameObject.SetActive(false);
                return;
            }

            capacityButton.labelText.text = capacityButton.name;
            capacityButton.levelText.text = "LV" + (playerData.stackCapacityLevel + 1) + " " + 
                                            "<color=green>►</color>" + " " + 
                                            "LV" + (playerData.stackCapacityLevel + 2);

            capacityButton.priceText.text = "$" + Extensions.AbbreviationUtility.AbbreviateNumber(player.capacityLevels[playerData.stackCapacityLevel + 1].upgradePrice);
            
            capacityButton.button.onClick.RemoveAllListeners();
            capacityButton.button.onClick.AddListener(CapacityButton_OnClick);
            
        }

        private void Update()
        {
            CheckBalance();
        }

        private void CheckBalance()
        {
            if (GameManager.Instance.PlayerData.stackCapacityLevel < GameManager.Instance.currentPlayer.capacityLevels.Length - 1)
            {
                capacityButton.priceText.color =
                    GameManager.Instance.EconomyManager.CheckForCurrency
                        (CurrencyType.Money, GameManager.Instance.currentPlayer.capacityLevels[GameManager.Instance.PlayerData.stackCapacityLevel + 1].upgradePrice)
                        ? Color.green
                        : Color.red;
            }

            if (GameManager.Instance.PlayerData.radiusLevel < GameManager.Instance.currentPlayer.radiusLevels.Length - 1)
            {
                radiusButton.priceText.color =
                    GameManager.Instance.EconomyManager.CheckForCurrency
                        (CurrencyType.Money, GameManager.Instance.currentPlayer.radiusLevels[GameManager.Instance.PlayerData.radiusLevel + 1].upgradePrice)
                        ? Color.green
                        : Color.red;
            }
            
            
            
        }

        private void CapacityButton_OnClick()
        {
            if (!GameManager.Instance.EconomyManager.CheckForCurrency
                    (CurrencyType.Money, GameManager.Instance.currentPlayer.capacityLevels[GameManager.Instance.PlayerData.stackCapacityLevel + 1].upgradePrice)) return;
            
            GameManager.Instance.currentPlayer.StackCapacityLevelUp();
            InitializeCapacityButton(GameManager.Instance.PlayerData);
        }
        private void RadiusButton_OnClick()
        {
            if (!GameManager.Instance.EconomyManager.CheckForCurrency
                    (CurrencyType.Money, GameManager.Instance.currentPlayer.radiusLevels[GameManager.Instance.PlayerData.radiusLevel + 1].upgradePrice)) return;
            
            GameManager.Instance.currentPlayer.CollectRadiusLevelUp();
            InitializeRadiusButton(GameManager.Instance.PlayerData);
        }
    }

    [Serializable]
    public class ButtonLevel
    {
        public string name;
        public Button button;
        public TextMeshProUGUI labelText;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI priceText;
    }
}