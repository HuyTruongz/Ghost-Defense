using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UDEV.SPM;
using UDEV;
using UDEV.GhostDefense;

[System.Serializable]
public class WavePlayer : Singleton<WavePlayer>
{
    public List<Wave> WaveSet = new List<Wave>();   //List of all waves in a game.

    public Vector3 spawnlocation;       //The location where a enemy will be spawned if 'spawnAtObject' is false.
    public bool spawnAtObject;          //True if an enemy will be spawned at an object.
    public GameObject[] spawnObjects;      //The name of the object that a enemy will be spawned at if 'spawnAtObject' is true.

    public Vector3 randomSpawnPosition; //The random distance on each axis that an enemy can spawn.
    public bool debugSpawnPosition;     //Toggles in world spawn position.

    private int currentWaveIdx = 0;        //The current wave.
    private int waveCounter;            //The position within a wave.
    private bool inWave;                //True if a wave is currently playing.
    private bool allESpawned = false;   //True if all elements of a wave been spawned.
    private int lastCompleted = -1;     //The last wave that has been completed.

    public UnityEvent finalWaveComplete;
    public UnityEvent allSpawned;
    public UnityEvent waveHasEnded;
    public UnityEvent waveBegins;
    public UnityEvent resetWave;

    public List<GameObject> alive = new List<GameObject>(); //List of all enemies currently alive
    private float xpos, ypos, zpos;                         //List of random offests from the spawnpoint
    public static WavePlayer instance = null;               //Used to creates an static instance script

    public int CurrentWaveIdx { get => currentWaveIdx;}
    public Wave CurrentWave { get => WaveSet[currentWaveIdx]; }

    //Creates a non persistent singleton object 
    public override void Awake()
    {
        MakeSingleton(false);
    }

    #region Getters
    public int GetCurrentWave ()
    {
        return currentWaveIdx;
    }
    public int GetWavesLeft()
    {
        return WaveSet.Count - currentWaveIdx;
    }
    public int GetRemainingEnemies()
    {
        return alive.Count;
    }
    public bool CurrentlyInWave()
    {
        return inWave;
    }
    public int lastCompletedWave ()
    {
        return lastCompleted;
    }
    public bool GetAllEnemiesSpawned()
    {
        return allESpawned;
    }
    #endregion

    #region User Functions
    /// <summary>
    /// Starts the current wave.
    /// </summary>
    public void StartWave()
    {
        if (currentWaveIdx < WaveSet.Count)
        {
            GetTotalEnemy();
            inWave = true;
            waveBegins.Invoke();
            waveCounter = 0;
            StartCoroutine(readWave());
        }
    }

    /// <summary>
    /// Removes enemies and moves to the next wave.
    /// </summary>
    public void EndWave()
    {
        StopAllCoroutines();
        lastCompleted = currentWaveIdx;
        currentWaveIdx += 1;
        waveHasEnded.Invoke();
        //waveCounter = 0;
        allESpawned = false;
        inWave = false;
        if (currentWaveIdx == WaveSet.Count)
        {
            finalWaveComplete.Invoke();
        }
        
    }

    /// <summary>
    /// Removes all enemies but does not move to the next wave
    /// </summary>
    public void ResetCurrentWave()
    {
        StopAllCoroutines();
        foreach (GameObject i in alive)
        {
            i.SetActive(false);
        }
    }
    #endregion

    void GetTotalEnemy()
    {
        WaveElement currentElement;
        Wave currentSet;
        int elementCounter = 0;
        currentSet = WaveSet[currentWaveIdx];
        currentSet.totalEnemy = 0;
        currentSet.enemyKilled = 0;
        while (elementCounter < currentSet.elements.Count) //Loop though all elements
        {
            currentElement = WaveSet[currentWaveIdx].elements[elementCounter];

            switch (currentElement.behaviour)
            {
                case ElementBehaviour.BULK:
                    currentSet.totalEnemy += currentElement.bulkElements;
                    break;
                case ElementBehaviour.SINGLE:
                    currentSet.totalEnemy++;
                    break;
            }
            elementCounter++;
        }
    }

    public void AddEnemyKilled(int killed)
    {
        if (currentWaveIdx < WaveSet.Count)
        {
            Wave currentSet;

            currentSet = WaveSet[currentWaveIdx];

            currentSet.enemyKilled += killed;
        }
    }

    /// <summary>
    /// Reads the elements of a wave and performs all spawn / wait actions. Tests for end of a wave.
    /// </summary>
    private IEnumerator readWave()
    {
        WaveElement currentElement;
        Wave currentSet;

        currentSet = WaveSet[currentWaveIdx];
        while (waveCounter < currentSet.elements.Count) //Loop though all elements
        {
            
            currentElement = WaveSet[currentWaveIdx].elements[waveCounter];
            
            switch (currentElement.behaviour)
            {
                case ElementBehaviour.BULK:
                    int bulkCount = 0;
                    while (bulkCount < currentElement.bulkElements)
                    {
                        spawn(currentElement);
                        if (currentElement.useTimeRange)
                        {
                            yield return new WaitForSeconds(Random.Range(currentElement.timeRange.x, currentElement.timeRange.y));
                        }
                        else
                        {
                            yield return new WaitForSeconds(currentElement.time);
                        }
                        bulkCount += 1;
                    }
                    break;
                case ElementBehaviour.SINGLE:
                    spawn(currentElement, currentElement.isBoss);
                    break;
                case ElementBehaviour.WAIT:
                    if (currentElement.useTimeRange)
                    {
                        yield return new WaitForSeconds(Random.Range(currentElement.timeRange.x, currentElement.timeRange.y));
                    }
                    else
                    {
                        yield return new WaitForSeconds(currentElement.time);
                    }
                    break;
            }
            waveCounter++;
        }
        allESpawned = true;
        allSpawned.Invoke();
        
        //Test for the end of a wave
        while (alive.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            alive.RemoveAll(x => !x.activeInHierarchy);
            if (alive.Count == 0)
            {
                EndWave();
            }
        }
    }

    /// <summary>
    /// Adds a random value to a spawnpoint
    /// </summary>
    /// <param name="start">Starting spawnpoint</param>
    /// <param name="value">The range to spawn in</param>
    /// <returns></returns>
    private float addRandom (float start, float value)
    {
        return start + Random.Range((0 - value), value);
    }

    public Transform GetRandomSpawnPos()
    {
        if(spawnObjects != null && spawnObjects.Length > 0)
        {
            int randIdx = Random.Range(0, spawnObjects.Length);

            return spawnObjects[randIdx].transform;
        }
        return null;
    }

    /// <summary>
    /// Selects a spawnpoint and spawn the an enemy
    /// </summary>
    /// <param name="element">The element to spawn</param>
    private void spawn(WaveElement element, bool isBoss = false)
    {
        Vector3 spawnPos = GetRandomSpawnPos() != null ? GetRandomSpawnPos().position : Vector3.zero;
        Vector3 startPoint = (spawnAtObject) ? spawnPos : spawnlocation;

        xpos = addRandom(startPoint.x, randomSpawnPosition.x);
        ypos = addRandom(startPoint.y, randomSpawnPosition.y);
        zpos = addRandom(startPoint.z, randomSpawnPosition.z);
        startPoint = new Vector3(xpos, ypos, zpos);
        var aiClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE, (element.randomEnemy) ? element.enemySet.GetEnemy() : element.enemy, startPoint, new Quaternion(0, 0, 0, 0));
        if (aiClone)
        {
            AI ai = aiClone.GetComponent<AI>();
            if (ai)
            {
                //ai.isBoss = isBoss;
                ai.Init();
            }
        }
        alive.Add(
            aiClone
            );
    }
}