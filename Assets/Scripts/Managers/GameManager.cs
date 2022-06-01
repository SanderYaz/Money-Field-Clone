using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Gameplay.Field;
using Gameplay.Player;
using NaughtyAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using UnityEngine;
using Util;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public static readonly int MoneyAmountBySingleMoney = 10;
        public PlayerController currentPlayer;
        public Field[] fields;

        public Structures.PlayerData PlayerData { get; private set; }
        public Structures.FieldData FieldData { get; private set; }

        #region Unity

        private void Awake()
        {
            Instance = this;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            AotHelper.EnsureList<Structures.FieldData>();
            AotHelper.EnsureList<Structures.Field>();
            AotHelper.EnsureList<Structures.PlayerData>();
            AotHelper.EnsureList<Structures.Float3>();


            EconomyManager = GetComponent<EconomyManager>();
            UIManager = GetComponent<UIManager>();
            PoolManager = GetComponent<PoolManager>();
            DataManager = GetComponent<DataManager>();
            CameraManager = GetComponent<CameraManager>();

           
        }

        private void Start()
        {
            DataManager.Initialize();
            PoolManager.Initialize();


            InitializeGameElements();

            EconomyManager.Initialize();
            UIManager.Initialize();
        }


        private void Update()
        {
#if UNITY_EDITOR


            ManipulateGame();


#endif
        }

        #endregion

        #region Methods

        private void InitializeGameElements()
        {
            FieldData = GetFieldData();
            PlayerData = GetPlayerData();


            InitializeFields();
            currentPlayer.ResetBehaviour(true);
        }

        private void InitializeFields()
        {
            foreach (var field in fields)
            {
                if (field == null) continue;
                var dataOfField = Array.Find(FieldData.fields, f => f.Name == field.gameObject.name);
                if (dataOfField == null)
                {
                    var newFieldData = new Structures.Field
                    {
                        Name = field.gameObject.name,
                        Unlocked = field.defaultUnlocked,
                        TookMoney = 0
                    };
                    var array = FieldData.fields.Concat(new[] {newFieldData}).ToArray();
                    FieldData.fields = array;

                    dataOfField = newFieldData;

                    var json = JsonConvert.SerializeObject(FieldData);
                    var path = DataManager.CombinePath("/" + "FieldData" + ".json");
                    DataManager.WriteToJson(path, json);
                }


                var index = Array.IndexOf(FieldData.fields, dataOfField);

                field.Initialize(dataOfField.Unlocked, dataOfField.TookMoney, index);
            }
        }


        private Structures.FieldData GetFieldData()
        {
            if (DataManager.CheckFileExists(DataManager.CombinePath("/" + "FieldData" + ".json")))
            {
                return FieldData = JsonConvert.DeserializeObject<Structures.FieldData>(DataManager.ReadFromJson(DataManager.CombinePath("/" + "FieldData" + ".json")));
            }

            FieldData = new Structures.FieldData
            {
                fields = fields.Select(field => new Structures.Field()
                {
                    Name = field.gameObject.name, Unlocked = field.defaultUnlocked,
                    TookMoney = 0
                }).ToArray(),
            };

            var json = JsonConvert.SerializeObject(FieldData);
            var path = DataManager.CombinePath("/" + "FieldData" + ".json");
            DataManager.WriteToJson(path, json);

            return FieldData;
        }

        public void SaveFieldData(int indexOfData, bool unlocked, int tookMoney)
        {
            FieldData.fields[indexOfData].Unlocked = unlocked;
            FieldData.fields[indexOfData].TookMoney = tookMoney;

            var json = JsonConvert.SerializeObject(FieldData);
            var path = DataManager.CombinePath("/" + "FieldData" + ".json");
            DataManager.WriteToJson(path, json);
        }


        private Structures.PlayerData GetPlayerData()
        {
            if (DataManager.CheckFileExists(DataManager.CombinePath("/" + "PlayerData" + ".json")))
            {
                return PlayerData = JsonConvert.DeserializeObject<Structures.PlayerData>(DataManager.ReadFromJson(DataManager.CombinePath("/" + "PlayerData" + ".json")));
            }

            PlayerData = new Structures.PlayerData
            {
                stackCapacityLevel = 0,
                radiusLevel = 0,
                currentMoney = 0,
                lastPositionInWorld = default,
                lastEulerInWorld = default,
            };

            var json = JsonConvert.SerializeObject(PlayerData);
            var path = DataManager.CombinePath("/" + "PlayerData" + ".json");
            DataManager.WriteToJson(path, json);

            return PlayerData;
        }

        public void SavePlayerCapacityLevel(int value)
        {
            PlayerData.stackCapacityLevel = value;

            var json = JsonConvert.SerializeObject(PlayerData);
            var path = DataManager.CombinePath("/" + "PlayerData" + ".json");
            DataManager.WriteToJson(path, json);
        }

        public void SavePlayerRadiusLevel(int value)
        {
            PlayerData.radiusLevel = value;

            var json = JsonConvert.SerializeObject(PlayerData);
            var path = DataManager.CombinePath("/" + "PlayerData" + ".json");
            DataManager.WriteToJson(path, json);
        }

        public void SavePlayerMoney(int value)
        {
            PlayerData.currentMoney = value;

            var json = JsonConvert.SerializeObject(PlayerData);
            var path = DataManager.CombinePath("/" + "PlayerData" + ".json");
            DataManager.WriteToJson(path, json);
        }

        public void SavePlayerLastPosition()
        {
            PlayerData.lastPositionInWorld = Structures.Float3.FromVector3(currentPlayer.transform.position);
            PlayerData.lastEulerInWorld = Structures.Float3.FromVector3(currentPlayer.transform.eulerAngles);

            var json = JsonConvert.SerializeObject(PlayerData);
            var path = DataManager.CombinePath("/" + "PlayerData" + ".json");
            DataManager.WriteToJson(path, json);
        }

        #endregion


        [Button]
        public void FindFields()
        {
            fields = FindObjectsOfType<Field>();
        }

        private void ManipulateGame()
        {
            #region Time Manipulation

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 4;
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Time.timeScale = 5;
            }

            #endregion
        }

        public UIManager UIManager { get; private set; }

        public PoolManager PoolManager { get; private set; }

        public DataManager DataManager { get; private set; }


        public CameraManager CameraManager { get; private set; }
        public EconomyManager EconomyManager { get; private set; }
    }
}