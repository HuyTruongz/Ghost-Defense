using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;
using System;

namespace UDEV.GhostDefense
{
    public class Projectile : MonoBehaviour, IDamageCreater
    {
        [Header("Base Setting: ")]
        public GameTag damageTo;
        public float speed;
        public float damage;
        public bool deactiveWhenHitted;

        [PoolerKeys(target = PoolerTarget.NONE)]
        public string bodyHitPool;

        [HideInInspector]
        public Actor owenr;

        private Vector2 m_prevPos;
        private RaycastHit2D m_hit;
        private Vector2 m_dir;

        private void OnEnable()
        {
            RefreshLastPos();
        }

        private void Update()
        {
            transform.Translate(transform.right * speed * Time.deltaTime,Space.World);
        }

        private void FixedUpdate()
        {
            DealDamage();

            RefreshLastPos();
        }

        public void DealDamage()
        {
            m_dir = (Vector2)transform.position - m_prevPos;
            float distance = m_dir.magnitude;
            m_dir.Normalize();

            m_hit = Physics2D.Raycast(m_prevPos, m_dir,distance);

            if (!m_hit || !m_hit.collider) return;

            var col = m_hit.collider;

            Cheking(col);
        }

        private void Cheking(Collider2D col)
        {
            if (col.CompareTag(damageTo.ToString()))
            {
                Actor actor = col.GetComponent<Actor>();

                if(!actor) return;

                actor.WhoHit = owenr;
                actor.TakeDamage(damage,owenr);

                //var rotate = transform.localScale.x < 0 ? Quaternion.Inverse(transform.rotation) : transform.rotation;

                PoolersManager.Ins.Spawn(PoolerTarget.NONE,bodyHitPool,m_hit.transform.position,Quaternion.identity);

                if (deactiveWhenHitted)
                {
                    gameObject.SetActive(false);
                }
             
            } 
        }

        private void RefreshLastPos()
        {
            m_prevPos = (Vector2)transform.position;
        }

        private void OnDisable()
        {
            m_hit = new RaycastHit2D();
            transform.position = new Vector3(1000f,1000f,0);
        }

    }
}
