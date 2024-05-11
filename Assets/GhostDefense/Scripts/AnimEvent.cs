using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class AnimEvent : MonoBehaviour
    {
        public Actor actor;
        public GameObject weapon;

        private void Start()
        {
            
        }

        public void Dash()
        {
            if (actor)
            {
                actor.Dash();
            }
        }

        public void WeaponAttack()
        {
            if (!weapon) return;

            IDamageCreater dmgCreater = weapon.GetComponent<IDamageCreater>();
            if (dmgCreater != null)
            {
                dmgCreater.DealDamage();
            }
        }

        public void Deactive()
        {
            if(!actor) return;

            actor.gameObject.SetActive(false); 
        }
    }
}
