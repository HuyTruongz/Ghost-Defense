using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

namespace UHUY.GhostDefense.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActorStat),editorForChildClasses: true)]

    public class ActorStatEditor : UnityEditor.Editor
    {
        protected ActorStat m_target;
        protected string m_fileName;
        private string m_path = string.Empty;
        private string m_filePath = string.Empty;

        public override void OnInspectorGUI()
        {
            m_target = (ActorStat)target;
            m_path = Application.dataPath + "/Editor/Resources/StatData";
            m_target.thumb = (Sprite)EditorGUILayout.ObjectField(
                m_target.thumb, typeof(Sprite),false, GUILayout.Width(80), GUILayout.Height(80));
            base.OnInspectorGUI();

            m_fileName = $"actor_data_{m_target.id}";
            m_filePath = $"{m_path}/{m_fileName}.txt";

            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button("Load"))
            {
                Load(m_fileName);
            }

            if (GUILayout.Button("Upgrade"))
            {
                Upgrade();
            }

            if (GUILayout.Button("Upgrade To Max"))
            {
                UpgradeToMax();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(m_target);
            }
        }

        public virtual void UpgradeToMax()
        {
            m_target.UpgradeToMax();
        }

        public virtual void Upgrade()
        {
            m_target.UpgradeCore();
        }

        public virtual void Load(string fileName)
        {
           string data = Resources.Load<TextAsset>($"StatData/{fileName}") .ToString();
            if (!string.IsNullOrEmpty(data))
            {
                JsonUtility.FromJsonOverwrite(data,m_target);
            }
        }

        private void CreateFilepath()
        {
            m_target.id = Helper.GenerateUID();
            m_fileName = $"actor_data_{m_target.id}";
            m_filePath = $"{m_path}/{m_fileName}.txt";
        }

        public virtual void Save()
        {
            if(IsDupplicateId(m_target.id) || string.IsNullOrEmpty(m_target.id))
            {
                CreateFilepath();
            }
            File.WriteAllText(m_filePath, m_target.ToJon());
            AssetDatabase.Refresh();
        }

        private bool IsDupplicateId(string id)
        {
            var data = Resources.LoadAll<ActorStat>("Data");
            var finder = data.Where(d => string.Compare(d.id, id) == 0);
            if (finder == null) return false;

            var rs = finder.ToArray();
            if(rs == null|| rs.Length == 0 ) return false;

            return rs.Length > 1;
        }
    }
}
