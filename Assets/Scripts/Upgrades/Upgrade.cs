using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Upgrades
{
	[Serializable]
	public class UpgradeSaveData
	{
		public UpgradeType type;
		public int currentLevel;

		public UpgradeSaveData(Upgrade upgrade)
		{
			type = upgrade.type;
			currentLevel = upgrade.currentLevel;
		}

	}

	[Serializable]
	public struct UpgradeLevel
	{
		public float cost;
		public float value;
		[HideInInspector]
		public GameObject levelPoint;
	}
	[Serializable]
	public enum UpgradeType
	{
		Tile,
		Wall
	}
	public class Upgrade
	{
		public UpgradeType type;
		public string upgradeName;
		public int currentLevel;
		public List<UpgradeLevel> levels;
		public GameObject upgradeUiRow;
		public UpgradeField upgradeField;
	}
}