using Assets.Scripts.CastleDefence.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TimeManager : MonoBehaviour
{
	[SerializeField] private GameSpeed[] gameSpeeds;

	private GameSpeed currentGameSpeed;
	private void Awake()
	{
		GameManager.instance.OnGameStarted += OnGameStarted;
		foreach (GameSpeed item in gameSpeeds)
			item.speedButton.onClick.AddListener(() => SwitchGameSpeed(item));
	}
	
	private void OnGameStarted()
	{
		currentGameSpeed = gameSpeeds[0];
		foreach (GameSpeed item in gameSpeeds)
			SetButtonColor(item.speedButton, Color.white);

		SwitchGameSpeed(currentGameSpeed);
	}

	public void SwitchGameSpeed(GameSpeed gameSpeed)
	{
		SetButtonColor(currentGameSpeed.speedButton, Color.white);
		SetGameSpeed(gameSpeed.speed);
		SetButtonColor(gameSpeed.speedButton, Color.green);
		currentGameSpeed = gameSpeed;
	}

	public void SetGameSpeed(float speedFactor)
	{
        Time.timeScale = 1f * speedFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}

	private void SetButtonColor(Button b, Color newColor)
	{
		var colors = b.colors;
		colors.normalColor = newColor;
		b.colors = colors;
	}

	[System.Serializable]
	public struct GameSpeed
	{
		public Button speedButton;
		public float speed;
	}

}
