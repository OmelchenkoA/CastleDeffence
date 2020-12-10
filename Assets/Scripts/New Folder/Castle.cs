using System;
using UnityEngine;

public class Castle : Spawnable
{
	public int HP;
	private void Awake()
	{
		Restore();
	}

	public void Restore()
	{
		state = States.Idle;
		faction = Faction.Player;
		health = HP;
		maxHealth = HP;
		healthBar.UpdateHealth((float)health / maxHealth);
	}
}
