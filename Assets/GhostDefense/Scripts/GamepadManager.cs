using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;

namespace UHUY.GhostDefense
{
    public class GamepadManager : Singleton<GamepadManager>
    {
        public Joystick joystick;
        private bool m_canMoveLeft;
        private bool m_canMoveRight;
        private bool m_canAttack;
        private bool m_canDash;
        private bool m_canUlti;

        private bool m_isStatic;

        public bool CanMoveLeft { get => m_canMoveLeft; set => m_canMoveLeft = value; }
        public bool CanMoveRight { get => m_canMoveRight; set => m_canMoveRight = value; }
        public bool CanAttack { get => m_canAttack; set => m_canAttack = value; }
        public bool CanDash { get => m_canDash; set => m_canDash = value; }
        public bool CanUlti { get => m_canUlti; set => m_canUlti = value; }

        public bool IsStatic
        {
            get => !m_canMoveLeft && !m_canMoveRight && !m_canAttack && !m_canUlti && !m_canDash;
        }

        public override void Awake()
        {
            MakeSingleton(false);
        }

        private void Update()
        {
            if (!GameManager.Ins.setting.isOnMobile)
            {
                float hozChecking = Input.GetAxisRaw("Horizontal");
                float vertCheking = Input.GetAxisRaw("Vertical");
                m_canMoveLeft = hozChecking < 0 ? true : false;
                m_canMoveRight = hozChecking > 0 ? true : false;
                m_canAttack = Input.GetMouseButtonDown(0);
                m_canDash = Input.GetMouseButtonDown(1);
                m_canUlti = vertCheking < 0 ? true : false;
            }
            else
            {
                if (joystick == null) return;
                m_canMoveLeft = joystick.xValue < 0 ? true : false;
                m_canMoveRight = joystick.xValue > 0 ? true : false;
            }
        }
    }
}
