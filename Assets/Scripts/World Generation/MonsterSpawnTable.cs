using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterDistribution
{
    Random,
    Even,
}

[System.Serializable]
public struct MonsterEncounter
{
    public GameObject[] monsterPrefabs;
    public int count;
    public MonsterDistribution distribution;
}

[CreateAssetMenu(fileName = "Monster Spawn Table", order = 55, menuName = "Data/Monster Spawn Table")]
public class MonsterSpawnTable : ScriptableObject
{
    public MonsterEncounter[] encounters;
}
