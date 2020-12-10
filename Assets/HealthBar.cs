using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
	public bool lookAtCamera = true;

    public void UpdateHealth(float normalizedHealth)
    {
		if(slider == null)
			slider = GetComponentInChildren<Slider>();
		slider.value = normalizedHealth;
    }

	private void LateUpdate()
	{
		if(lookAtCamera)
		{
			transform.LookAt(Camera.main.transform);
			transform.Rotate(0, 180, 0);

		}
	}

}
