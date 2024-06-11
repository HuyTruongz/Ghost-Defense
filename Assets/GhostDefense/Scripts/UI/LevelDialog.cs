using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.GhostDefense
{
    public class LevelDialog : Dialog
    {
        public Transform gridRoot;
        public LevelItemUI itemUIPb;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
            Time.timeScale = 0f;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var levels = LevelManager.Ins.levels;

            if (levels == null || levels.Length <= 0 || !gridRoot || !itemUIPb) return;

            Helper.ClearChilds(gridRoot);

            for (int i = 0; i < levels.Length; i++)
            {
                int levelId = i;
                var level = levels[i];
                if (level == null) continue;

                var itemUIClone = Instantiate(itemUIPb, Vector3.zero, Quaternion.identity);
                itemUIClone.transform.SetParent(gridRoot);
                itemUIClone.transform.localScale = Vector3.one;
                itemUIClone.transform.position = Vector3.zero;
                itemUIClone.UpdateUI(level, levelId);

                if (itemUIClone.btnComp)
                {
                    itemUIClone.btnComp.onClick.RemoveAllListeners();
                    itemUIClone.btnComp.onClick.AddListener(() => ItemEvent(level, levelId));
                }

            }
        }

        private void ItemEvent(LevelItem level, int levelId)
        {
            if (level == null) return;

            bool isUnlocked = GameData.Ins.IsLevelUnlocked(levelId);

            if (isUnlocked)
            {
                GameData.Ins.curLevelId = levelId;
                LevelManager.Ins.CurLevelId = levelId;
                GameData.Ins.SaveData();
                Close();
                SceneController.Ins.LoadGameplay();
            }
            else
            {
                Debug.Log("Level not Unlocked");
            }
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1.0f;
        }
    }
}
