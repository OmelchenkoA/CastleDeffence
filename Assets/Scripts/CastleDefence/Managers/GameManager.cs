using Assets.Scripts.CastleDefence.Placement;
using Assets.Scripts.CastleDefence.Towers;
using Assets.Scripts.Saving;
using Assets.Scripts.Upgrades;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CastleDefence.Managers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;

		public Castle castle;
		public EnemySpawner enemySpawner;
		public BuildManager buildManager;
		public PlacementTilesHolder placementTilesHolder;
		public UpgradeSystem upgradeSystem;

		public event Action OnGameStarted;
		public event Action OnGoToMainMenu;
		public event Action<int, int> OnLevelStarted;
		public event Action<int> OnGameOver;


		public GameData gameData;

		[HideInInspector] public List<Spawnable> enemiesCurrentWave;
		[HideInInspector] public List<Spawnable> towers;
		[HideInInspector] public List<Spawnable> castles;
		[HideInInspector] public List<Spawnable> allSpawnable;

		private GameStates gameState;

		private enum GameStates
		{
			Menu,
			Play,
			Pause,
			GameOver
		};

		private void Awake()
		{
			if (instance == null)
				instance = this;

			castle.OnDie += OnCastleDie;
			enemySpawner.OnUnitSpawned += OnNewUnitSpawned;
			buildManager.OnTowerBuild += OnTowerBuild;
		}
		private void Start()
		{
			InitGame();
		}

		private void InitGame()
		{
			gameState = GameStates.Menu;

			enemiesCurrentWave = new List<Spawnable>();
			castles = new List<Spawnable>();
			allSpawnable = new List<Spawnable>();
			castles.Add(castle);

			SetGameData(gameData);
		}

		private void OnCastleDie(Spawnable arg0)
		{
			gameState = GameStates.GameOver;
			StopAll();
			OnGameOver?.Invoke(enemySpawner.CurrentWave);
		}

		private void OnTowerBuild(Spawnable tower)
		{
			allSpawnable.Add(tower);
			towers.Add(tower);
		}

		private void OnNewUnitSpawned(Spawnable unit)
		{
			enemiesCurrentWave.Add(unit);
			allSpawnable.Add(unit);
			unit.OnDealDamage += OnPlaceableDealtDamage;
		}



		void Update()
		{
			switch (gameState)
			{
				case GameStates.Menu:
					ResetGameField();
					break;
				case GameStates.Play:
					UpdateWaves();
					UpdateSpawnables();
					break;
				case GameStates.Pause:
					break;
				case GameStates.GameOver:
					break;
			}
		}
		public void StartGame()
		{
			gameState = GameStates.Play;
			enemySpawner.CurrentWave = 0;
			enemySpawner.StartNextWave();
			OnLevelStarted?.Invoke(enemySpawner.CurrentWave, enemySpawner.MaxWave);
			OnGameStarted?.Invoke();
		}
		public void SaveGame()
		{
			SaveSystem.SaveGame(CollectGameData());
		}

		public void LoadGame()
		{
			GameData loadedGameData = SaveSystem.LoadGame();
			if (loadedGameData != null)
			{
				DeleteTowers();
				SetGameData(loadedGameData);
			}
		}
		public GameData CollectGameData()
		{
			GameData savingData = new GameData();
			savingData.activeTiles = placementTilesHolder.CountActiveTiles();
			savingData.coins = CurrencyManager.instance.GetCurrency();
			savingData.currentWave = enemySpawner.CurrentWave;
			savingData.maxWave = enemySpawner.MaxWave;
			savingData.gatesHp = castle.maxHealth;

			savingData.upgradeSaveDatas = upgradeSystem.GetUpgradeSaveData();
			savingData.towerSaveDatas = CollectTowerSaveData();

			return savingData;
		}
		public void SetGameData(GameData loadingData)
		{
			placementTilesHolder.Init(loadingData.activeTiles);
			CurrencyManager.instance.SetCurrency(loadingData.coins);
			enemySpawner.Init(loadingData.currentWave, loadingData.maxWave);
			castle.Init(loadingData.gatesHp);

			upgradeSystem.SetUpgradesFromLoad(loadingData.upgradeSaveDatas);
			buildManager.BuildLoadedTowers(loadingData.towerSaveDatas);
		}

		private List<TowerSaveData> CollectTowerSaveData()
		{
			List<TowerSaveData> towersList = new List<TowerSaveData>();
			towers.ForEach(t => towersList.Add((t as Tower).GetTowerSaveData()));
			return towersList;
		}

		public void GoToMainMenu()
		{
			gameState = GameStates.Menu;
			ResetGameField();
			OnGoToMainMenu?.Invoke();
		}
		private void UpdateWaves()
		{
			if (enemiesCurrentWave.Count == 0)
			{
				enemySpawner.StartNextWave();
				OnLevelStarted?.Invoke(enemySpawner.CurrentWave, enemySpawner.MaxWave);
			}
		}
		private void StopAll()
		{
			allSpawnable.ForEach(x => x.state = Spawnable.States.Idle);
		}
		private void ResetGameField()
		{
			castles.ForEach(c => ((Castle)c).Restore());
			enemiesCurrentWave.ForEach(s => s.state = Spawnable.States.Dead);
			UpdateSpawnables();
		}
		private void DeleteTowers()
		{
			towers.ForEach(s => s.state = Spawnable.States.Dead);
			UpdateSpawnables();
		}

		#region Battle System
		private void UpdateSpawnables()
		{
			Spawnable targetToPass;
			Spawnable s;
			for (int pN = 0; pN < allSpawnable.Count; pN++)
			{
				s = allSpawnable[pN];
				if (s.target == null && s.state != Spawnable.States.Dead)
					s.state = Spawnable.States.Idle;
				switch (s.state)
				{
					case Spawnable.States.Idle:
						if (s.targetType == Spawnable.TargetType.None)
							break;

						//find closest target and assign it to the Spawnable
						bool targetFound = FindClosestInList(s.transform.position, GetAttackList(s.faction, s.targetType), out targetToPass);
						if (targetFound)
						{
							s.SetTarget(targetToPass);
							s.Seek();
						}
						else
							Debug.Log("No more targets!");
						break;

					case Spawnable.States.Seeking:
						if (s.IsTargetInRange())
							s.StartAttack();
						break;

					case Spawnable.States.Attacking:
						if (s.IsTargetInRange())
						{
							if (Time.time >= s.lastBlowTime + s.attackRatio)
								s.DealBlow();
						}
						break;

					case Spawnable.States.Dead:
						if (allSpawnable.Contains(s)) allSpawnable.Remove(s);
						if (enemiesCurrentWave.Contains(s)) enemiesCurrentWave.Remove(s);
						if (towers.Contains(s)) enemiesCurrentWave.Remove(s);
						Destroy(s.gameObject);
						break;
				}
			}
		}

		private List<Spawnable> GetAttackList(Spawnable.Faction f, Spawnable.TargetType t)
		{
			switch (t)
			{
				case Spawnable.TargetType.All:
					return f == Spawnable.Faction.Player ? enemiesCurrentWave : null;
				case Spawnable.TargetType.Castle:
					return castles;
				case Spawnable.TargetType.Units:
					return enemiesCurrentWave;
				default:
					Debug.LogError("What faction is this?? Not Player nor Opponent.");
					return null;
			}
		}

		private bool FindClosestInList(Vector3 p, List<Spawnable> list, out Spawnable t)
		{
			t = null;
			bool targetFound = false;
			float closestDistanceSqr = Mathf.Infinity; //anything closer than here becomes the new designated target

			for (int i = 0; i < list.Count; i++)
			{
				float sqrDistance = (p - list[i].transform.position).sqrMagnitude;
				if (sqrDistance < closestDistanceSqr)
				{
					t = list[i];
					closestDistanceSqr = sqrDistance;
					targetFound = true;
				}
			}

			return targetFound;
		}


		private void OnPlaceableDealtDamage(Spawnable p)
		{
			if (p.target.state != Spawnable.States.Dead)
			{
				float newHealth = p.target.SufferDamage(p.damage);
			}
		}
		#endregion

	}
}