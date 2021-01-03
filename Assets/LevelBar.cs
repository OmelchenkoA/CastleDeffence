using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
	public Text LevelText;
	public bool lookAtCamera = true;

	public void SetLevel(int lvl)
	{
		LevelText.text = $"LVL {lvl}";
	}

	private void LateUpdate()
	{
		if (lookAtCamera)
		{
			transform.rotation = Camera.main.transform.rotation;
		}
	}
}
