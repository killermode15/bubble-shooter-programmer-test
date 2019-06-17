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



        [SerializeField] private ObjectData poolData;
        private List<GameObject> spawnPool;
        private int currentIndex;

        public PoolData(ObjectData data)
        {
            poolData = data;
            spawnPool = new List<GameObject>();
            currentIndex = 0;
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
