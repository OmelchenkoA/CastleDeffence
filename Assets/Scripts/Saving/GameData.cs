using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Upgrades;

namespace Assets.Scripts.Saving
{
	[System.Serializable]
	public class GameData
	{
		public int activeTiles;
		public float gatesHp;
		public int currentWave;
		public int maxWave;
		public int coins;

		[HideInInspector] public List<UpgradeSaveData> upgradeSaveDatas;
		[HideInInspector] public List<TowerSaveData> towerSaveDatas;
	}
}