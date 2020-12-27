using Assets.Scripts.CastleDefence.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text value;
    [SerializeField] private Text price;
	[SerializeField] private Button button;

	[SerializeField] private Sprite buttonSprite;

	private Sprite m_sprite;
	private string m_value;
	private float m_price;

	public ButtonType type = ButtonType.Buy;

	public enum ButtonType
	{
		Buy,
		Sell
	}

	public void Init(Sprite newImage, string newValue, float newPrice)
	{
		m_sprite = buttonSprite != null ? buttonSprite : newImage;
		m_value = newValue;
		m_price = newPrice;

		value.gameObject.SetActive(value != null);
		image.gameObject.SetActive(m_sprite != null);
		CurrencyManager.instance.OnCurrencyChanged += OnCurrencyChanged;
		UpdateButton();
	}

	private void OnCurrencyChanged(int obj)
	{
		SetActive(CurrencyManager.instance.IsEnoughMoneyFor(m_price) && m_price > 0);
	}

	public void UpdateButton()
	{
		image.sprite = m_sprite;

		value.text = $"{m_value}";

		if (type == ButtonType.Sell)
			price.text = $"+ {m_price}";
		else
			price.text = m_price == 0 ? $"Max" : $"{m_price}";

		SetActive(CurrencyManager.instance.IsEnoughMoneyFor(m_price) && m_price > 0);
	}

	public void SetActive(bool active)
	{
		if (type == ButtonType.Buy)
		{
			price.color = active ? Color.black : m_price <= 0 ? Color.black : Color.red;
			button.interactable = active;
		}
	}


	private void SetButtonColor(Color newColor)
	{
		var colors = button.colors;
		colors.normalColor = newColor;
		button.colors = colors;
	}

	public void SetValue(string newValue)
	{
		m_value = newValue;
		UpdateButton();
	}
	public void SetPrice(float newPrice)
	{
		m_price = newPrice;
		UpdateButton();
	}

	public void AddListenerToButton(UnityAction unityAction)
	{
		button.onClick.AddListener(unityAction);
	}
}
