using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class HpCollectable : Collectable
    {
        protected override void TriggerCore()
        {
            GameManager.Ins.AddHp(m_bonus);
        }
    }
}
