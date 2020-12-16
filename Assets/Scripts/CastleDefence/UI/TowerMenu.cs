using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public GameObject upgradeCost;
    public GameObject destroyCost;
    public Button upgradeButton;

	private Text upgradeText;
	private Text destroyText;
	private void Awake()
	{
		upgradeText = upgradeCost.GetComponent<Text>();
		destroyText = destroyCost.GetComponent<Text>();
	}


	public void SetTowerCost(int upgradePrice, int destroyPrice)
	{
		upgradeText.text = upgradePrice == 0 ? $"Max lvl" : $"- {upgradePrice}";
		upgradeButton.interactable = upgradePrice == 0 ? false : true;
		
		destroyText.text = $"+ {destroyPrice}";
	}

}
