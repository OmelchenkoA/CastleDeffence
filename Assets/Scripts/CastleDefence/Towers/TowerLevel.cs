using Assets.Scripts.CastleDefence.Towers;
using UnityEngine;

public class TowerLevel : MonoBehaviour
{
	public Tower ParentTower { get; set; }
	public Transform[] projectileSpawnPoints;
	public GameObject gun;
}
