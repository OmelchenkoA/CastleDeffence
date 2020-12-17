using UnityEngine;
using System.Linq;
using Assets.Scripts.Upgrades;

namespace Assets.Scripts.CastleDefence.Placement
{
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
				placementTiles[i].Id = i;
				if (i < activeTilesOnStart)
					placementTiles[i].SetActive(true);
				else
					placementTiles[i].SetActive(false);
			}
		}

		public PlacementTile GetPlacementTile(int id)
		{
			return placementTiles.Where(t => t.Id.Equals(id)).FirstOrDefault();
		}

	}
}