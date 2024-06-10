using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;
using UDEV.GhostDefense;

namespace UDEV.GhostDefense
{
    public enum GameTag
    {
        Player,
        Enemy,
        Collectable
    }

    public enum GameScene
    {
        Gameplay,
        MaiMenu
    }

    public enum KeyPref
    {
        GameData,
        IsFirstTime,
        SpriteOrder
    }

    public enum Direction
    {
        Left, Right, Top, Bottom, None
    }

    public enum GameState
    {
        Starting,
        Playing,
        Wining,
        Gameover
    }

    public enum PlayerSate
    {
        Idle,
        Walk,
        Run,
        Dead,
        Attack,
        Ultimate,
        GotHit,
        Dash
    }

    public enum AIState
    {
        Walk,
        Dash1,
        Dash2,
        Attack,
        GotHit,
        Ultimate,
        Dead
    }

    public enum PlayerCollider
    {
        Normal,
        Dead
    }

    [System.Serializable]
    public class CollectableItem
    {
        [Range(0f, 1f)] public float spawnRate;
        public int amount;
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string collectablePool;
    }

    [System.Serializable]
    public class ShopItem
    {
        public int price;
        public string heroName;
        public Sprite preview;
        public Sprite avata;
        public Player heroPb;
    }

    [System.Serializable]
    public class LevelItem
    {
        public int minBonus;
        public int maxBonus;
        public int minXpBonus;
        public int maxXpBonus;
        public Goal goal;
        public WavePlayer waveCtrFb;
        public FreeParallax mapFb;
    }

    [System.Serializable]
    public class Goal
    {
        public int timeOneStar;
        public int timeTwoStar;
        public int timeThreeStar;

        public int GetStar(int time)
        {
            if (time < timeOneStar)
            {
                return 3;
            }
            else if (time < timeTwoStar)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }

}