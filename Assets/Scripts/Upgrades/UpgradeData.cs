using UnityEngine;

namespace Assets.Scripts.Upgrades
{
	[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Configs/Upgrade Data")]
	public class UpgradeData : ScriptableObject
	{
		[SerializeField] private UpgradeType type;
		[SerializeField] private string upgradeName;
		[SerializeField] private UpgradeLevel[] levels;

		public UpgradeType Type => type;
		public string UpgradeName => upgradeName;
		public UpgradeLevel[] Levels => levels;
	}
}