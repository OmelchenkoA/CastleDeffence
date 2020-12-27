using Assets.Data.ScriptableObjects;
using CastleDefence.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
	[Header("UnitData List")]
    public List<UnitData> unitsToSpawn;
    public Node nodeToSpawnIn;

	public UnityAction<Spawnable> OnUnitSpawned;

	public int waveNumber;
	public int enemiesInWave;
	private int m_currentWave;
	private int m_maxWave;
	private int m_enemiesInCurrentWave;
	private static GameObject EnemiesHolder;

	private Queue<int> fastWaves;

	public int CurrentWave { get => m_currentWave; set => m_currentWave = value; }
	public int MaxWave { get => m_maxWave; set => m_maxWave = value; }

	public void Init(int currentLevel, int maxLevel)
	{
		fastWaves = new Queue<int>();
		m_currentWave = currentLevel;
		m_maxWave = maxLevel;
		if (EnemiesHolder == null)
		{
			EnemiesHolder = new GameObject("EnemiesHolder");
		}

		
	}
	public void StartNextWave()
	{
		if(m_currentWave == 0 && m_maxWave > 10)
		{
			for (int i = 1; i <= 5; i++)
			{
				fastWaves.Enqueue((int)(((float)m_maxWave / 5) * i));
			}
		}

		if (fastWaves.Count > 0)
			m_currentWave = fastWaves.Dequeue();
		else
			m_currentWave++;

		if (m_currentWave > m_maxWave)
			m_maxWave = m_currentWave;

		StartWave(m_currentWave);
	}
	private void SpawnUnit(Node node)
	{
		foreach (UnitData uData in unitsToSpawn)
		{
			Vector3 spawnPosition = node.GetRandomPointInNodeArea();	
			Quaternion rot = Quaternion.Euler(0f, -90f, 0f);

			GameObject prefabToSpawn = uData.Prefab;
			GameObject newSpawnable = Instantiate<GameObject>(prefabToSpawn, spawnPosition, rot, EnemiesHolder.transform);

			Unit unit = newSpawnable.GetComponent<Unit>();
			unit.Init(uData);

			OnUnitSpawned?.Invoke(unit);
		}
	}

	IEnumerator SpawnWave(int waveNumber)
	{
		for (int i = 0; i < enemiesInWave * waveNumber; i++)
		{
			SpawnUnit(nodeToSpawnIn);
			yield return new WaitForSeconds(0.4f);
		}
	}

	public void StartWave(int waveNumber)
	{
		StartCoroutine(SpawnWave(waveNumber));
	}


}
