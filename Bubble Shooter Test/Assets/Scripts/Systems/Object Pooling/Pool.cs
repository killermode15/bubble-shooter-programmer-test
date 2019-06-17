using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BubbleShooter
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private List<ObjectData> objectData;

        private List<PoolData> poolData;

        private void Start()
        {
            poolData = new List<PoolData>();
            foreach (ObjectData data in objectData)
            {
                PoolData pd = new PoolData(data);
                poolData.Add(pd);
            }

            foreach (PoolData data in poolData)
            {
                GameObject poolParent = new GameObject("Object Pool [" + data.Data.Identifier + "]");

                for (int j = 0; j < data.Data.PoolAmount; j++)
                {
                    GameObject obj = Instantiate(data.Data.Object, poolParent.transform);
                    PooledObject pooledObject = obj.AddComponent<PooledObject>();
                    pooledObject.Identifier = data.Data.Identifier;
                    pooledObject.OriginalParent = poolParent.transform;
                    data.SpawnPool.Add(obj);
                    obj.SetActive(false);
                }
            }
        }

        public GameObject Instantiate(string objIdentifier, Transform parent)
        {
            PoolData pd = GetPoolData(objIdentifier);

            if (pd == null)
            {
                Debug.LogWarning("The identifier [" + objIdentifier + "] couldn't be found", this);
                return null;
            }


            GameObject spawnedObject = pd.GetInstance();
            spawnedObject.SetActive(true);
            spawnedObject.transform.SetParent(parent);
            spawnedObject.transform.localPosition = Vector3.zero;
            return spawnedObject;
        }

        public GameObject Instantiate(string objIdentifier, Vector3 position, Quaternion rotation)
        {
            PoolData pd = GetPoolData(objIdentifier);

            if (pd == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("The identifier [");
                sb.Append(objIdentifier);
                sb.Append("] couldn't be found");
                Debug.LogWarning(sb.ToString(), this);
                return null;
            }


            GameObject spawnedObject = pd.GetInstance();
            spawnedObject.SetActive(true);
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

        public IEnumerator Destroy(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
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