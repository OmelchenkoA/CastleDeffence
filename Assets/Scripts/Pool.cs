using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool
{
	private GameObject m_prefab;
	private int m_poolSize;
	private bool m_expandable;
	private Transform m_parent;

	private List<GameObject> m_freeList;
	private List<GameObject> m_usedList;


	public Pool(GameObject prefab, int poolSize, bool expandable, Transform parent = null)
	{
		m_prefab = prefab;
		m_poolSize = poolSize;
		m_expandable = expandable;
		m_parent = parent;

		m_freeList = new List<GameObject>();
		m_usedList = new List<GameObject>();

		for (int i = 0; i < m_poolSize; i++)
		{
			InstantiateNewObject();
		}
	}


	public GameObject GetObject()
	{
		if (m_freeList.Count == 0 && m_expandable == false)
			return null;
		else if (m_freeList.Count == 0)
			InstantiateNewObject();

		GameObject obj = m_freeList.Last();
		m_freeList.Remove(obj);
		m_usedList.Add(obj);
		obj.SetActive(true);

		return obj;
	}

	public void ReturnObject(GameObject obj)
	{
		if (m_usedList.Contains(obj))
		{
			obj.SetActive(false);
			m_usedList.Remove(obj);
			m_freeList.Add(obj);
		}
	}

	private void InstantiateNewObject()
	{
		GameObject obj = UnityEngine.GameObject.Instantiate(m_prefab);
		if(m_parent != null)
			obj.transform.parent = m_parent;
		obj.SetActive(false);
		m_freeList.Add(obj);
	}
}
