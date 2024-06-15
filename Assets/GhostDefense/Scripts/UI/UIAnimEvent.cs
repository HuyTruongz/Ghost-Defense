using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.GhostDefense
{
    public class UIAnimEvent : MonoBehaviour
    {
      
        public void Deactive()
        {
            gameObject.SetActive(false);
        }

        public void ShowCompletedDialog()
        {
            if (GUIManager.Ins.completedDialog)
            {
                GUIManager.Ins.completedDialog.Show(true);
            }
        }

        public void ShowGameoverDialog()
        {
            if (GUIManager.Ins.gameoverDialog)
            {
                GUIManager.Ins.gameoverDialog.Show(true);
            }
        }
    }

}

