using System;
using UnityEngine;
using Assets.Scripts.Upgrades;

public class Castle : Spawnable
{
	private int HP;
	private void Awake()
	{
		UpgradeSystem.OnUpgrade += UpgradeSystem_OnUpgrade;
	}

	private void UpgradeSystem_OnUpgrade(Upgrade obj)
	{
		if (obj.type == UpgradeType.Wall)
		{
			maxHealth += obj.levels[obj.currentLevel-1].value;
		}
	}

	public void Init(float HP)
	{
		state = States.Idle;
		faction = Faction.Player;
		maxHealth = HP;
		health = maxHealth;
		healthBar.UpdateHealth(health, maxHealth);
	}
	public void Restore()
	{
		state = States.Idle;
		faction = Faction.Player;
		health = maxHealth;
		healthBar.UpdateHealth(health, maxHealth);
	}
}
