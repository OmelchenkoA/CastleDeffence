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

	public int waveNumber = 1;
	public int enemiesInWave = 5;
	public float delayWave = 5;
	private float countdown;


	private void Awake()
	{

	}

	private void SpawnUnit(Node node)
	{
		foreach (UnitData uData in unitsToSpawn)
		{
			Vector3 spawnPosition = node.GetRandomPointInNodeArea();	

			Quaternion rot = Quaternion.Euler(0f, -90f, 0f); ;
			//Quaternion rot = Quaternion.identity;

			GameObject prefabToSpawn = uData.prefab;
			GameObject newSpawnable = Instantiate<GameObject>(prefabToSpawn, spawnPosition, rot);

			Unit unit = newSpawnable.GetComponent<Unit>();
			unit.Init(uData);

			OnUnitSpawned?.Invoke(unit);
			
			//unit.OnDealDamage += OnSpawnableDealtDamage;
			//unit.OnProjectileFired += OnProjectileFired;
			//AddSpawnableToList(unit); //add the Unit to the appropriate list
			//UIManager.AddHealthUI(unit);
		}
	}

	IEnumerator SpawnWave()
	{
		for (int i = 0; i < enemiesInWave * waveNumber; i++)
		{
			SpawnUnit(nodeToSpawnIn);
			yield return new WaitForSeconds(0.4f);
		}
	}

	private void Spawning()
	{
		if (countdown <= 0f)
		{
			StartCoroutine(SpawnWave());
			waveNumber++;
			countdown = delayWave;
		}
		countdown -= Time.deltaTime;
	}

	internal void NextWave()
	{
		waveNumber++;
		StartCoroutine(SpawnWave());
	}

	private void Update()
	{
		//Spawning();
	}
}
