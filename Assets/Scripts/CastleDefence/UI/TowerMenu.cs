using Assets.Scripts.CastleDefence.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CastleDefence.UI
{
	public class TowerMenu : MonoBehaviour
	{
		public ButtonGroup upgradeButton;
		public ButtonGroup destroyButton;

		private void Awake()
		{
			Init();
		}

		private void Init()
		{
			upgradeButton.Init(null, "", 0);
			destroyButton.Init(null, "", 0);
			destroyButton.type = ButtonGroup.ButtonType.Sell;
		}



		public void SetTowerCost(int upgradePrice, int destroyPrice)
		{
			upgradeButton.SetPrice(upgradePrice);
			destroyButton.SetPrice(destroyPrice);
		}

	}
}