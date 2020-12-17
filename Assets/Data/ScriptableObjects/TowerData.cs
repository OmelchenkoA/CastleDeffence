using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Data.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewTower", menuName = "Configs/Tower Data")]
	public class TowerData : ScriptableObject
	{
		[SerializeField] private string towerName;
		[SerializeField] private GameObject prefab;
		[Space]
		[SerializeField] private TowerLevelData[] towerLevels;
		[SerializeField] private Spawnable.Faction faction = Spawnable.Faction.None;
		[SerializeField] private Spawnable.AttackType attackType = Spawnable.AttackType.Ranged;
		[SerializeField] private Spawnable.TargetType targetType = Spawnable.TargetType.All;

		[ScriptableObjectId] [SerializeField] private string id;
		public string TowerName => towerName;
		public GameObject Prefab => prefab;
		public TowerLevelData[] TowerLevels => towerLevels;
		public Spawnable.Faction Faction => faction;
		public Spawnable.AttackType AttackType => attackType;
		public Spawnable.TargetType TargetType => targetType;
		public string Id => id;


	}

	public class ScriptableObjectIdAttribute : PropertyAttribute { }

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
	public class ScriptableObjectIdDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;
			if (string.IsNullOrEmpty(property.stringValue))
			{
				property.stringValue = Guid.NewGuid().ToString();
			}
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}
#endif
}