using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter
{
    [System.Serializable]
    public class PoolData
    {
        public ObjectData Data
        {
            get => poolData;
            private set => poolData = value;
        }

        public List<GameObject> SpawnPool
        {
            get => spawnPool;
            private set => spawnPool = value;
        }

        public int CurrentIndex
        {
            get => currentIndex;
            set => currentIndex = value;
        }

        public bool IsInitialized
        {
            get => isInitialized;
            private set => isInitialized = value;
        }


        [SerializeField] private ObjectData poolData;
        private List<GameObject> spawnPool;
        private int currentIndex;
        private bool isInitialized;

        public PoolData(ObjectData data)
        {
            poolData = data;
            spawnPool = new List<GameObject>();
            currentIndex = 0;
            isInitialized = false;
        }

        public void Initialize(GameObject poolParent)
        {
            if (!poolData?.Object) return;
            if (poolData.PoolAmount <= 0) return;
            if (spawnPool == null) spawnPool = new List<GameObject>();

            isInitialized = true;

            for (int j = 0; j < Data.PoolAmount; j++)
            {
                // Instantiate the object and set the pool parent
                GameObject obj = GameObject.Instantiate(Data.Object, poolParent.transform);
                // Add pooled object component for identifier
                PooledObject pooledObject = obj.AddComponent<PooledObject>();
                // Set the identifier and parent
                pooledObject.Identifier = Data.Identifier;
                pooledObject.OriginalParent = poolParent.transform;
                // Add to the spawn pool
                SpawnPool.Add(obj);
                // Set active to false
                obj.SetActive(false);
            }

        }

        public GameObject GetInstance()
        {
            GameObject obj = spawnPool[currentIndex];

            currentIndex += 1;
            if (currentIndex >= poolData.PoolAmount)
            {
                currentIndex = 0;
            }

            if (!obj)
            {
                Debug.LogWarning("Game Object not found!");
            }

            return obj;
        }
    }
}
