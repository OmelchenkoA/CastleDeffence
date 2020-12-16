using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "Configs/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public GameObject prefab;
    [Space]
    public TowerLevelData[] towerLevels;

    public Spawnable.Faction faction = Spawnable.Faction.None;
    public Spawnable.AttackType attackType = Spawnable.AttackType.Ranged;
    public Spawnable.TargetType targetType = Spawnable.TargetType.All;
}
