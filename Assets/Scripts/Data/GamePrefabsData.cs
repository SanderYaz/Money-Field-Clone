using System.Collections.Generic;
using Data.Object;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GamePrefabsData", menuName = "Data/Game Prefabs Data")]
    public class GamePrefabsData : ScriptableObject
    {
        public List<PrefabData> PrefabDataList;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GamePrefabsData))]
    public class GamePrefabsDataEditor : Editor
    {
        private GamePrefabsData _target;

        public override void OnInspectorGUI()
        {
            if (_target == null)
                _target = (GamePrefabsData) target;
            base.OnInspectorGUI();

            foreach (var t in _target.PrefabDataList)
            {
                if (t.prefab != null)
                    t.id = t.prefab.prefabID;
            }
        }
    }

#endif
    [System.Serializable]
    public class PrefabData
    {
        public int id;
        public OPrefabInfo prefab;
        public int size;
    }
}