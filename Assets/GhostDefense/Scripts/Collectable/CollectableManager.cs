using System.Collections;
using System.Collections.Generic;
using UDEV.SPM;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class CollectableManager : Singleton<CollectableManager>
    {
        public CollectableItem[] items;

        public void Spawn(Vector3 pos)
        {
            if (items == null || items.Length <= 0) return;

            float rateChecking = Random.Range(0f, 1f);

            for (int i = 0; i < items.Length; i++)
            {
                CollectableItem item = items[i];
                if (item == null) continue;

                if (item.spawnRate >= rateChecking )
                {
                    for (int j = 0; j < item.amount; j++)
                    {
                        GameObject c = PoolersManager.Ins.Spawn(PoolerTarget.NONE,item.collectablePool,pos,Quaternion.identity);
                        if (c)
                        {
                            Collectable cComp = c.GetComponent<Collectable>();
                            if (cComp)
                            {
                                cComp.Init();
                            }
                        }
                    }
                }
            }
        }
    }
}
