using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.GhostDefense
{
    public class GameOverDialog : Dialog
    {
        public Text gameplayTimeTxt;
        public Text bestTimeTxt;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
            if (gameplayTimeTxt)
            {
                gameplayTimeTxt.text = Helper.TimeConvert(GameManager.Ins.GplayTimeCounting);
            }

            if (bestTimeTxt)
            {
                float bestTime = GameData.Ins.GetLevelScore(GameData.Ins.curLevelId);
                bestTimeTxt.text = Helper.TimeConvert(bestTime);
            }
        }

        public void Replay()
        {
            Close();
            GameManager.Ins.Replay();
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1f;
        }
    }
}
