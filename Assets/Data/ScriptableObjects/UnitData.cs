using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Configs/Unit Data")]
public class UnitData : ScriptableObject
{
    public GameObject prefab;

    public Spawnable.Faction faction = Spawnable.Faction.None;
    public Spawnable.AttackType attackType = Spawnable.AttackType.Ranged;
    public Spawnable.TargetType targetType = Spawnable.TargetType.All;
    public float attackRatio = 1f; //time between attacks
    public float damagePerAttack = 2f; 
    public float attackRange = 1f;
    public float hitPoints = 10f;
    public float speed = 5f; //movement speed
    public float dropCoins = 5f;

}
