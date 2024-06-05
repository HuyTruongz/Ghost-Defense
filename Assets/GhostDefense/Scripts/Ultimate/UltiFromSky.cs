using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;
using System;

namespace UDEV.GhostDefense
{
    public class UltiFromSky : UltiController
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string weapon;
        public int targetNum;
        public LayerMask targetLayer;
        public float atkRadius;
        public float waeponSpeed;
        protected List<AI> m_targets = new List<AI>();

        public override void DealDamage()
        {
            FindTargets();
        }

        protected void FindTargets()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position,atkRadius,targetLayer);
            if(cols == null || cols.Length <= 0) return;
            for (int i = 0; i < cols.Length; i++)
            {
                var col = cols[i];
                if(col == null) continue;
                if (col.CompareTag(m_owner.damageTo.ToString()))
                {
                    AI aiComp = col.GetComponent<AI>();
                    if(!aiComp) continue;
                    m_targets.Add(aiComp);
                }
            }
        }

        protected int GetCurTargetNum()
        {
            int curentTargetNum = 0;
            curentTargetNum = m_targets.Count > targetNum ? targetNum : m_targets.Count;
            return curentTargetNum;
        }
    }
}
