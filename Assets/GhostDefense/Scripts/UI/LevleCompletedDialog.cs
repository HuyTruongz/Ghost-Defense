using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.GhostDefense
{
    public class LevleCompletedDialog : Dialog
    {
        public Image[] stars;
        public Text gameplayTimeTxt;
        public Text bonusTxt;
        public Sprite activeStar;
        public Sprite deactiveStar;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if (stars == null || stars.Length <= 0) return;

            for (int i = 0; i < stars.Length; i++)
            {
                var star = stars[i];
                if (!star) continue;
                star.sprite = deactiveStar;
            }

            for (int i = 0; i < GameManager.Ins.Stars; i++)
            {
                var star = stars[i];
                if (!star) continue;
                star.sprite = activeStar;
            }

            if (gameplayTimeTxt)
            {
                gameplayTimeTxt.text = Helper.TimeConvert(GameManager.Ins.GplayTimeCounting);
            }

            if (bonusTxt)
            {
                bonusTxt.text =GameManager.Ins.MissionCoinBonus.ToString();
            }
        }

        public void Replay()
        {
            Close();
            GameManager.Ins.Replay();
        }

        public void NextLevle()
        {
            LevelItem[] levels = LevelManager.Ins.levels;

            if (levels == null || levels.Length <= 0) return;

            if(GameData.Ins.curLevelId >= LevelManager.Ins.levels.Length - 1)
            {
                SceneController.Ins.LoadScene(GameScene.MaiMenu.ToString());
            }
            else
            {
                SceneController.Ins.LoadGameplay();
            }
        }
    }
}