using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class MainMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (!Pref.IsFirstTime)
            {
                GameData.Ins.LoadData();
            }
            else
            {
                ShopManager.Ins.Init();
                LevelManager.Ins.Init();
                GameData.Ins.SaveData();
            }

            AudioController.Ins.PlayMusic(AudioController.Ins.menus);

            Pref.IsFirstTime = false;
        }

    }
}
