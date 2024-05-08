using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    [CreateAssetMenu(menuName = " Game Setting", fileName = "GameSetting")]
    public class GamePlaySetting : ScriptableObject
    {
        public bool isOnMobile;
    }
}
