using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using NaughtyAttributes;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {

        public void Initialize()
        {

        }

        #region Getting Data

        public int GetData(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
        public string GetData(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
        public float GetData(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static bool CheckFileExists(string path)
        {
            return File.Exists(path);
        }

        public static string CombinePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath + fileName);
        }

        public static string ReadFromJson(string path)
        {
            return File.ReadAllText(path);
        }



        #endregion


        #region Setting Data
        public void SetData(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        public void SetData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
        public void SetData(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
        public static void WriteToJson(string path, string json)
        {
            File.WriteAllText(path, json);
        }
        #endregion

        [Button]
        public void DeleteAllData()
        {
            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                var fileInfo = new FileInfo(file);
                fileInfo.Delete();
            }
        }


    }
}