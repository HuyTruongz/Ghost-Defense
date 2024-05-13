using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UDEV.GhostDefense
{
    public class UltiManager : MonoBehaviour
    {
        public UltiController[] ultiCtrs;

        private Actor m_owner;

        public Actor Owner { get => m_owner; set => m_owner = value; }

        public void UltiTrigger()
        {
            float rateCheking = Random.Range(0f,1f);
            var finder = ultiCtrs.Where(u => u.rate > rateCheking);

            if (finder == null) return;

            var rs = finder.ToArray();

            if(rs == null || rs.Length <= 0 ) return;

            int randIdx = Random.Range(0,rs.Length);

            var ultiCtr = rs[randIdx];

            if(!ultiCtr) return;

            ultiCtr.Owner = m_owner;
            ultiCtr.DealDamage();
            CamShake.ins.ShakeTrigger(0.2f,0.3f);
        }
    }
}
