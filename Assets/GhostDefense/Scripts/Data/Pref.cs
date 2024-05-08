using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public static class Pref
    {
        public static bool IsFirstTime
        {
            set => SetBool(KeyPref.IsFirstTime.ToString(), value);
            get => GetBool(KeyPref.IsFirstTime.ToString(), true);
        }

        public static int SpriteOrder
        {
            set => PlayerPrefs.SetInt(KeyPref.SpriteOrder.ToString(), value);
            get => PlayerPrefs.GetInt(KeyPref.SpriteOrder.ToString(), 0);
        }

        public static string GameData
        {
            set => PlayerPrefs.SetString(KeyPref.GameData.ToString(), value);
            get => PlayerPrefs.GetString(KeyPref.GameData.ToString(), "");
        }

        public static void SetBool(string key, bool v)
        {
            if (v)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            else
            {
                PlayerPrefs.SetInt(key, 0);
            }
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                int valFromPlayer = PlayerPrefs.GetInt(key);
                if (valFromPlayer == 1)
                {
                    return true;
                }
                else if (valFromPlayer == 0)
                {
                    return false;
                }
            }
            else
            {
                return defaultValue;
            }

            return defaultValue;

        }
    }
}
