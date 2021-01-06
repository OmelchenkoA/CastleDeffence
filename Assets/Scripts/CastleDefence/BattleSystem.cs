using Assets.Scripts.CastleDefence.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
	public Castle castle;
	[HideInInspector] public List<Spawnable> enemiesCurrentWave;
	[HideInInspector] public List<Spawnable> towers;
	[HideInInspector] public List<Spawnable> castles;
	[HideInInspector] public List<Spawnable> allSpawnable;

	public BuildManager buildManager;
	public EnemySpawner enemySpawner;

	private void Awake()
	{
		buildManager.OnTowerBuild += OnTowerBuild;
		enemySpawner.OnUnitSpawned += OnNewUnitSpawned;
		castle.OnDie += OnCastleDie;
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
		unit.OnDie += onUnitDie;
	}

	private void onUnitDie(Spawnable unit)
	{
		allSpawnable.Remove(unit);
		enemiesCurrentWave.Remove(unit);
		towers.Remove(unit);

		unit.OnDealDamage -= OnPlaceableDealtDamage;
		unit.OnDie -= onUnitDie;
	}
	private void OnCastleDie(Spawnable arg0)
	{
		StopAll();
	}

	public void Init()
	{
		enemiesCurrentWave = new List<Spawnable>();
		castles = new List<Spawnable>();
		allSpawnable = new List<Spawnable>();
		castles.Add(castle);

	}

	public void UpdateWaves()
	{
		if (enemiesCurrentWave.Count == 0)
		{
			enemySpawner.StartNextWave();
		}
	}
	private void StopAll()
	{
		allSpawnable.ForEach(x => x.state = Spawnable.States.Idle);
	}
	public void ResetGameField()
	{
		castles.ForEach(c => ((Castle)c).Restore());
		enemiesCurrentWave.ForEach(s => s.state = Spawnable.States.Dead);
		UpdateSpawnables();
	}
	public void DeleteTowers()
	{
		towers.ForEach(s => s.state = Spawnable.States.Dead);
		UpdateSpawnables();
	}


	public void UpdateSpawnables()
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

	private void OnDestroy()
	{
		buildManager.OnTowerBuild -= OnTowerBuild;
		enemySpawner.OnUnitSpawned -= OnNewUnitSpawned;
		castle.OnDie -= OnCastleDie;
	}
}
