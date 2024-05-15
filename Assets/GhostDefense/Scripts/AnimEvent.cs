using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class AnimEvent : MonoBehaviour
    {
        public Actor owner;
        public GameObject weapon;
        public UltiManager ultiMng;

        private void Start()
        {
            if(!ultiMng|| !owner) return;

            ultiMng.Owner = owner;
        }

        public void Dash()
        {
            if (owner)
            {
                owner.Dash();
            }
        }

        public void WeaponAttack()
        {
            if (!weapon) return;

            UltiController ultiCtr = weapon.GetComponent<UltiController>();
            if (ultiCtr)
            {
                ultiCtr.Owner = owner;
            }

            IDamageCreater dmgCreater = weapon.GetComponent<IDamageCreater>();
            if (dmgCreater != null)
            {
                dmgCreater.DealDamage();
            }
        }

        public void UltiTrigger()
        {
            if (!ultiMng) return;
            ultiMng.UltiTrigger();
           
        }

        public void Deactive()
        {
            if(!owner) return;

            owner.gameObject.SetActive(false); 
        }

        public void PlayFootstepSound()
        {

        }
    }
}
