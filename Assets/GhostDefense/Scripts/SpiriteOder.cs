using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class SpiriteOder : MonoBehaviour
    {
        private SpriteRenderer m_sp;

        private void Awake()
        {
            m_sp = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (!m_sp) return;

            m_sp.sortingOrder = Pref.SpriteOrder++;
        }
    }
}
