using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace UDEV.SPM
{
    public class PoolersManager : Singleton<PoolersManager>
    {
        public bool dontDestroyOnloadPool;

        public ObjectPooler[] objectPoolers;

        private List<PoolerItem> m_items;

        [System.Serializable]
        private class PoolerItem
        {
            public PoolerTarget target;
            public List<GameObject> pooledObjects;
            public PoolerItem()
            {

            }
            public PoolerItem(PoolerTarget _target, List<GameObject> _pooledObjects)
            {
                target = _target;
                pooledObjects = _pooledObjects;
            }
        }

        public override void Awake()
        {
            base.Awake();
            Init();
        }


        public void Init()
        {
            m_items = new List<PoolerItem>();
            
            if (objectPoolers != null && objectPoolers.Length > 0)
            {
                for (int i = 0; i < objectPoolers.Length; i++)
                {
                    if (objectPoolers[i] != null)
                    {
                        m_items.Add(new PoolerItem(objectPoolers[i].target, new List<GameObject>()));
                        var pools = objectPoolers[i].pools;
                        for (int j = 0; j < pools.Count; j++)
                        {
                            for (int k = 0; k < pools[j].startingQuantity; k++)
                            {
                                GameObject o = Instantiate(pools[j].prefab, Vector3.zero, Quaternion.identity, pools[j].startingParent ? pools[j].startingParent : null);
                                Pooler pooler = o.GetComponent<Pooler>();
                                if (!pooler)
                                {
                                    o.AddComponent<Pooler>();
                                    pooler.id = pools[j].id;
                                }
                                o.SetActive(false);
                                m_items[i].pooledObjects.Add(o);
                            }
                        }
                    }
                }
            }
        }

        public GameObject Spawn(PoolerTarget target, string poolId, Vector3 position, Quaternion rotation, Transform parentTransform = null)
        {
            var poolers = objectPoolers.Where(t => t.target == target).ToArray();

            GameObject pool = null;

            if (poolers != null && poolers.Length > 0)
            {
                for (int i = 0; i < poolers.Length; i++)
                {
                    if (poolers[i] != null)
                    {
                        var poolerObjs = GetPooleds(poolers[i].target);
                        pool = poolers[i].Spawn(poolerObjs ,poolId, position, rotation, parentTransform);

                        if (dontDestroyOnloadPool && pool)
                            DontDestroyOnLoad(pool);
                    }

                    if (pool) return pool;
                }
            }
            else
            {
                Debug.LogWarning("Pooler for " + target.ToString() + " is Null.Please add it to Pooler Manager!.");
            }

            if (pool == null)
            {
                Debug.LogWarning("Pool key does not exist in pooler for " + target.ToString());
            }

            return pool;
        }

        public GameObject GetPrefab(PoolerTarget target, string poolId)
        {
            var poolers = objectPoolers.Where(t => t.target == target).ToArray();

            GameObject pool = null;

            if (poolers != null && poolers.Length > 0)
            {
                for (int i = 0; i < poolers.Length; i++)
                {
                    ObjectPooler.Pool[] findeds = new ObjectPooler.Pool[] { };

                    if (poolers[i] != null)
                    {
                        findeds = poolers[i].pools.Where(p => string.Compare(p.id, poolId) == 0).ToArray();

                        if (findeds != null && findeds.Length > 0)
                        {
                            pool = findeds[0].prefab;
                        }
                    }

                    if (pool) return pool;
                }
            }
            else
            {
                Debug.LogWarning("Pooler for " + target.ToString() + " is Null.Please add it to Pooler Manager!.");
            }

            if (pool == null)
            {
                Debug.LogWarning("Pool key does not exist in pooler for " + target.ToString());
            }

            return pool;
        }

        public void Clear(PoolerTarget target)
        {
            var poolers = objectPoolers.Where(t => t.target == target).ToArray();

            if (poolers != null && poolers.Length > 0)
            {
                for (int i = 0; i < poolers.Length; i++)
                {
                    if (poolers[i] != null)
                    {
                        var poolerObjs = GetPooleds(poolers[i].target);
                        poolers[i].ClearPooledObjects(ref poolerObjs);
                    }
                }
            }
        }

        public void ClearAll()
        {
            if (objectPoolers != null && objectPoolers.Length > 0)
            {
                for (int i = 0; i < objectPoolers.Length; i++)
                {
                    if (objectPoolers[i] != null)
                    {
                        var poolerObjs = GetPooleds(objectPoolers[i].target);
                        objectPoolers[i].ClearPooledObjects(ref poolerObjs);
                    }
                }
            }
        }

        public void RollBackAllToPools()
        {
            if (objectPoolers != null && objectPoolers.Length > 0)
            {
                for (int i = 0; i < objectPoolers.Length; i++)
                {
                    if (objectPoolers[i] != null)
                    {
                        var poolerObjs = GetPooleds(objectPoolers[i].target);
                        objectPoolers[i].BackToPools(poolerObjs);
                    }
                }
            }
        }

        public List<GameObject> GetPooleds(PoolerTarget target)
        {
            var findeds = m_items.Where(p => p.target == target).ToList();
            if (findeds != null && findeds.Count > 0)
                return findeds[0].pooledObjects;
            return null;
        }
    }
}