using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;

namespace UDEV.GhostDefense
{
    public class UltiLighting : UltiFromSky
    {
        public override void DealDamage()
        {
            base.DealDamage();

            if(m_targets == null || m_targets.Count <= 0) return;

            for(int i = 0; i < GetCurTargetNum(); i++)
            {
                var target = m_targets[i];
                if (!target) continue;
                Vector3 spawnPos = new Vector3(target.transform.position.x, 0f, 0f);
                var lightingClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE,weapon,spawnPos,Quaternion.identity);
                if (!lightingClone) break;
                lightingClone.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                Projectile pComp = lightingClone.GetComponent<Projectile>();
                if (pComp)
                {
                    pComp.owenr = m_owner;
                    if(weaponSpeed > 0)
                    {
                        pComp.speed= weaponSpeed;
                    }
                    pComp.damage = m_owner.CurDmg;
                    pComp.damageTo = m_owner.damageTo;
                }
            }
        }
    }
}
