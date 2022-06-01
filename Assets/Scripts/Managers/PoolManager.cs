using System.Collections.Generic;
using Data;
using Data.Object;
using UnityEngine;

namespace Managers
{
    public class PoolManager : MonoBehaviour
    {
        public GamePrefabsData prefabData;

        [SerializeField] private Transform poolParent;
        [SerializeField] private Transform runtimeParent;
        public List<OPrefabInfo> pooledObjects;
        public List<OPrefabInfo> currentObjects;
        public void Initialize()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            foreach (var pData in prefabData.PrefabDataList)
            {
                for (int i = 0; i < pData.size; i++)
                {
                    var obj = Instantiate(pData.prefab.gameObject, poolParent);
                    var prefabInfo = obj.GetComponent<OPrefabInfo>();
                    obj.SetActive(false);
                    pooledObjects.Add(prefabInfo);
                }
            }
        }


        public void RePoolObject(OPrefabInfo gameObjectToPool)
        {
            pooledObjects.Add(gameObjectToPool);
            if (currentObjects.Contains(gameObjectToPool)) currentObjects.Remove(gameObjectToPool);
            gameObjectToPool.transform.SetParent(poolParent);
            gameObjectToPool.gameObject.SetActive(false);
        }

        public OPrefabInfo GetFromPool(int objectID)
        {
            var oPrefabInfo = pooledObjects.Find(obj => obj.prefabID == objectID);

            if (oPrefabInfo != null)
            {
                
                oPrefabInfo.transform.SetParent(runtimeParent);
                pooledObjects.Remove(oPrefabInfo);
                currentObjects.Add(oPrefabInfo);
                return oPrefabInfo;
            }

            
             
            var data = prefabData.PrefabDataList.Find(pData => pData.id == objectID);
            var extendSize = data.size;
            Debug.Log($"No object in pool with id : {objectID}. Extending pool by it's own size : {extendSize}.");
            for (int i = 0; i < extendSize; i++)
            {
                var obj = Instantiate(data.prefab.gameObject, poolParent);
                var prefabInfo = obj.GetComponent<OPrefabInfo>();
                obj.SetActive(false);
                pooledObjects.Add(prefabInfo);
            }

            return GetFromPool(objectID);
        }
    }
}
