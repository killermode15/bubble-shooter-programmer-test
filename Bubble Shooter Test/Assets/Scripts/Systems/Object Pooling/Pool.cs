using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BubbleShooter
{
    public class Pool : MonoBehaviour
    {
        // List of objects to pool
        [SerializeField] private List<ObjectData> objectData = new List<ObjectData>();

        // List of object pool data
        private List<PoolData> poolData;

        private void Start()
        {
            // Initialize object pool data list
            poolData = new List<PoolData>();
            foreach (ObjectData data in objectData)
            {
                PoolData pd = new PoolData(data);
                poolData.Add(pd);
            }

            // Spawn the object data for each pool data
            foreach (PoolData data in poolData)
            {
                // Make a parent for all the pooled object under this pool data
                GameObject poolParent = new GameObject("Object Pool [" + data.Data.Identifier + "]");

                // Initialize pool data with values
                data.Initialize(poolParent);
            }
        }

        public GameObject Instantiate(string objIdentifier, Transform parent)
        {
            PoolData pd = GetPoolData(objIdentifier);

            if (pd == null)
            {
                throw new NullReferenceException("The identifier [" + objIdentifier + "] couldn't be found");
            }
            if (!pd.Data.Object)
            {
                throw new NullReferenceException("The object for [" + objIdentifier + "] is null");
            }
            if (!parent)
            {
                Debug.Log("Parent is null");
            }


            GameObject spawnedObject = pd.GetInstance();
            spawnedObject?.SetActive(true);
            spawnedObject?.transform.SetParent(parent);
            if (spawnedObject != null)
            {
                spawnedObject.transform.localPosition = Vector3.zero;
            }
            return spawnedObject;
        }

        public GameObject Instantiate(string objIdentifier, Vector3 position, Quaternion rotation)
        {
            PoolData pd = GetPoolData(objIdentifier);

            if (pd == null)
            {
                throw new NullReferenceException("The identifier [" + objIdentifier + "] couldn't be found");
            }

            if (!pd.Data.Object)
            {
                throw new NullReferenceException("The object for [" + objIdentifier + "] is null");
            }


            GameObject spawnedObject = pd.GetInstance();
            spawnedObject?.SetActive(true);

            if (spawnedObject == null) throw new NullReferenceException("");

            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = rotation;
            //pd.CurrentIndex++;
            return spawnedObject;
        }

        public void Destroy(GameObject obj)
        {
            PoolData pd = GetPoolData(obj);
            PooledObject po = obj.GetComponent<PooledObject>();
            obj.SetActive(false);

            if (obj.transform.parent != po.OriginalParent)
            {
                obj.transform.SetParent(po.OriginalParent);
            }
        }

        public void Destroy(GameObject obj, float delay)
        {
            StartCoroutine(DestroyCR(obj, delay));
        }

        public void ResetPool(string objIdentifier)
        {
            PoolData poolData = GetPoolData(objIdentifier);

            foreach (GameObject poolObject in poolData.SpawnPool)
            {
                poolObject.transform.position = Vector3.zero;
                poolObject.SetActive(false);
            }
        }

        public bool IsInitialized(string objIdentifier)
        {
            return GetPoolData(objIdentifier).IsInitialized;
        }

        private IEnumerator DestroyCR(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!obj) yield break;
            Destroy(obj);
        }

        private PoolData GetPoolData(string objIdentifier)
        {
            return poolData.Find(x => x.Data.Identifier == objIdentifier);
        }

        private PoolData GetPoolData(GameObject obj)
        {
            return poolData.Find(x => x.Data.Identifier == obj.GetComponent<PooledObject>().Identifier);
        }
    }
}