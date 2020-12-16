using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets.Scripts.Upgrades;

public class PlacementTilesHolder : MonoBehaviour
{
    public PlacementTile[] placementTiles;
	
	public int CountActiveTiles()
	{
		return placementTiles.Where(t => t.isActiveAndEnabled == true).Count();
	}

	private void Awake()
	{
		UpgradeSystem.OnUpgrade += PlacementTilesHolder_OnUpgrade;
	}

	private void PlacementTilesHolder_OnUpgrade(Upgrade obj)
	{
		if (obj.type == UpgradeType.Tile)
			EnableNextTile();
	}

	private void EnableNextTile()
	{
		placementTiles.Where(t => t.isActiveAndEnabled == false).FirstOrDefault().SetActive(true);
	}

	public void Init(int activeTilesOnStart)
	{
		for (int i = 0; i < placementTiles.Length; i++)
		{
			if(i < activeTilesOnStart)
				placementTiles[i].SetActive(true);
			else
				placementTiles[i].SetActive(false);
		}
	}



}
