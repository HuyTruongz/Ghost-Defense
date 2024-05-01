using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDEV.SPM;


[CreateAssetMenu(fileName = "New Weighted Enemy Set", menuName = "Wave Manager/Weighted Enemy Set")]
public class WeightedEnemySet : ScriptableObject
{
    [SerializeField] public List<WeightedEnemy> enemies = new List<WeightedEnemy>();
    [SerializeField] public int total;
    [SerializeField] public Texture2D icon;

    private void OnEnable()
    {
        icon = (Texture2D)Resources.Load("WeightedEnemySet");
    }

    public void updateTotal ()
    {
        int sum = 0;
        foreach (WeightedEnemy i in enemies)
        {
            sum += i.weight;
        }
        total = sum;
    }

    /// <summary>
    /// Selects a random enemy
    /// </summary>
    /// <returns>Returns an random enemy based on the set weight</returns>
    public string GetEnemy ()
    {
        List<WeightedEnemy> s = new List<WeightedEnemy>(enemies); 
        s.Sort(delegate (WeightedEnemy x, WeightedEnemy y) { return x.weight.CompareTo(y.weight); });

        int ran = Random.Range(0, total);

        string selected = s[enemies.Count - 1].spawn;
        foreach (var i in s)
        {
            if (ran < i.weight)
            {
                selected = i.spawn;
                break;
            }
            ran -= i.weight;
        }
        return selected;
    }
}

[System.Serializable]
public class WeightedEnemy
{
    [PoolerKeys(target = PoolerTarget.NONE)]
    [SerializeField] public string spawn;
    [SerializeField] public int weight;
}
