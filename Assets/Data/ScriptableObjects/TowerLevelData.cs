using UnityEngine;

namespace Assets.Data.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewTowerLevel", menuName = "Configs/Tower Level Data")]
	public class TowerLevelData : ScriptableObject
	{
		[SerializeField] private GameObject prefab;
		[Space]
		[SerializeField] private float attackRatio = 1f; //time between attacks
		[SerializeField] private float damagePerAttack = 2f;
		[SerializeField] private float attackRange = 1f;
		[SerializeField] private float hitPoints = 10f;
		[SerializeField] private int upgradeCost = 5;
		[SerializeField] private int destroyCost = 5;

		public GameObject Prefab => prefab;
		public float AttackRatio => attackRatio;
		public float DamagePerAttack => damagePerAttack;
		public float AttackRange => attackRange;
		public float HitPoints => hitPoints;
		public int UpgradeCost => upgradeCost;
		public int DestroyCost => destroyCost;
	}
}