using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.GhostDefense
{
    public class ActorStat : Stat
    {
#if UNITY_EDITOR
        [HideInInspector]
        public string id;
        [HideInInspector]
        public Sprite thumb;
#endif
        [Header("Base Stats:")]
        public float hp;
        public float damage;
        public float moveSpeed;
        public float knockbackForce;
        public float knockbackTime;
        public float invincibleTime;

        public override bool IsMaxLevel()
        {
            return false;
        }

        public override void Load()
        {
            
        }

        public override void Load(int id)
        {
           
        }

        public override void Save()
        {
            
        }

        public override void Save(int id)
        {
           
        }

        public override string ToJon()
        {
           return JsonUtility.ToJson(this);
        }

        public override void Upgrade(UnityAction Success = null, UnityAction Failed = null)
        {
            
        }

        public virtual void LevelUpCore()
        {

        }

        public virtual void UpgradeCore()
        {

        }

        public virtual void UpgradeToMax()
        {

        }
    }
}
