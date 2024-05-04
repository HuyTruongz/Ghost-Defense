using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;
using UnityEngine.UIElements;

namespace UHUY.GhostDefense
{
    public class GameData : Singleton<GameData>
    {
        public int coin;
        public int curLevelId;
        public int curPlayerId;
        public float musicVol;
        public float soundVol;
        public List<bool> levelUnlockeds;
        public List<bool> levelPasseds;
        public List<bool> playerUnlockeds;
        public List<int> levelStars;
        public List<string> playerStats;
        public List<float> completedTimes;

        public override void Awake()
        {
            base.Awake();
            levelUnlockeds = new List<bool>();
            levelPasseds = new List<bool>();
            playerUnlockeds = new List<bool>();
            levelStars = new List<int>();
            playerStats = new List<string>();
            completedTimes = new List<float>();
        }

        private void HandleData(string data)
        {
            if (string.IsNullOrEmpty(data)) return;

            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void SaveData()
        {
            Pref.GameData = JsonUtility.ToJson(this);
        }

        public void LoadData()
        {
            HandleData(Pref.GameData);
        }

        private T GetValue<T>(List<T> dataList, int idx)
        {
            if (dataList == null || dataList.Count == 0 || dataList[idx] == null) return default;

            return dataList[idx];
        }

        private void UpdateValue<T>(ref List<T> dataList, int idx, T value)
        {
            if (dataList == null) return;

            if (dataList.Count <= 0 || (dataList.Count > 0 && idx > dataList.Count))
            {
                dataList.Add(value);
            }
            else
            {
                dataList[idx] = value;
            }
        }

        public bool GetLevelUnlocked(int id)
        {
            return GetValue<bool>(levelUnlockeds, id);
        }

        public void UpdateLevelUnlocked(int id, bool isUnlocked)
        {
            UpdateValue<bool>(ref levelUnlockeds, id, isUnlocked);
        }

        public bool GetLevelPassed(int id)
        {
            return GetValue<bool>(levelPasseds, id);
        }

        public void UpdateLevelPassed(int id, bool isUnlocked)
        {
            UpdateValue<bool>(ref levelPasseds, id, isUnlocked);
        }

        public bool GetPlayerUnlocked(int id)
        {
            return GetValue<bool>(playerUnlockeds, id);
        }

        public void UpdatePlayerUnlocked(int id, bool isUnlocked)
        {
            UpdateValue<bool>(ref playerUnlockeds, id, isUnlocked);
        }

        public int GetLeveStars(int levelId)
        {
            return GetValue<int>(levelStars, levelId);
        }

        public void UpdateLevelStars(int levelId, int stars)
        {
            UpdateValue<int>(ref levelStars, levelId, stars);
        }

        public string GetPlayerStat(int id)
        {
            return GetValue<string>(playerStats, id);
        }

        public void UpdatePlayerStat(int id,string stat)
        {
            UpdateValue<string>(ref playerStats, id, stat);
        }

        public float GetLevelScore(int levelId)
        {
            return GetValue<float>(completedTimes, levelId);
        }

        public void UpdateLevelScoreNoneCheck(int levelId, float time)
        {
            UpdateValue<float>(ref completedTimes, levelId, time);
        }

        public void UpdateLevelScore(int levelId,float time)
        {
            float oldTime = GetValue<float>(completedTimes, levelId);

            if(time < oldTime || oldTime == 0)
            {
                UpdateLevelScoreNoneCheck(levelId,time);
            } 
        }

        private bool IsItemUnlocked(List<bool> dataList,int idx)
        {
            if(dataList == null|| dataList.Count <= 0) return false;

            return dataList[idx];
        }

        public bool IsLevelUnlocked(int id)
        {
            return IsItemUnlocked(levelUnlockeds,id);
        }

        public bool IsLevelPassed(int id)
        {
            return IsItemUnlocked(levelPasseds, id);
        }

        public bool IsPlayerUnlocked(int id)
        {
            return IsItemUnlocked(playerUnlockeds, id);
        }
    }
}
