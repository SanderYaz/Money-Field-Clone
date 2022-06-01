using System;
using System.Linq;
using Data;
using Data.Object;
using DG.Tweening;
using Gameplay.Stack;
using Gameplay.Stack.Stackable;
using Interfaces;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerTrigger))]
    public class PlayerController : MonoBehaviour
    {
        public CapacityLevels[] capacityLevels;
        public RadiusLevels[] radiusLevels;

        public SpriteRenderer radiusDisplay;
        public Transform moneyDisplayTransform;
        public SpriteRenderer moneyDisplayFillBar;

        [ShowNativeProperty] public int CurrentCapacityLevel { get; private set; }
        [ShowNativeProperty] public int CurrentRadiusLevel { get; private set; }

        private PlayerSoundController playerSoundController;
        public PlayerMovement Movement { get; private set; }
        public Stacker Stacker { get; private set; }
        public PlayerTrigger Trigger { get; private set; }
        public PlayerAnimatorController AnimatorController { get; private set; }

        public PlayerSoundController PlayerSoundController
        {
            get => playerSoundController;
        }

        public void ResetBehaviour(bool initializeData)
        {
            Movement = GetComponent<PlayerMovement>();
            Trigger = GetComponent<PlayerTrigger>();
            Stacker = GetComponent<Stacker>();
            AnimatorController = GetComponent<PlayerAnimatorController>();

            Movement.Initialize(this);
            Trigger.Initialize(this);
            Stacker.Initialize(this);
            AnimatorController.Initialize(this);


            if (initializeData)
                Initialize();

            GameManager.Instance.CameraManager.SetTarget(transform);
        }

        private void Initialize()
        {
            CurrentCapacityLevel = GameManager.Instance.PlayerData.stackCapacityLevel;
            CurrentRadiusLevel = GameManager.Instance.PlayerData.radiusLevel;
            transform.position = Structures.Float3.ToVector3(GameManager.Instance.PlayerData.lastPositionInWorld);
            transform.eulerAngles = Structures.Float3.ToVector3(GameManager.Instance.PlayerData.lastEulerInWorld);
            var moneyCount = GameManager.Instance.PlayerData.currentMoney / GameManager.MoneyAmountBySingleMoney;

            Trigger.collectRadius = radiusLevels[CurrentRadiusLevel].radius;
            radiusDisplay.size = Vector2.one * (radiusLevels[CurrentRadiusLevel].radius * 2);
            radiusDisplay.transform.GetChild(0).transform.localScale = radiusDisplay.size / 2;
            Stacker.SetXZTransforms(capacityLevels[CurrentCapacityLevel].parent.Cast<Transform>().ToArray());
            AddMoneys(moneyCount);
        }

        private void AddMoneys(int moneyCount)
        {
            for (int i = 0; i < moneyCount; i++)
            {
                var stackablePrefab = GameManager.Instance.PoolManager.GetFromPool(0);
                var stackableBase = stackablePrefab.GetComponent<StackableBase>();

                var tStackable = stackablePrefab.transform;
                var gStackable = stackablePrefab.gameObject;
                tStackable.transform.position = transform.position;

                gStackable.layer = LayerMask.NameToLayer("Stacked");
                gStackable.SetActive(true);


                var localScaleOfMoney = Vector3.one * 0.4f;
                tStackable.localScale = localScaleOfMoney;
                // tStackable.DOPunchScale(Vector3.up * 0.4f, 0.5f, 10,1);
                Stacker.Stack(stackableBase);
            }
        }

        private void SpendMoneys(int moneyCount)
        {
            for (int i = 0; i < moneyCount; i++)
            {
                
                var stackableBase = Stacker.Stacks.Last();

                var tStackable = stackableBase.transform;
                var gStackable = stackableBase.gameObject;
                var oStackable = stackableBase.GetComponent<OPrefabInfo>();
                

                gStackable.layer = LayerMask.NameToLayer("Stackable");
                Stacker.Unstack(stackableBase);
                stackableBase.OnUnstacked();
                GameManager.Instance.PoolManager.RePoolObject(oStackable);
                
            }  
        }


        public void GiveMoneyToField(Field.Field field)
        {
            var stackable = Stacker.Stacks.Last();


            GameManager.Instance.EconomyManager.ForegoCurrency(CurrencyType.Money, GameManager.MoneyAmountBySingleMoney);
            field.TakeMoney(GameManager.MoneyAmountBySingleMoney);
            Stacker.Unstack(stackable);
            stackable.transform.DOJump(field.transform.position, 5, 1, 0.5f).OnComplete((() => { stackable.OnUnstacked(); }));
        }

        [Button]
        public void StackCapacityLevelUp()
        {
            
            CurrentCapacityLevel++;
            GameManager.Instance.EconomyManager.ForegoCurrency(CurrencyType.Money, capacityLevels[CurrentCapacityLevel].upgradePrice);
            SpendMoneys(capacityLevels[CurrentCapacityLevel].upgradePrice/GameManager.MoneyAmountBySingleMoney);
            Stacker.SetXZTransforms(capacityLevels[CurrentCapacityLevel].parent.Cast<Transform>().ToArray());
            GameManager.Instance.SavePlayerCapacityLevel(CurrentCapacityLevel);
        }

        [Button]
        public void CollectRadiusLevelUp()
        {
            CurrentRadiusLevel++;
            Trigger.collectRadius = radiusLevels[CurrentRadiusLevel].radius;


            radiusDisplay.size = Vector2.one * (radiusLevels[CurrentRadiusLevel].radius * 2);
            radiusDisplay.transform.GetChild(0).transform.localScale = radiusDisplay.size / 2;
            GameManager.Instance.EconomyManager.ForegoCurrency(CurrencyType.Money, radiusLevels[CurrentRadiusLevel].upgradePrice);
            SpendMoneys(radiusLevels[CurrentRadiusLevel].upgradePrice/GameManager.MoneyAmountBySingleMoney);
            GameManager.Instance.SavePlayerRadiusLevel(CurrentRadiusLevel);
        }
    }


    [Serializable]
    public class CapacityLevels
    {
        public Transform parent;
        public int upgradePrice;
        public int maxYCount;
    }

    [Serializable]
    public class RadiusLevels
    {
        public float radius;
        public int upgradePrice;
    }
}