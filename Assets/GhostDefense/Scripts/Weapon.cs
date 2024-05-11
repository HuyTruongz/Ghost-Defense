using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class Weapon : MonoBehaviour, IDamageCreater
    {
        [Header("Common:")]
        public GameTag damageTo;
        public LayerMask targetLayer;
        public float atkRadius;
        public Vector3 offset;
        [SerializeField]
        private Actor m_owner;
        public float damage;

        [Header("Cam Shake:")]
        public bool useCameraShake;
        public float shakeDur;
        public float shakeFreg;
        public float shakeAmpli;

        public Actor Owner { get => m_owner; set => m_owner = value; }

        private void Start()
        {
            if (m_owner == null) return;

            damageTo = m_owner.damageTo;

            damage = m_owner.CurDmg;
        }

        public void DealDamage()
        {
            if (m_owner == null) return;

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + offset,atkRadius,targetLayer); 

            if(cols == null || cols.Length <= 0) return;

            for (int i = 0; i < cols.Length; i++)
            {
                var col = cols[i];
                if(col == null) continue;
                if (col.gameObject.CompareTag(damageTo.ToString()))
                {
                    Actor actor = col.gameObject.GetComponent<Actor>();

                    if (actor)
                    {
                        actor.TakeDamage(damage,m_owner);
                    }
                }
            }

            if (useCameraShake)
            {
                CamShake.ins.ShakeTrigger(shakeDur,shakeFreg,shakeAmpli);
            }

            damage = m_owner.CurDmg;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Helper.ChangAlpha(Color.yellow,0.4f);
            Gizmos.DrawSphere(transform.position + offset,atkRadius);
        }
    }
}
