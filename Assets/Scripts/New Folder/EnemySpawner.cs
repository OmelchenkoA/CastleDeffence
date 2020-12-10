using CastleDefence.Nodes;
using Core.Utilities;
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

	private void SpawnUnit(Node node)
	{
		foreach (UnitData uData in unitsToSpawn)
		{
			Vector3 spawnPosition = node.GetRandomPointInNodeArea();	
			Quaternion rot = Quaternion.Euler(0f, -90f, 0f); ;

			GameObject prefabToSpawn = uData.prefab;
			GameObject newSpawnable = Instantiate<GameObject>(prefabToSpawn, spawnPosition, rot);

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

	internal void StartWave(int waveNumber)
	{
		StartCoroutine(SpawnWave(waveNumber));
	}


}
