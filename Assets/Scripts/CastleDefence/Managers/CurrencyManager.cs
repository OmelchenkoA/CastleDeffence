using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
	public static CurrencyManager instance;
	
	protected int Coins { get; set; }

	public event Action<int> OnCurrencyChanged;


	private void Awake()
	{
		if (instance == null)
			instance = this;
	}
	public int GetCurrency() => Coins;

	public void AddCurrency(int value)
	{
		SetCurrency(Coins + value);
	}

	public void DecreaseCurrency(int value)
	{
		SetCurrency(Coins - value);
	}

	public void SetCurrency(int value)
	{
		Coins = value >= 0? value : 0;
		OnCurrencyChanged?.Invoke(Coins);
	}
	
	public bool IsEnoughMoneyFor(int value) => Coins >= value;

}
