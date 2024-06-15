using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Object Pooler
/// - allows the reuse of frequently "spawned" objects for optimization
/// </summary>

namespace UDEV.SPM
{
    [CreateAssetMenu(fileName = "NEW_POOLER", menuName = "UDEV/SPM/Create Pooler")]
    public class ObjectPooler : ScriptableObject
    {
        [UniqueId]
        public string id;
        public PoolerTarget target;

        [System.Serializable]
        public class PoolerCategory
        {
            public string name;
            [UniqueId]
            public string id;


            public PoolerCategory() { }
            public PoolerCategory(string _name, string _id)
            {
                name = _name;
                id = _id;
            }
        }

        public List<PoolerCategory> categories = new List<PoolerCategory>();

        [System.Serializable]
        public class Pool
        {
            public string poolName;
            [UniqueId]
            public string id;
            [HideInInspector]
            public GameObject prefab;
            public Transform startingParent;
            public int startingQuantity = 10;
            [PoolerCategory]
            public string category;

            public Pool(string _poolName, GameObject _prefab, Transform _startingParent, int _startingQuantity, string _category)
            {
                poolName = _poolName;
                prefab = _prefab;
                startingParent = _startingParent;
                startingQuantity = _startingQuantity;
                category = _category;
            }
        }
        public List<Pool> pools;

        public GameObject Spawn(List<GameObject> pooledObjects, string poolId, Vector3 position, Quaternion rotation, Transform parentTransform = null)
        {
            if (IsPoolerExist(poolId))
            {
                // Find the pool that matches the pool name:
                int pool = 0;
                for (int i = 0; i < pools.Count; i++)
                {
                    if (string.Compare(pools[i].id, poolId) == 0)
                    {
                        pool = i;
                        break;
                    }
                    if (i == pools.Count - 1)
                    {
                        Debug.LogError("There's no pool named \"" + poolId + "\"! Check the spelling or add a new pool with that name.");
                        return null;
                    }
                }

                var findeds = pooledObjects.Where(
                    p => p && string.Compare(p.GetComponent<Pooler>().id, pools[pool].id) == 0).ToList();
                
                if(findeds != null && findeds.Count > 0)
                {
                    for (int i = 0; i < findeds.Count; i++)
                    {
                        if (findeds[i] &&
                            !findeds[i].activeSelf)
                        {
                            // Set active:
                            findeds[i].SetActive(true);
                            findeds[i].transform.localPosition = position;
                            findeds[i].transform.localRotation = rotation;
                            // Set parent:
                            if (parentTransform)
                            {
                                findeds[i].transform.SetParent(parentTransform, false);
                            }

                            return findeds[i];
                        }
                    }
                }

                // If there's no game object available then expand the list by creating a new one:
                GameObject o = null;
                if (pools[pool].prefab)
                {
                    o = Instantiate(pools[pool].prefab, position, rotation);
                    Pooler pooler = o.GetComponent<Pooler>();
                    if(!pooler)
                    {
                        o.AddComponent<Pooler>();
                        o.GetComponent<Pooler>().id = pools[pool].id;
                    }
                    pooledObjects.Add(o);
                }

                // Add newly instantiated object to pool:
                return o;
            }

            return null;
        }

        public void ClearPooledObjects(ref List<GameObject> pooledObjects)
        {
            pooledObjects.Clear();
        }

        public void BackToPools(List<GameObject> pooledObjects)
        {
            if (pooledObjects != null && pooledObjects.Count > 0)
            {
                for (int i = 0; i < pooledObjects.Count; i++)
                {
                    if (pooledObjects[i] != null)
                    {
                        pooledObjects[i].SetActive(false);
                    }
                }
            }
        }

        public Dictionary<string, string> GetPoolIds()
        {
            Dictionary<string, string> ids = new Dictionary<string, string>();

            if (pools != null && pools.Count > 0)
            {
                for (int i = 0; i < pools.Count; i++)
                {
                    if (pools[i] != null)
                    {
                        if (!string.IsNullOrEmpty(pools[i].id))
                        {
                            ids[pools[i].id] = GetCategoryName(pools[i].category) + "/" + pools[i].poolName;
                        }
                    }
                }
            }

            return ids;
        }

        string GetCategoryName(string id)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i] != null && string.Compare(categories[i].id, id) == 0)
                {
                    return categories[i].name;
                }
            }

            return "";
        }

        bool IsPoolerExist(string id)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i] != null && string.Compare(pools[i].id, id) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

