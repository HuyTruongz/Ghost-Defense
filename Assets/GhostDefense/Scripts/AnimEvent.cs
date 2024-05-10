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
    }
}
