using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Unity.VisualScripting;
using System;

namespace UDEV.GhostDefense
{
    public class Player : Actor
    {
        [Header("Collider")]
        public Collider2D headCol;
        public Collider2D bodyCol;
        public Collider2D deadCol;

        private PlayerStat m_curStat;
        private StateMachine<PlayerSate> m_fsm;
        private PlayerSate m_preState;
        private int m_hozDir, m_vertDir;
        private bool m_isDashed;
        private bool m_isAttacked;
        private float m_curDashRate;
        private float m_curAttackRate;
        private float m_curEnergy;

        public PlayerStat CurStat { get => m_curStat; set => m_curStat = value; }
        public StateMachine<PlayerSate> Fsm { get => m_fsm; }

        public bool IsDead
        {
            get => m_fsm.State == PlayerSate.Dead || m_preState == PlayerSate.Dead;
        }

        public bool IsAttacking
        {
            get => m_fsm.State == PlayerSate.Attack || m_fsm.State == PlayerSate.Ultimate;
        }

        public bool IsUlti
        {
            get => m_fsm.State == PlayerSate.Ultimate;
        }

        public bool IsDashing
        {
            get => m_fsm.State == PlayerSate.Dash;
        }
        public float CurEnergy { get => m_curEnergy; set => m_curEnergy = value; }

        protected override void Awake()
        {
            base.Awake();
            m_fsm = StateMachine<PlayerSate>.Initialize(this);
            if (stat)
            {
                m_curStat = (PlayerStat)stat;
            }
        }

        private void Start()
        {
            FSM_MethodGen.Gen<PlayerSate>(this);
        }

        public override void Init()
        {
            LoadStat();

            m_fsm.ChangeState(PlayerSate.Idel);
            ChangState(PlayerSate.Idel);
        }

        private void LoadStat()
        {
            if (m_curStat)
            {
                m_curStat.Load(GameData.Ins.curPlayerId);
            }

            m_curSpeed = m_curStat.moveSpeed;
            m_curHp = m_curStat.hp;
            m_curDmg = m_curStat.damage;
            m_curDashRate = m_curStat.dashRate;
            m_curAttackRate = m_curStat.atkRate;
            m_curEnergy = m_curStat.ultiEnegry;
        }

        private void ActionHandle()
        {

        }

        public void ChangState(PlayerSate state)
        {
            m_preState = m_fsm.State;
            m_fsm.ChangeState(state);
        }

        private IEnumerator ChangeStateDelayCo(PlayerSate newState, float timeExtra = 0)
        {
            var animClip = Helper.GetClip(m_amin, m_fsm.State.ToString());
            if (animClip)
            {
                yield return new WaitForSeconds(animClip.length + timeExtra);
                if (!IsDead)
                {
                    ChangState(newState);
                }
            }
            yield return null;
        }

        private void ChangeStatDalay(PlayerSate newState, float timeExtra = 0)
        {
            StartCoroutine(ChangeStateDelayCo(newState, timeExtra));
        }

        private void ActiveCol(PlayerCollider collider)
        {
            if (headCol)
            {
                headCol.enabled = collider == PlayerCollider.Normal;
            }

            if (bodyCol)
            {
                bodyCol.enabled = collider == PlayerCollider.Normal;
            }

            if (deadCol)
            {
                deadCol.enabled = collider == PlayerCollider.Dead;
            }
        }

        protected override void Dead()
        {
            base.Dead();
            ChangState(PlayerSate.Dead);
        }

        public override void TakeDamage(float dmg, Actor whoHit)
        {
            if (IsDead || IsUlti) return;

            base.TakeDamage(dmg - m_curStat.defense, whoHit);
            if(m_curHp > 0 && !m_isInvincible)
            {
                ChangState(PlayerSate.GotHit);
            }
        }

        public void AddXp(float xp)
        {
            m_curStat.xp += xp;

            StartCoroutine(m_curStat.LevelUpCo(
                () =>
                {
                    m_curHp = m_curStat.hp;
                }));
        }

        public void AddEnergy(float energyBouns)
        {
            m_curEnergy += energyBouns;
        }

        #region
        private void Idel_Enter() { }
        private void Idel_Update() { }
        private void Idel_FixedUpdate() { }
        private void Idel_Exit() { }
        private void Walk_Enter() { }
        private void Walk_Update() { }
        private void Walk_FixedUpdate() { }
        private void Walk_Exit() { }
        private void Run_Enter() { }
        private void Run_Update() { }
        private void Run_FixedUpdate() { }
        private void Run_Exit() { }
        private void Dead_Enter() { }
        private void Dead_Update() { }
        private void Dead_FixedUpdate() { }
        private void Dead_Exit() { }
        private void Attack_Enter() { }
        private void Attack_Update() { }
        private void Attack_FixedUpdate() { }
        private void Attack_Exit() { }
        private void Ultimate_Enter() { }
        private void Ultimate_Update() { }
        private void Ultimate_FixedUpdate() { }
        private void Ultimate_Exit() { }
        private void GotHit_Enter() { }
        private void GotHit_Update() { }
        private void GotHit_FixedUpdate() { }
        private void GotHit_Exit() { }
        private void Dash_Enter() { }
        private void Dash_Update() { }
        private void Dash_FixedUpdate() { }
        private void Dash_Exit() { }

        #endregion
    }

}
