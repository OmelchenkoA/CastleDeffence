using Assets.Data.ScriptableObjects;
using Assets.Scripts.CastleDefence.Managers;
using Assets.Scripts.CastleDefence.Placement;
using UnityEngine;

namespace Assets.Scripts.CastleDefence.Towers
{
	public class Tower : Spawnable
	{
		private Animator animator;

		public TowerData towerData;
		public int upgradeCost;
		public int destroyCost;
		public int currentLevel;

		private TowerLevel m_towerLevel;
		private PlacementTile m_placementTile;

		public PlacementTile PlacementTile { get => m_placementTile; set => m_placementTile = value; }

		private void Awake()
		{
			type = Type.Tower;
			animator = GetComponent<Animator>();
		}

		public void Init(TowerData tData)
		{
			state = States.Idle;
			towerData = tData;
			faction = tData.Faction;
			targetType = tData.TargetType;

			SetLevel(0);
		}

		public TowerSaveData GetTowerSaveData()
		{
			TowerSaveData towerSaveData = new TowerSaveData();
			towerSaveData.currentLevel = currentLevel;
			towerSaveData.towerId = towerData.Id;
			towerSaveData.tileId = PlacementTile.Id;

			return towerSaveData;
		}

		private void SetLevel(int level)
		{

			if (level >= 0 && level < towerData.TowerLevels.Length)
			{

				if (!CurrencyManager.instance.IsEnoughMoneyFor(towerData.TowerLevels[currentLevel].UpgradeCost))
				{
					Debug.Log("Not enough money to upgrade tower!");
					return;
				}

				currentLevel = level;

				health = towerData.TowerLevels[currentLevel].HitPoints;
				attackRange = towerData.TowerLevels[currentLevel].AttackRange;
				attackRatio = towerData.TowerLevels[currentLevel].AttackRatio;
				damage = towerData.TowerLevels[currentLevel].DamagePerAttack;
				destroyCost = towerData.TowerLevels[currentLevel].DestroyCost;
				upgradeCost = currentLevel + 1 >= towerData.TowerLevels.Length ? 0 : towerData.TowerLevels[currentLevel + 1].UpgradeCost;

				if (m_towerLevel != null)
					Destroy(m_towerLevel.gameObject);
				m_towerLevel = Instantiate(towerData.TowerLevels[currentLevel].Prefab, transform).GetComponent<TowerLevel>();
				m_towerLevel.ParentTower = this;

				projectileSpawnPoints = m_towerLevel.projectileSpawnPoints;
				gun = m_towerLevel.gun;

				CurrencyManager.instance.DecreaseCurrency(towerData.TowerLevels[currentLevel].UpgradeCost);
			}
		}

		public void UpgradeTower()
		{
			SetLevel(currentLevel + 1);
		}

		public void Destroy()
		{
			Die();
		}

		public override void Seek()
		{
			if (target == null)
				return;

			base.Seek();
		}

		public override void StartAttack()
		{
			base.StartAttack();
		}

		public override void DealBlow()
		{
			base.DealBlow();
			FireProjectile();
		}

		public override void Stop()
		{
			base.Stop();
		}

		protected override void Die()
		{
			base.Die();
		}
	}
}