using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Used for example purposes only - Not essential to the wave manager
public class EnemyExample : MonoBehaviour
{
    public Vector3 direction = new Vector3(0.05f, 0, 0);
    public float destroyTime = 2;

    private void OnEnable()
    {
        Invoke("Deactive", destroyTime);
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction);
    }
}
