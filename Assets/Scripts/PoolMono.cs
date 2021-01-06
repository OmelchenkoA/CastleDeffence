using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolMono : MonoBehaviour
{
	[SerializeField] private GameObject prefab;
	[SerializeField] private int poolSize;
	[SerializeField] private bool expandable;

	private Pool poolBase;

	private void Awake()
	{
		poolBase = new Pool(prefab, poolSize, expandable, transform);
	}

}
