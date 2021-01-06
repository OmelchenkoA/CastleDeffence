using UnityEngine;

namespace Assets.Data.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewUnit", menuName = "Configs/Unit Data")]
	public class UnitData : ScriptableObject
	{
		[SerializeField] private GameObject prefab;

		[SerializeField] private Spawnable.Faction faction = Spawnable.Faction.None;
		[SerializeField] private Spawnable.AttackType attackType = Spawnable.AttackType.Ranged;
		[SerializeField] private Spawnable.TargetType targetType = Spawnable.TargetType.All;
		[SerializeField] private float attackRatio = 1f; //time between attacks
		[SerializeField] private float damagePerAttack = 2f;
		[SerializeField] private float attackRange = 1f;
		[SerializeField] private float hitPoints = 10f;
		[SerializeField] private float speed = 5f;
		[SerializeField] private float dropCoins = 5f;
		[ScriptableObjectId] [SerializeField] private string id;

		public GameObject Prefab => prefab;
		public Spawnable.Faction Faction => faction;
		public Spawnable.AttackType AttackType => attackType;
		public Spawnable.TargetType TargetType => targetType;
		public float AttackRatio => attackRatio;
		public float DamagePerAttack => damagePerAttack;
		public float AttackRange => attackRange;
		public float HitPoints => hitPoints;
		public float Speed => speed;
		public float DropCoins => dropCoins;
		public string Id => id;

	}
}