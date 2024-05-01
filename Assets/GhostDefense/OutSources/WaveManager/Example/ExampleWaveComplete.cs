using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used for example purposes only - Not essential to the wave manager
public class ExampleWaveComplete : MonoBehaviour
{
    public Text wave;
    public GameObject uiElement;

    public void endWave ()
    {
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        wave.text = "Completed Wave " + WavePlayer.instance.GetCurrentWave();
        uiElement.SetActive(true);
        yield return new WaitForSeconds(2);
        uiElement.SetActive(false);
    }
}
