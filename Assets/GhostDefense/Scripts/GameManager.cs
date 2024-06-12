using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;
using MonsterLove.StateMachine;

namespace UDEV.GhostDefense
{
    public class GameManager : Singleton<GameManager>
    {
        public GamePlaySetting setting;
        [SerializeField]
        private Player m_player;
        private FreeParallax m_map;
        private WavePlayer m_waveCtr;
        private LevelItem m_curLevel;

        private int m_killed;
        private float m_gplayTimeCounting;
        private int m_missionCoinBonus;
        private int m_missionXpBonus;
        private int m_stars;
        private StateMachine<GameState> m_fsm;

        public Player Player { get => m_player; set => m_player = value; }
        public WavePlayer WaveCtr { get => m_waveCtr;}
        public int Killed { get => m_killed; set => m_killed = value; }
        public float GplayTimeCounting { get => m_gplayTimeCounting; }
        public int MissionCoinBonus { get => m_missionCoinBonus;}
        public int Stars { get => m_stars;}
        public StateMachine<GameState> Fsm { get => m_fsm;}

        public override void Awake()
        {
            MakeSingleton(false);
            m_fsm = StateMachine<GameState>.Initialize(this);
            m_fsm.ChangeState(GameState.Playing);
        }

        private void Start()
        {
            Init();
            StartCoroutine(CamFollowDelay());
        }

        public void Init()
        {
            m_curLevel = LevelManager.Ins.levels[GameData.Ins.curLevelId];

            if (m_curLevel == null) return;

            m_missionCoinBonus = Random.Range(m_curLevel.minBonus,m_curLevel.maxBonus);
            m_missionXpBonus = Random.Range(m_curLevel.minXpBonus,m_curLevel.maxXpBonus);
            if (m_curLevel.mapFb)
            {
                Instantiate(m_curLevel.mapFb,Vector3.zero,Quaternion.identity);
            }

            ChangePlayer();

            if (!m_curLevel.waveCtrFb) return;

            m_waveCtr = Instantiate(m_curLevel.waveCtrFb,Vector3.zero,Quaternion.identity);
            m_waveCtr.waveBegins.AddListener(() =>
            {
                //update wave countingTxt
                //update wave bar
                //hide big wave countingtxt
            });

            m_waveCtr.finalWaveComplete.AddListener(() =>
            {
                MissionCompleted();
            });
            m_waveCtr.StartWave();

            Pref.SpriteOrder = 0;

            //update coin on gui
            //Show mobile gamepad gui depend on setting
            //play background music
        }

        public void ChangePlayer()
        {
            Vector3 spawnPos = m_player ? m_player.transform.position : Vector3.zero;
            if (m_player)
            {
                Destroy(m_player.gameObject);
            }
            ShopItem shopItem = ShopManager.Ins.items[GameData.Ins.curPlayerId];
            if (shopItem == null) return;
            m_player = Instantiate(shopItem.heroPb, spawnPos, Quaternion.identity);
            m_player.Init();
            //Update avatar cho hero tren GUI
        }

        public void AddCoin(int cointoAdd)
        {
            if (!m_player) return;

            GameData.Ins.coin += cointoAdd;
            GameData.Ins.SaveData();
            //Update coin GUI
        }

        public void AddHp(int hpToAdd)
        {
            if (!m_player) return;

            m_player.CurHp += hpToAdd;
            //Upadate hp on Gui
        }

        public void Gameover()
        {
            m_fsm.ChangeState(GameState.Gameover);
            //Hide YouDieTxt on GUI
        }

        public void MissionCompleted()
        {
            m_fsm.ChangeState(GameState.Wining);
            //hide misssionconplettxt on gui
        }

        public void Replay()
        {
            m_waveCtr.StopAllCoroutines();
            SceneController.Ins.LoadGameplay();
        }

        public void SetMapSpeed(float speed)
        {
            if (!WaveCtr) return;

            if (!m_map) return;
            m_map.Speed = speed;
        }

        private IEnumerator CamFollowDelay()
        {
            yield return new WaitForSeconds(0.3f);
            CameraFollow.ins.target = m_player.transform;
        }

        #region
        private void Starting_Enter() { }
        private void Starting_Update() { }
        private void Starting_Exit() { }
        private void Playing_Enter() { }
        private void Playing_Update()
        {
            m_gplayTimeCounting += Time.deltaTime;
        }
        private void Playing_Exit() { }
        private void Wining_Enter()
        {
            m_player.AddXp(m_missionXpBonus);
            AddCoin(m_missionCoinBonus);
            int timeScore = Mathf.RoundToInt(m_gplayTimeCounting);
            m_stars = m_curLevel.goal.GetStar(Mathf.RoundToInt(m_gplayTimeCounting));
            GameData.Ins.UpdateLevelStars(GameData.Ins.curLevelId,m_stars);
            GameData.Ins.UpdateLevelScore(GameData.Ins.curLevelId,timeScore);
            GameData.Ins.curLevelId++;
            GameData.Ins.UpdateLevelUnlocked(GameData.Ins.curLevelId,true);
            GameData.Ins.SaveData();
            //Play winning sound effect
        }
        private void Wining_Update() { }
        private void Wining_Exit() { }
        private void Gameover_Enter()
        {
            //play lose sound effect
        }
        private void Gameover_Update() { }
        private void Gameover_Exit() { }

        #endregion
    }
}
