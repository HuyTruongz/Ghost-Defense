using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;

//Stores infromation relating to a wave
[CreateAssetMenu(fileName = "New Wave", menuName = "Wave Manager/Wave")]
public class Wave : ScriptableObject
{
    public List<WaveElement> elements = new List<WaveElement>();    //List of all elements in a wave
    [SerializeField] public Texture2D icon;                         //Custom icon used in asset view
    //[HideInInspector]
    public int totalEnemy;
    //[HideInInspector]
    public int enemyKilled;

    private void OnEnable()
    {
        icon = (Texture2D)Resources.Load("Wave");
    }
}

[System.Serializable]
public class WaveElement
{
    [SerializeField] public ElementBehaviour behaviour;     //Type of event e.g. WAIT, SINGLE or BULK.

    [PoolerKeys(target = PoolerTarget.NONE)]
    [SerializeField] public string enemy;               //The enemy that will be spawned if 'randomEnemy' is false.
    [SerializeField] public WeightedEnemySet enemySet;      //The list of enemies which can be spawned if 'randomEnemy' is true.
    [SerializeField] public bool randomEnemy = false;       //True if random spawns will be used.

    [SerializeField] public int bulkElements;               //The number of enemies to be spawned in a BULK element.

    [SerializeField] public bool useTimeRange;              //True if wait times will be randomised.
    [SerializeField] public float time;                     //The wait time if 'useTimeRange' is false.
    [SerializeField] public Vector2 timeRange;              //The max and min time wait time if 'useTimeRange' true.
    [SerializeField] public bool isBoss;
    [SerializeField] public bool vis;
}

//Defines the types of wave elements
public enum ElementBehaviour
{
    SINGLE, BULK, WAIT
}