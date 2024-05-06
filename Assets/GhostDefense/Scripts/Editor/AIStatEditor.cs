using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UHUY.GhostDefense.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AIStat), editorForChildClasses: true)]
    public class AIStatEditor : ActorStatEditor
    {
        public override void Upgrade()
        {
            Load(m_fileName);
           m_target.Upgrade();
        }
    }
}
