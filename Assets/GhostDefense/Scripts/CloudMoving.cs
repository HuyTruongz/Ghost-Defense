using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CloudMoving : MonoBehaviour
    {
        public float speed;
        private Rigidbody2D m_rb;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
            float check = Random.Range(0f, 1f);
            speed = check > 0.5f ? speed : -speed;
        }

        private void FixedUpdate()
        {
            if (m_rb)
                m_rb.velocity = Vector2.right * speed;
        }
    }

}