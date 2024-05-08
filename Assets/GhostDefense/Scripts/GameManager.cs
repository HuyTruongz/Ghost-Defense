using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class GameManager : Singleton<GameManager>
    {
        public GamePlaySetting setting;
        public override void Awake()
        {
            MakeSingleton(false);
        }
    } 
}
