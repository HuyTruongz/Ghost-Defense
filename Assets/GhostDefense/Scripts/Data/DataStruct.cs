using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTag
{
    Player,
    Enemy
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
