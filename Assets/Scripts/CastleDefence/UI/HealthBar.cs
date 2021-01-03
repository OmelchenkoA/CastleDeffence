using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CastleDefence.UI
{
	public class HealthBar : MonoBehaviour
	{
		private Slider slider;
		private Text healthText;
		public bool lookAtCamera = true;
		public bool showNumbers = true;

		public void UpdateHealth(float currentHealth, float maxHealth)
		{
			if (slider == null)
				slider = GetComponentInChildren<Slider>();
			slider.value = (float)currentHealth / maxHealth;

			if (showNumbers)
			{
				if (healthText == null)
					healthText = GetComponentInChildren<Text>();
				healthText.text = $"{currentHealth}/{maxHealth}";
			}
		}

		private void LateUpdate()
		{
			if (lookAtCamera)
			{
				transform.rotation = Camera.main.transform.rotation;
			}
		}

	}
}