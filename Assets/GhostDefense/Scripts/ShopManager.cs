using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class ShopManager : Singleton<ShopManager>
    {
        public ShopItem[] items;


        public void Init()
        {
            if (items == null || items.Length <= 0) return;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (item == null) continue;

                if(i == 0)
                {
                    GameData.Ins.UpdatePlayerUnlocked(i, true);
                    GameData.Ins.curPlayerId = i;
                }
                else
                {
                    GameData.Ins.UpdatePlayerUnlocked(i, false);
                }

                GameData.Ins.UpdatePlayerStat(i, item.heroPb.stat.ToJon());
            }
            GameData.Ins.SaveData();
        }
    }
}
