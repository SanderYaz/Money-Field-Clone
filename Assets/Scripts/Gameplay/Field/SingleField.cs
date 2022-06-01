using Data;
using Data.Object;
using DG.Tweening;
using Gameplay.Player;
using Gameplay.Stack;
using Gameplay.Stack.Stackable;
using Managers;
using UnityEngine;

namespace Gameplay.Field
{
    public class SingleField : MonoBehaviour
    {
        public bool growth;

        public Renderer rend;

        public StackableBase stackableBase;

        public void Grow()
        {
            if (growth && stackableBase != null) return;
            var stackablePrefab = GameManager.Instance.PoolManager.GetFromPool(0);
            stackableBase = stackablePrefab.GetComponent<StackableBase>();

            var tStackable = stackablePrefab.transform;
            var gStackable = stackablePrefab.gameObject;


            gStackable.layer = LayerMask.NameToLayer("Stackable");
            gStackable.SetActive(true);


            var localScaleOfMoney = Vector3.one * 0.4f;
            tStackable.localScale = localScaleOfMoney;
            tStackable.eulerAngles = new Vector3(0, 45, 15);
            tStackable.position = transform.position + Vector3.up * 1;
            tStackable.SetParent(transform);
            tStackable.DOPunchScale(Vector3.up * 0.4f, 0.5f, 10, 1);
            tStackable.DOMove(transform.position + Vector3.up * 1, 1);

            growth = true;
        }

        public void Crop(PlayerController _controller)
        {
            GameManager.Instance.EconomyManager.EarnCurrency(CurrencyType.Money, GameManager.MoneyAmountBySingleMoney);
            _controller.Stacker.Stack(stackableBase);
            stackableBase.transform.DOKill();
            stackableBase.transform.localScale = Vector3.one * 0.4f;
            stackableBase.transform.DOPunchScale(Vector3.right * 0.5f, 0.5f, 6, 1);
            growth = false;
            stackableBase = null;
        }

        public void Toggle(bool enable)
        {
            rend.enabled = enable;
        }
    }
}