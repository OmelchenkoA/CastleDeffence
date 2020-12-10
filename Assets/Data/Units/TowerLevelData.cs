using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerLevel", menuName = "Configs/Tower Level Data")]
public class TowerLevelData : ScriptableObject
{
    public GameObject prefab;
    [Space]
    public float attackRatio = 1f; //time between attacks
    public float damagePerAttack = 2f; 
    public float attackRange = 1f;
    public float hitPoints = 10f;
    public float speed = 5f; //movement speed
    public int upgradeCost = 5; 
    public int destroyCost = 5; 

}
