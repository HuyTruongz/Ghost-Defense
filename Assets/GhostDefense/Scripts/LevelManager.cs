using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelItem[] levels;
        private int m_curLevelId;

        public int CurLevelId { get => m_curLevelId; set => m_curLevelId = value; }

        public void Init()
        {
            if (levels == null || levels.Length <= 0) return;

            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];

                if(level == null ) continue;

                if(i == 0)
                {
                    GameData.Ins.UpdateLevelUnlocked(i, true);
                    GameData.Ins.curLevelId = i;
                }
                else
                {
                    GameData.Ins.UpdateLevelUnlocked(i, false);
                }
                GameData.Ins.UpdateLevelPassed(i, false);
                GameData.Ins.UpdateLevelStars(i, 0);
                GameData.Ins.UpdateLevelScoreNoneCheck(i,0);
            }
            GameData.Ins.SaveData();
        }
    }

}