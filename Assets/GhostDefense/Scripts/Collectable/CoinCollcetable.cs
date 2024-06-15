using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class CoinCollcetable : Collectable
    {
        private bool m_canMove;

        private void Update()
        {
            if(!m_canMove || !m_isNotMoving) return;

            Vector3 destPoint = GUIManager.Ins.GetCoinIconUICorner();

            transform.position = Vector3.MoveTowards(transform.position,destPoint,200f * Time.deltaTime);

            if(Vector2.Distance(transform.position,destPoint) <= 0.1f)
            {
                transform.position = destPoint;
                gameObject.SetActive(false);
            }
        }

        protected override void TriggerCore()
        {
            m_canMove = true;
            GameManager.Ins.AddCoin(m_bonus);
        }
    }
}
