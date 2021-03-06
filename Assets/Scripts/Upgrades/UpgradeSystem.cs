﻿using Assets.Scripts.CastleDefence.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Upgrades
{
	public class UpgradeSystem : MonoBehaviour
	{
		public static event Action<Upgrade> OnUpgrade;

		public UpgradeData[] upgradeDatas;
		public GameObject upgradesPanel;
		public GameObject upgradePrefab;

		[HideInInspector]
		public List<Upgrade> upgrades;

		private void Awake()
		{
			upgrades = new List<Upgrade>();
			InitUpgradesUi();
		}

		private void InitUpgradesUi()
		{
			foreach (UpgradeData upgradeData in upgradeDatas)
			{
				if (upgradeData==null || upgrades.Where(u => u.type.Equals(upgradeData.Type)).Count() > 0)
					continue;

				Upgrade upgrade = new Upgrade();
				upgrade.upgradeName = upgradeData.UpgradeName;
				upgrade.type = upgradeData.Type;
				upgrade.currentLevel = 0;
				
				upgrade.upgradeUiRow = GameObject.Instantiate(upgradePrefab, upgradesPanel.transform);
				upgrade.upgradeField = upgrade.upgradeUiRow.GetComponent<UpgradeField>();
				upgrade.upgradeField.upgradeLabel.text = upgradeData.UpgradeName;

				upgrade.levels = new List<UpgradeLevel>();
				for (int i = 0; i < upgradeData.Levels.Length; i++)
				{
					UpgradeLevel level = new UpgradeLevel();
					level.levelPoint = GameObject.Instantiate(upgrade.upgradeField.levelPointPrefab, upgrade.upgradeField.levels.transform);
					level.cost = upgradeData.Levels[i].cost;
					level.value = upgradeData.Levels[i].value;
					upgrade.levels.Add(level);
				}
				upgrade.upgradeField.buttonGroup.AddListenerToButton(() => { DoUpdrade(upgrade); });
				upgrade.upgradeField.buttonGroup.Init(null, "", 0);
				UpdateUpgradeLevelUI(upgrade);
				upgrades.Add(upgrade);
			}
		}

		public void DoUpdrade(Upgrade upgrade)
		{
			if(upgrade.currentLevel < upgrade.levels.Count)
			{
				if(CurrencyManager.instance.IsEnoughMoneyFor((int)upgrade.levels[upgrade.currentLevel].cost))
				{
					upgrade.currentLevel++;
					UpdateUpgradeLevelUI(upgrade);
					CurrencyManager.instance.DecreaseCurrency((int)upgrade.levels[upgrade.currentLevel - 1].cost);
					OnUpgrade?.Invoke(upgrade);
				}
				else
				{
					Debug.Log("Not enough money");
				}
			}
		}

		private void UpdateUpgradeLevelUI(Upgrade upgrade)
		{
			for (int i = 0; i < upgrade.levels.Count; i++)
			{
				upgrade.levels[i].levelPoint.GetComponent<Image>().color = i < upgrade.currentLevel ? Color.green : Color.gray;
			}
			
			if (upgrade.currentLevel < upgrade.levels.Count)
			{
				upgrade.upgradeField.buttonGroup.SetValue($"{upgrade.levels[upgrade.currentLevel].value}");
				upgrade.upgradeField.buttonGroup.SetPrice(upgrade.levels[upgrade.currentLevel].cost);
			}
			else
			{
				upgrade.upgradeField.buttonGroup.SetValue($"Max");
				upgrade.upgradeField.buttonGroup.SetPrice(0);
			}
		}

		public List<UpgradeSaveData> GetUpgradeSaveData()
		{
			List<UpgradeSaveData> upgradeSaveDatas = new List<UpgradeSaveData>();

			upgrades.ForEach(u => upgradeSaveDatas.Add(new UpgradeSaveData(u)));
			
			return upgradeSaveDatas;
		}

		public void SetUpgradesFromLoad(List<UpgradeSaveData> upgradeSaveDatas)
		{
			foreach (Upgrade upgrade in upgrades)
			{
				UpgradeSaveData upgradeSaveData = upgradeSaveDatas.Where(u => u.type.Equals(upgrade.type)).FirstOrDefault();
				if(upgradeSaveData != null)
				{
					upgrade.currentLevel = upgradeSaveData.currentLevel;

					for (int i = 0; i < upgrade.currentLevel; i++)
						UpdateUpgradeLevelUI(upgrade);

				}
			}
		}

	}
}