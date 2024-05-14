using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;

namespace UDEV.GhostDefense
{
    public class FireProjectile : UltiController
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string projectilePool;
        public float speed;
        public Transform firePoint;

        public override void DealDamage()
        {
        
            GameObject p = PoolersManager.Ins.Spawn(PoolerTarget.NONE, projectilePool, firePoint.position, Quaternion.identity);
            if (!p || !m_owner) return;

            Projectile pComp = p.GetComponent<Projectile>();

            if (!pComp) return;

            pComp.damageTo = m_owner.damageTo;
            if (speed > 0)
            {
                pComp.speed = speed;

            }

            pComp.damage = m_owner.CurDmg;
            pComp.owenr = m_owner;

            if (m_owner.IsFacingLeft)
            {
                if(p.transform.localScale.x > 0)
                {
                    p.transform.localScale = new Vector3(
                        p.transform.localScale.x * -1,
                        p.transform.localScale.y,
                        p.transform.localScale.z);
                }

                if(pComp && pComp.speed > 0)
                {
                    pComp.speed *=- 1;
                }
            }
            else
            {
                if (p.transform.localScale.x < 0)
                {
                    p.transform.localScale = new Vector3(
                        p.transform.localScale.x * -1,
                        p.transform.localScale.y,
                        p.transform.localScale.z);
                }

                if (pComp && pComp.speed < 0)
                {
                    pComp.speed *= -1;
                }
            }

        }
    }
}
