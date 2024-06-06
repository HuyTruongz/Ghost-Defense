using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;

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
