﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int StartCoins;

	public Spawnable Castle;
	public EnemySpawner enemySpawner;
	public BuildManager buildManager;

	public event Action OnGameStarted;
	public event Action OnGoToMainMenu;
	public event Action<int> OnLevelChanged;
	public event Action<int> OnGameOver;

	public TowerData[] towerConfigurations;

	[HideInInspector] public List<Spawnable> enemiesCurrentWave; 
	[HideInInspector] public List<Spawnable> castles; 
	[HideInInspector] public List<Spawnable> allSpawnable;

	private GameStates gameState;
	private int currentLevel;
	private enum GameStates
	{ 
		Menu, 
		Play, 
		Pause, 
		GameOver 
	};

	private void Awake()
	{
		gameState = GameStates.Menu;

		if (instance == null)
            instance = this;

		enemiesCurrentWave = new List<Spawnable>();
		castles = new List<Spawnable>();
		allSpawnable = new List<Spawnable>();

		castles.Add(Castle);
		Castle.OnDie += OnCastleDie;
		enemySpawner.OnUnitSpawned += OnNewUnitSpawned;
		buildManager.OnTowerBuild += OnTowerBuild;
	}

	private void OnCastleDie(Spawnable arg0)
	{
		gameState = GameStates.GameOver;
		StopAll();
		OnGameOver?.Invoke(currentLevel);
	}

	private void OnTowerBuild(Spawnable tower)
	{
		allSpawnable.Add(tower);
	}

	private void OnNewUnitSpawned(Spawnable unit)
	{
		enemiesCurrentWave.Add(unit);
		allSpawnable.Add(unit);
		unit.OnDealDamage += OnPlaceableDealtDamage;
	
	}

	// Start is called before the first frame update
	void Start()
    {
        CurrencyManager.instance.SetCurrency(StartCoins);

    }

    // Update is called once per frame
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
		currentLevel = 0;
		OnGameStarted?.Invoke();
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
			enemySpawner.StartWave(++currentLevel);
			OnLevelChanged?.Invoke(currentLevel);
		}
	}
	private void StopAll()
	{
		allSpawnable.ForEach(x => x.state = Spawnable.States.Idle);
	}
	private void ResetGameField()
	{
		CurrencyManager.instance.SetCurrency(StartCoins);
		castles.ForEach(c=>((Castle)c).Restore());
		allSpawnable.ForEach(s => s.state = Spawnable.States.Dead);
		UpdateSpawnables();
	}
	private void UpdateSpawnables()
	{

		Spawnable targetToPass; //ref
		Spawnable s; //ref
		for (int pN = 0; pN < allSpawnable.Count; pN++)
		{
			s = allSpawnable[pN];

			if (s.target == null)
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
					{
						s.StartAttack();
					}
					break;


				case Spawnable.States.Attacking:
					if (s.IsTargetInRange())
					{
						if (Time.time >= s.lastBlowTime + s.attackRatio)
						{
							s.DealBlow();
						}
					}
					break;

				case Spawnable.States.Dead:
					if (allSpawnable.Contains(s)) allSpawnable.Remove(s);
					if (enemiesCurrentWave.Contains(s)) enemiesCurrentWave.Remove(s);
					GameObject.Destroy(s.gameObject);
					break;
			}
		}
	}

	private List<Spawnable> GetAttackList(Spawnable.Faction f, Spawnable.TargetType t)
	{
		switch (t)
		{
			case Spawnable.TargetType.All:
				return (f == Spawnable.Faction.Player) ? enemiesCurrentWave : null;
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
			//p.target.healthBar.SetHealth(newHealth);
		}
	}
}
