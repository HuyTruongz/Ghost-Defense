using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guirao.UltimateTextDamage;
using UnityEngine.UI;

namespace UDEV.GhostDefense
{
    public class GUIManager : Singleton<GUIManager>
    {
        [Header("Txt Damage:")]
        public UltimateTextDamageManager dmgTxtMng;
        [Header("Mobile Gamepad:")]
        public GameObject mobileGamepad;
        public ImageFilled atkBtnFilled;
        public ImageFilled dashBtnFilled;
        public ImageFilled ultiBtnFilled;

        [Header("Hero Info:")]
        public ImageFilled hpBar;
        public ImageFilled energyBar;
        public Image heroAvatar;
        public Text lvCountingTxt;
        public Text ptContingTxt;

        [Header("Wave Info:")]
        public ImageFilled waveBar;
        public ImageFilled bossHpBar;
        public Text waveBarTxt;

        [Header("Other:")]
        public Text coinCountingTxt;
        public Text waveCountingTxt;
        public Text missionCompletedTxt;
        public Text youDieTxt;
        public RectTransform coinMovingDest;

        [Header("Dialog")]
        public Dialog completedDialog;
        public Dialog gameoverDialog;


        public override void Awake()
        {
            MakeSingleton(false);
        }

        public void ShowMobileGamepad(bool isShow)
        {
            if (mobileGamepad)
            {
                mobileGamepad.SetActive(isShow);
            }
        }

        private void UpdateTxt(Text txt, string content)
        {
            if (txt)
            {
                txt.text = content;
            }
        }

        public void UpdateHeroLevel(int level)
        {
            UpdateTxt(lvCountingTxt, $"Levle {level}");
        }

        public void UpdateHeroPoint(int point)
        {
            UpdateTxt(ptContingTxt, $"Point {point}");
        }

        public void UpdateHeroAvatar(Sprite av)
        {
            if (heroAvatar)
            {
                heroAvatar.sprite = av;
            }
        }

        public void UpdateCoinCounting()
        {
            UpdateTxt(coinCountingTxt, GameData.Ins.coin.ToString());
        }

        public void UpdateWaveCounting(int cur, int total)
        {
            string content = string.Empty;
            if (cur == total)
            {
                content = "FINAL WAVE";
            }
            else
            {
                content = $"WAVE {cur} / {total}";
            }

            UpdateTxt(waveBarTxt, content);
            UpdateTxt(waveCountingTxt,content);
        }

        public Vector3 GetCoinIconUICorner()
        {
            if (!coinMovingDest) return Vector3.zero;

            var rect = coinMovingDest;
            Vector3[] v = new Vector3[4];
            rect.GetWorldCorners(v);
            return v[0];
        }

    }

}