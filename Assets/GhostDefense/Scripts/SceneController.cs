using System.Collections;
using System.Collections.Generic;
using UDEV;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UHUY.GhostDefense
{
    public class SceneController : Singleton<SceneController>
    {
        public void LoadGameplay()
        {
            SceneManager.LoadScene(GameScene.Gameplay.ToString());
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
