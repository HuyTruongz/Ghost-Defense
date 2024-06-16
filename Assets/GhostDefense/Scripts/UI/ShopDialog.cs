using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using System;
namespace UDEV.GhostDefense
{
    public class ShopDialog : Dialog
    {
        public Text totalCoinTxt;
        public Image heroPreview;
        public Text heroNameTxt;
        public Image heroAvata;
        public Image levelFilled;
        public Text lvProgTxt;
        public Text lvCountingTxt;
        public Text pointTxt;
        public Image hpFilled;
        public Image atkFilled;
        public Image defFilled;
        public Image luckFilled;
        public Button unlockBtn;
        public Button upgradeBtn;
        public Text unlockBtnTxt;
        public Text upgradeBtnTxt;
        public Image nextBtnImg;
        public Image prevBtnImg;
        public Sprite navBtnNormal;
        public Sprite navBtnActive;

        private ShopItem[] m_items;
        private int m_curPlayerId;
        private PlayerStat m_curStat;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            m_items = ShopManager.Ins.items;
            m_curPlayerId = GameData.Ins.curPlayerId;

            UpdateUI();
            SwitchNavigatorSprite(true);

            Time.timeScale = 0;
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1f;
        }

        private void UpdateUI()
        {
            if (totalCoinTxt)
            {
                totalCoinTxt.text = GameData.Ins.coin.ToString();
            }

            bool isUnlocked = GameData.Ins.IsPlayerUnlocked(m_curPlayerId);

            if (m_items == null || m_items.Length <= 0) return;

            var item = m_items[m_curPlayerId];

            if (item == null) return;

            if (item.heroPb && item.heroPb.stat)
            {
                m_curStat = (PlayerStat)item.heroPb.stat;
                m_curStat.Load(m_curPlayerId);
            }

            if (heroPreview)
            {
                heroPreview.sprite = item.preview;
            }

            if (heroNameTxt)
            {
                heroNameTxt.text = item.heroName;
            }

            if (heroAvata)
            {
                heroAvata.sprite = item.avata;
            }

            if (levelFilled)
            {
                levelFilled.fillAmount = m_curStat.xp / m_curStat.lvUpXpRequired;
            }

            if (lvProgTxt)
            {
                lvProgTxt.text = (Mathf.RoundToInt(m_curStat.xp / m_curStat.lvUpXpRequired * 100)) + "%";
            }

            if (lvCountingTxt)
            {
                lvCountingTxt.text = $"Level {m_curStat.level}";
            }

            if (pointTxt)
            {
                pointTxt.text = $"{m_curStat.point} Point";
            }

            if (hpFilled)
            {
                hpFilled.fillAmount = m_curStat.hp / m_curStat.MaxHp;
            }

            if (atkFilled)
            {
                atkFilled.fillAmount = m_curStat.damage / m_curStat.MaxDmg;
            }

            if (defFilled)
            {
                defFilled.fillAmount = m_curStat.defense / m_curStat.MaxDef;
            }

            if (luckFilled)
            {
                luckFilled.fillAmount = m_curStat.luck / m_curStat.MaxLuck;
            }

            if (unlockBtn)
            {
                unlockBtn.gameObject.SetActive(!isUnlocked);
                unlockBtn.onClick.RemoveAllListeners();
                unlockBtn.onClick.AddListener(() => unlockHero(item));
            }

            if (unlockBtnTxt)
            {
                unlockBtnTxt.text = item.price.ToString();
            }

            if (upgradeBtn)
            {
                upgradeBtn.gameObject.SetActive(isUnlocked);
            }

            if (upgradeBtnTxt)
            {
                upgradeBtnTxt.text = $"{m_curStat.pointRequired} PT";
            }

            if (GUIManager.Ins && GameManager.Ins)
            {
                GUIManager.Ins.UpdateCoinCounting();
                GUIManager.Ins.UpdateHeroPoint(m_curStat.point);
                GUIManager.Ins.hpBar.UpdateValue(
                    GameManager.Ins.Player.CurHp,
                    GameManager.Ins.Player.CurStat.hp);
                GUIManager.Ins.energyBar.UpdateValue(
                     GameManager.Ins.Player.CurEnergy,
                    GameManager.Ins.Player.CurStat.ultiEnegry
                    );
            }
        }

        private void unlockHero(ShopItem item)
        {
            if (GameData.Ins.coin >= item.price)
            {
                GameData.Ins.coin -= item.price;
                GameData.Ins.UpdatePlayerUnlocked(m_curPlayerId, true);
                GameData.Ins.curPlayerId = m_curPlayerId;
                GameData.Ins.SaveData();
                UpdateUI();

                if (GameManager.Ins)
                {
                    GameManager.Ins.ChangePlayer();
                }
                //phat am thanh
            }
        }

        public void UpgradeHero()
        {
            if (!m_curStat) return;

            m_curStat.Upgrade(
                () =>
                {
                    UpdateUI();
                    if (GameManager.Ins && GameManager.Ins.Player)
                    {
                        GameManager.Ins.Player.LoadStat();
                    }
                    UpdateUI();
                    //phat am thanh
                }
                );
        }

        private void SwitchNavigatorSprite(bool isNext)
        {
            if (nextBtnImg)
            {
                nextBtnImg.sprite = isNext ? navBtnActive : navBtnNormal;
            }

            if (prevBtnImg)
            {
                nextBtnImg.sprite = isNext ? navBtnNormal : navBtnActive;
            }
        }

        private void SelectHero()
        {
            bool isUnlocked = GameData.Ins.IsPlayerUnlocked(m_curPlayerId);

            if (isUnlocked)
            {
                GameData.Ins.curPlayerId = m_curPlayerId;
                GameData.Ins.SaveData();
                if (GameManager.Ins)
                {
                    GameManager.Ins.ChangePlayer();
                }
            }
            UpdateUI();
        }

        public void NextHero()
        {
            m_curPlayerId++;

            if (m_curPlayerId >= m_items.Length)
            {
                m_curPlayerId = 0;
            }

            SelectHero();
            UpdateUI();
            SwitchNavigatorSprite(true);
        }

        public void PrevHero()
        {
            m_curPlayerId--;

            if (m_curPlayerId < 0)
            {
                m_curPlayerId = m_items.Length - 1;
            }

            SelectHero();
            UpdateUI();
            SwitchNavigatorSprite(false);
        }


    }
}
