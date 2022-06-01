using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Currency", menuName = "Data/Currency/New Currency")]
    public class Currency : ScriptableObject
    {
        public CurrencyType currencyType;
        public UnityEngine.Sprite currencySprite;
    }

    public enum CurrencyType
    {
        Money,
    
    }
}