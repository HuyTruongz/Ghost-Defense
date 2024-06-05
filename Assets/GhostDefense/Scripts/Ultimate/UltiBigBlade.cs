using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;

namespace UDEV.GhostDefense
{
    public class UltiBigBlade : UltiController
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string weapon;
        public Transform createPoint;

        public override void DealDamage()
        {
            GameObject wpClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE,weapon,createPoint.position,Quaternion.identity);
            if (!wpClone) return;

            Weapon wpComp = wpClone.transform.GetChild(0).GetComponent<Weapon>();
            wpComp.Owner = m_owner;
            wpComp.damage = m_owner.CurDmg;

            if (m_owner.IsFacingLeft)
            {
                if(wpClone.transform.localScale.x > 0)
                {
                    wpClone.transform.localScale = new Vector3(
                        wpClone.transform.localScale.x * -1,
                        wpClone.transform.localScale.y,
                        wpClone.transform.localScale.z);
                }
            }
            else
            {
                if (wpClone.transform.localScale.x < 0)
                {
                    wpClone.transform.localScale = new Vector3(
                        wpClone.transform.localScale.x * -1,
                        wpClone.transform.localScale.y,
                        wpClone.transform.localScale.z);
                }
            }
        }
    }
}
