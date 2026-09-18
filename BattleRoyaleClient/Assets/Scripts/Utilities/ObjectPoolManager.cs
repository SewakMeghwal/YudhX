using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Utilities
{
    public class ObjectPoolManager : MonoBehaviour, IObjectPool
    {
        private static ObjectPoolManager instance;
        private readonly Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
        private readonly Dictionary<int, int> instanceToPrefabKey = new Dictionary<int, int>();

        public static ObjectPoolManager Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void Prewarm(GameObject prefab, int count)
        {
            if (prefab == null || count <= 0) return;

            int key = prefab.GetInstanceID();
            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<GameObject>());
            }

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                poolDictionary[key].Enqueue(obj);
                instanceToPrefabKey[obj.GetInstanceID()] = key;
            }
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null) return null;

            int key = prefab.GetInstanceID();
            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<GameObject>());
            }

            GameObject spawnedObj;
            if (poolDictionary[key].Count > 0)
            {
                spawnedObj = poolDictionary[key].Dequeue();
            }
            else
            {
                spawnedObj = Instantiate(prefab, transform);
                instanceToPrefabKey[spawnedObj.GetInstanceID()] = key;
            }

            spawnedObj.transform.position = position;
            spawnedObj.transform.rotation = rotation;
            spawnedObj.SetActive(true);

            return spawnedObj;
        }

        public void ReturnToPool(GameObject instanceObj)
        {
            if (instanceObj == null) return;

            int objKey = instanceObj.GetInstanceID();
            if (instanceToPrefabKey.TryGetValue(objKey, out int prefabKey))
            {
                instanceObj.SetActive(false);
                instanceObj.transform.SetParent(transform);

                if (poolDictionary.TryGetValue(prefabKey, out var queue))
                {
                    queue.Enqueue(instanceObj);
                }
            }
            else
            {
                // Unregistered object fallback
                instanceObj.SetActive(false);
            }
        }
    }
}
