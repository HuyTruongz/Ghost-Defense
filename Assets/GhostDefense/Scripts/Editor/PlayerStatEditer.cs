using System.Collections;
using System.Collections.Generic;
using UHUY.GhostDefense.Editor;
using UnityEngine;
using UnityEditor;
using System;


namespace UHUY.GhostDefense
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PlayerStat), editorForChildClasses: true)]
    public class PlayerStatEditer : ActorStatEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Level Up"))
            {
                m_target.LevelUpCore();
            }
        }
    }
}
