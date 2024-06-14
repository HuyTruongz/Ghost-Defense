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
        private PlayerSate m_prevState;
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
            get => m_fsm.State == PlayerSate.Dead || m_prevState == PlayerSate.Dead;
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
            Init();
        }

        private void Update()
        {
            LimitHozMoving();
            if(m_isInvincible || m_isKnockBack)
            {
                float mapSpeed = m_isFacingLeft ? m_curStat.knockbackForce : -m_curStat.knockbackForce;
                GameManager.Ins.SetMapSpeed(mapSpeed);
            }
            ActionHandle();
        }
        public override void Init()
        {
            LoadStat();

            m_fsm.ChangeState(PlayerSate.Idle);
            m_prevState = PlayerSate.Idle;
        }

        public void LoadStat()
        {
            if (!m_curStat) return;

            m_curStat.Load(GameData.Ins.curPlayerId);
            m_curSpeed = m_curStat.moveSpeed;
            m_curHp = m_curStat.hp;
            m_curDmg = m_curStat.damage;
            m_curDashRate = m_curStat.dashRate;
            m_curAttackRate = m_curStat.atkRate;
            m_curEnergy = m_curStat.ultiEnegry;

      
        }

        private void ActionHandle()
        {
            if (IsAttacking || IsDashing || m_isKnockBack || IsDead) return;

            if (GamepaConreoller.Ins.IsStatic)
            {
                m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
                m_curSpeed = 0f;
                if (!m_isInvincible)
                {
                    GameManager.Ins.SetMapSpeed(0);
                    ChangState(PlayerSate.Idle);
                }
            }
            else
            {
                if (GamepaConreoller.Ins.CanAttack)
                {
                    if (!m_isAttacked)
                    {
                        m_isAttacked = true;
                        ChangState(PlayerSate.Attack);
                    }
                }
                else if(GamepaConreoller.Ins.CanUlti && m_curEnergy >= m_curStat.ultiEnegry)
                {
                    ChangState(PlayerSate.Ultimate);
                }
            }

            ReduceActionRate(ref m_isDashed,ref m_curDashRate, m_curStat.dashRate);
            ReduceActionRate(ref m_isAttacked,ref m_curAttackRate, m_curStat.atkRate);

            GUIManager.Ins.atkBtnFilled.UpdateValue(m_curAttackRate,m_curStat.atkRate);
            GUIManager.Ins.dashBtnFilled.UpdateValue(m_curDashRate,m_curStat.dashRate);
            GUIManager.Ins.ultiBtnFilled.UpdateValue(m_curEnergy,m_curStat.ultiEnegry);
        }

        private void Move(Direction dir)
        {
            if (m_isKnockBack) return;

            if (dir == Direction.Left || dir == Direction.Right)
            {
                Flip(dir);

                m_hozDir = dir == Direction.Left ? -1 : 1;

                if (GameManager.Ins.setting.isOnMobile)
                {
                    m_rb.velocity = new Vector2(GamepaConreoller.Ins.joystick.xValue * m_curSpeed,
                        m_rb.velocity.y);
                }
                else
                {
                    m_rb.velocity = new Vector2(m_hozDir * m_curSpeed, m_rb.velocity.y);
                }

                if (CameraFollow.ins.IsHozStuck)
                {
                    GameManager.Ins.SetMapSpeed(0);
                }
                else
                {
                    GameManager.Ins.SetMapSpeed(-m_hozDir * CurSpeed);
                }
            }
        }

        public override void Dash()
        {
            if (IsFacingLeft)
            {
                transform.position = new Vector3(transform.position.x - m_curStat.dashDist,
                    transform.position.y,
                    transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + m_curStat.dashDist,
                    transform.position.y,
                    transform.position.z);
            }
        }

        public void ChangState(PlayerSate state)
        {
            m_prevState = m_fsm.State;
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
            if (m_curHp > 0 && !m_isInvincible)
            {
                ChangState(PlayerSate.GotHit);
            }

            GUIManager.Ins.hpBar.UpdateValue(m_curHp, m_curStat.hp);
        }

        public void AddXp(float xp)
        {
            m_curStat.xp += xp;

            StartCoroutine(m_curStat.LevelUpCo(
                () =>
                {
                    m_curHp = m_curStat.hp;

                    GUIManager.Ins.dmgTxtMng.Add("Level Up",transform,"level_up");
                    GUIManager.Ins.UpdateHeroLevel(m_curStat.playerLevel);
                    GUIManager.Ins.UpdateHeroPoint(m_curStat.point);
                    GUIManager.Ins.hpBar.UpdateValue(m_curHp,m_curStat.hp);

                    //play sound effect
                }));
        }

        public void AddEnergy(float energyBouns)
        {
            m_curEnergy += energyBouns;
            GUIManager.Ins.ultiBtnFilled.UpdateValue(m_curEnergy,m_curStat.ultiEnegry);
            GUIManager.Ins.energyBar.UpdateValue(m_curEnergy, m_curStat.ultiEnegry);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameTag.Collectable.ToString()))
            {
                Collectable collectable = col.gameObject.GetComponent<Collectable>();
                if(!collectable) return;
                collectable.Trigger();
            }
        }

        #region
        private void Idle_Enter()
        {
            ActiveCol(PlayerCollider.Normal);
        }
        private void Idle_Update()
        {
            if (GamepaConreoller.Ins.CanMoveLeft || GamepaConreoller.Ins.CanMoveRight)
            {
                ChangState(PlayerSate.Walk);
            }

            if (IsDead)
            {
                ChangState(PlayerSate.Dead);
            }

            Helper.PlayAnim(m_amin, PlayerSate.Idle.ToString());
        }
        private void Idle_FixedUpdate() { }
        private void Idle_Exit() { }
        private void Walk_Enter()
        {
            m_curSpeed = m_curStat.moveSpeed;
        }
        private void Walk_Update()
        {
            if (GamepaConreoller.Ins.CanDash)
            {
                if (!m_isDashed)
                {
                    m_isDashed = true;
                    ChangState(PlayerSate.Dash);
                }
            }
            else
            {
                m_curSpeed += Time.deltaTime * 1.5f;
                m_curSpeed = Mathf.Clamp(m_curSpeed, m_curStat.moveSpeed, m_curStat.runSeed);
                if(m_curSpeed >= m_curStat.runSeed)
                {
                    Helper.PlayAnim(m_amin, PlayerSate.Run.ToString());
                }
                else
                {
                    Helper.PlayAnim(m_amin, PlayerSate.Walk.ToString());
                }
            }

            if (GamepaConreoller.Ins.CanMoveLeft)
            {
                Move(Direction.Left);
            }
            else if (GamepaConreoller.Ins.CanMoveRight)
            {
                Move(Direction.Right);
            }

            
        }
        private void Walk_FixedUpdate() { }
        private void Walk_Exit() { }
        private void Run_Enter() { }
        private void Run_Update()
        {

            Helper.PlayAnim(m_amin, PlayerSate.Run.ToString());
        }
        private void Run_FixedUpdate() { }
        private void Run_Exit() { }
        private void Dead_Enter()
        {
            ActiveCol(PlayerCollider.Dead);
            CamShake.ins.ShakeTrigger(0.2f,0.2f);
        }
        private void Dead_Update()
        {
            gameObject.layer = deadLayer;
            GameManager.Ins.SetMapSpeed(0);
            Helper.PlayAnim(m_amin, PlayerSate.Dead.ToString());
        }
        private void Dead_FixedUpdate() { }
        private void Dead_Exit() { }
        private void Attack_Enter()
        {
            ChangeStatDalay(PlayerSate.Idle);
        }
        private void Attack_Update()
        {
            Helper.PlayAnim(m_amin, PlayerSate.Attack.ToString());
        }
        private void Attack_FixedUpdate() { }
        private void Attack_Exit() { }
        private void Ultimate_Enter()
        {
            m_curEnergy -= m_curStat.ultiEnegry;
            m_curDmg = m_curStat.damage + m_curStat.damage * 0.3f;
            ChangeStatDalay(PlayerSate.Idle);

            GUIManager.Ins.energyBar.UpdateValue(m_curEnergy, m_curStat.ultiEnegry);
        }
        private void Ultimate_Update()
        {
            Helper.PlayAnim(m_amin, PlayerSate.Ultimate.ToString());
        }
        private void Ultimate_FixedUpdate() { }
        private void Ultimate_Exit()
        {
            m_curDmg = m_curStat.damage;
        }
        private void GotHit_Enter()
        {
            AIStat aiStat = (AIStat)m_whoHit.stat;
            AddEnergy(aiStat.EnergyBouns / 5);
        }
        private void GotHit_Update()
        {
            KnockBackMove(0.2f);
            if (!m_isKnockBack)
            {
                ChangState(PlayerSate.Idle);
            }
            Helper.PlayAnim(m_amin, PlayerSate.GotHit.ToString());
        }
        private void GotHit_FixedUpdate() { }
        private void GotHit_Exit()
        {
            
        }
        private void Dash_Enter()
        {
            gameObject.layer = invincibleLayer;
            ChangeStatDalay(PlayerSate.Idle);
        }
        private void Dash_Update()
        {
            Helper.PlayAnim(m_amin, PlayerSate.Dash.ToString());
        }
        private void Dash_FixedUpdate() { }
        private void Dash_Exit()
        {
            gameObject.layer = normalLayer;
        }

        #endregion
    }

}
