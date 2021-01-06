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

		
		public EnemySpawner enemySpawner;
		public BuildManager buildManager;
		public BattleSystem battleSystem;
		public PlacementTilesHolder placementTilesHolder;
		public UpgradeSystem upgradeSystem;

		public event Action OnGameStarted;
		public event Action OnGoToMainMenu;
		
		public event Action<int> OnGameOver;


		public GameData gameData;

		private GameStates gameState;
		private GameMode gameMode;

		private enum GameStates
		{
			Menu,
			Play,
			Pause,
			GameOver
		};
		private enum GameMode
		{
			Manual,
			Auto
		};

		private void Awake()
		{
			if (instance == null)
				instance = this;
			battleSystem.castle.OnDie += OnCastleDie;
		}

		private void Start()
		{
			InitGame();
		}

		private void InitGame()
		{
			gameState = GameStates.Menu;
			gameMode = GameMode.Manual;

			battleSystem.Init();

			SetGameData(gameData);
		}

		void Update()
		{
			switch (gameState)
			{
				case GameStates.Menu:
					battleSystem.ResetGameField();
					break;
				case GameStates.Play:
					battleSystem.UpdateWaves();
					battleSystem.UpdateSpawnables();
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
				battleSystem.DeleteTowers();
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
			savingData.gatesHp = battleSystem.castle.maxHealth;

			savingData.upgradeSaveDatas = upgradeSystem.GetUpgradeSaveData();
			savingData.towerSaveDatas = CollectTowerSaveData();

			return savingData;
		}
		public void SetGameData(GameData loadingData)
		{
			placementTilesHolder.Init(loadingData.activeTiles);
			CurrencyManager.instance.SetCurrency(loadingData.coins);
			enemySpawner.Init(loadingData.currentWave, loadingData.maxWave);
			battleSystem.castle.Init(loadingData.gatesHp);

			upgradeSystem.SetUpgradesFromLoad(loadingData.upgradeSaveDatas);
			buildManager.BuildLoadedTowers(loadingData.towerSaveDatas);
		}

		private List<TowerSaveData> CollectTowerSaveData()
		{
			List<TowerSaveData> towersList = new List<TowerSaveData>();
			battleSystem.towers.ForEach(t => towersList.Add((t as Tower).GetTowerSaveData()));
			return towersList;
		}

		public void GoToMainMenu()
		{
			gameState = GameStates.Menu;
			battleSystem.ResetGameField();
			OnGoToMainMenu?.Invoke();
		}

		private void OnCastleDie(Spawnable arg0)
		{
			gameState = GameStates.GameOver;
			OnGameOver?.Invoke(enemySpawner.CurrentWave);
		}


		private void OnDestroy()
		{
			battleSystem.castle.OnDie -= OnCastleDie;
		}
	}
}