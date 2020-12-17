using Assets.Data.ScriptableObjects;
using Assets.Scripts.CastleDefence.Placement;
using Assets.Scripts.CastleDefence.Towers;
using Assets.Scripts.CastleDefence.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CastleDefence.Managers
{
	public class BuildManager : MonoBehaviour
	{
		public static BuildManager instance;

		public TowerData[] towerConfigurations;
		public PlacementTilesHolder placementTilesholder;
		private PlacementTile selectedTile;
		private Tower selectedTower;

		public UnityAction<Spawnable> OnTowerBuild;
		private void Awake()
		{
			if (instance == null)
				instance = this;
		}


		void Update()
		{

		}

		public void BuildTower(TowerData towerToBuild)
		{
			if (!CurrencyManager.instance.IsEnoughMoneyFor(towerToBuild.TowerLevels[0].UpgradeCost))
			{
				Debug.Log("Not enough money to build tower!");
				return;
			}
			selectedTile = UiManager.instance.selectedTile;
			GameObject newTowerGO = Instantiate(towerToBuild.Prefab.gameObject, selectedTile.transform.position, selectedTile.transform.rotation);
			Tower deffenceTower = newTowerGO.GetComponent<Tower>();
			deffenceTower.Init(towerToBuild);
			deffenceTower.PlacementTile = selectedTile;
			OnTowerBuild?.Invoke(deffenceTower);

			UiManager.instance.SelectTower(deffenceTower);
		}

		public void DestroyTower()
		{
			selectedTower = UiManager.instance.currentSelectedTower;
			UiManager.instance.SelectTile(selectedTower.PlacementTile);
			CurrencyManager.instance.AddCurrency(selectedTower.destroyCost);

			selectedTower.Destroy();
		}


		public void UpgradeTower()
		{
			selectedTower = UiManager.instance.currentSelectedTower;
			selectedTower.UpgradeTower();
			UiManager.instance.UpdateTowerMenu(selectedTower.upgradeCost, selectedTower.destroyCost);
		}

		public void BuildLoadedTowers(List<TowerSaveData> towerSaveDatas)
		{
			int tempCoins = CurrencyManager.instance.GetCurrency();
			CurrencyManager.instance.SetCurrency(100000);

			foreach (TowerSaveData towerSaveData in towerSaveDatas)
			{
				TowerData towerToBuild = towerConfigurations.Where(t => t.Id.Equals(towerSaveData.towerId)).FirstOrDefault();
				PlacementTile tile = placementTilesholder.GetPlacementTile(towerSaveData.tileId);
				if (towerToBuild != null && tile != null)
				{
					GameObject newTowerGO = Instantiate(towerToBuild.Prefab.gameObject, tile.transform.position, tile.transform.rotation);
					Tower deffenceTower = newTowerGO.GetComponent<Tower>();
					deffenceTower.Init(towerToBuild);
					deffenceTower.PlacementTile = tile;
					OnTowerBuild?.Invoke(deffenceTower);

					UiManager.instance.SelectTower(deffenceTower);
					while (deffenceTower.currentLevel < towerSaveData.currentLevel)
						UpgradeTower();
				}

			}
			CurrencyManager.instance.SetCurrency(tempCoins);
		}


	}
}