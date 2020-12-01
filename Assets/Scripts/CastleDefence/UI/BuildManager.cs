using UnityEngine;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
	public static BuildManager instance;

    public PlacementTile[] placementTiles;
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
		if (!CurrencyManager.instance.IsEnoughMoneyFor(towerToBuild.towerLevels[0].upgradeCost))
		{
			Debug.Log("Not enough money to build tower!");
			return;
		}
		selectedTile = UiManager.instance.selectedTile;
		GameObject newTowerGO = (GameObject)Instantiate(towerToBuild.prefab.gameObject, selectedTile.transform.position, selectedTile.transform.rotation);
		Tower deffenceTower = newTowerGO.GetComponent<Tower>();
		deffenceTower.Init(towerToBuild);
		deffenceTower.PlacementTile = selectedTile;
		OnTowerBuild?.Invoke(deffenceTower);

		UiManager.instance.SelectTower(deffenceTower);
	}

	public void UpgradeTower()
	{
		selectedTower = UiManager.instance.currentSelectedTower;
		selectedTower.UpgradeTower();
	}




}
