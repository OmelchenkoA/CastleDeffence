using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CastleDefence.UI
{
	public class TowerMenu : MonoBehaviour
	{
		public GameObject upgradeCost;
		public GameObject destroyCost;
		public Button upgradeButton;

		private Text upgradeText;
		private Text destroyText;
		private void Awake()
		{
			Init();
		}

		private void Init()
		{
			upgradeText = upgradeCost.GetComponent<Text>();
			destroyText = destroyCost.GetComponent<Text>();
		}
		public void SetTowerCost(int upgradePrice, int destroyPrice)
		{
			if (upgradeText == null || destroyText == null)
				Init();
			upgradeText.text = upgradePrice == 0 ? $"Max lvl" : $"- {upgradePrice}";
			upgradeButton.interactable = upgradePrice == 0 ? false : true;

			destroyText.text = $"+ {destroyPrice}";
		}

	}
}