using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Upgrades
{
	[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Configs/Upgrade Data")]
	public class UpgradeData : ScriptableObject
	{
		public UpgradeType type;
		public string upgradeName;
		public UpgradeLevel[] levels;
	}
}