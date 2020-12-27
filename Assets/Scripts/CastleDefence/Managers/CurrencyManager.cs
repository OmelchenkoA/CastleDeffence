using System;
using UnityEngine;

namespace Assets.Scripts.CastleDefence.Managers
{
	public class CurrencyManager : MonoBehaviour
	{
		public event Action<int> OnCurrencyChanged;

		public static CurrencyManager instance;
		protected int Coins { get; set; }


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
			Coins = value >= 0 ? value : 0;
			OnCurrencyChanged?.Invoke(Coins);
		}

		public bool IsEnoughMoneyFor(int value) => Coins >= value;
		public bool IsEnoughMoneyFor(float value) => Coins >= (int)value;

	}
}