using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;

namespace UDEV.GhostDefense
{
    public class UltiSpear : UltiFromSky
    {
        public override void DealDamage()
        {
            base.DealDamage();

            if (m_targets == null || m_targets.Count <= 0) return;

            int randIdx = 0;
            int curTargetNum = GetCurTargetNum();

            while(curTargetNum > 0)
            {
                randIdx = Random.Range(0, m_targets.Count);
                var target = m_targets[randIdx];
                if(!target) continue;
                Vector3 spawnPos = new Vector3(Random.Range(-9, 9), 9, 0f);
                var spearClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE, weapon, spawnPos, Quaternion.identity);
                if (!spearClone) break;
                var dirToTarget = target.transform.position - spearClone.transform.position;
                dirToTarget.Normalize();
                float angle = Mathf.Atan2(dirToTarget.y,dirToTarget.x) * Mathf.Rad2Deg;
                spearClone.transform.rotation = Quaternion.Euler(0f,0f,angle);

                Projectile pComp = spearClone.GetComponent<Projectile>();
                if (pComp)
                {
                    pComp.owenr = m_owner;
                    pComp.damage = m_owner.CurDmg;
                    if(weaponSpeed > 0)
                    {
                        pComp.speed = weaponSpeed;
                    }
                    pComp.damageTo = m_owner.damageTo;
                }

                m_targets.RemoveAt(randIdx);
                curTargetNum--;
            }
        }
    }
}
