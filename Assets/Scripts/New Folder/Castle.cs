using UnityEngine;

public class Castle : Spawnable
{
	public int HP;
	private void Awake()
	{
		faction = Faction.Player;
		health = HP;
		maxHealth = HP;
	}
}
