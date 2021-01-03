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
	private int m_currentWave;
	private int m_maxWave;
	private int m_enemiesInCurrentWave;
	private static GameObject EnemiesHolder;
	private int maxEnemiesInWave = 10;

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
	private void SpawnUnit(Node node, UnitData uData, int unitLevel)
	{

		Vector3 spawnPosition = node.GetRandomPointInNodeArea();	
		Quaternion rot = Quaternion.Euler(0f, -90f, 0f);

		GameObject prefabToSpawn = uData.Prefab;
		GameObject newSpawnable = Instantiate<GameObject>(prefabToSpawn, spawnPosition, rot, EnemiesHolder.transform);

		Unit unit = newSpawnable.GetComponent<Unit>();
		unit.Init(uData, unitLevel);

		OnUnitSpawned?.Invoke(unit);
		
	}

	IEnumerator SpawnWave(int waveNumber)
	{
		int unitLevel = (waveNumber / maxEnemiesInWave) / unitsToSpawn.Count + 1;

		for (int i = 0; i < Mathf.Clamp(waveNumber, 0, maxEnemiesInWave); i++)
		{
			if(waveNumber >= 10 && i < maxEnemiesInWave - waveNumber % maxEnemiesInWave)
			{
				var curUnitNum = (waveNumber / maxEnemiesInWave - 1) % unitsToSpawn.Count;
				var curUnitLevel = unitLevel > 1 && curUnitNum == unitsToSpawn.Count - 1 ? unitLevel - 1: unitLevel;
				SpawnUnit(nodeToSpawnIn, unitsToSpawn[curUnitNum], curUnitLevel);
			}
			else
				SpawnUnit(nodeToSpawnIn, unitsToSpawn[(waveNumber / maxEnemiesInWave) % unitsToSpawn.Count], unitLevel);
			yield return new WaitForSeconds(0.4f);
		}
	}

	public void StartWave(int waveNumber)
	{
		StartCoroutine(SpawnWave(waveNumber));
	}


}
