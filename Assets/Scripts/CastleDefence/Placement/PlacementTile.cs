﻿using System;
using UnityEngine;

namespace Assets.Scripts.CastleDefence.Placement
{
	public class PlacementTile : MonoBehaviour
	{
		public GameObject SelectionVFX;
		public Color hoverColor;
		public Color selectColor;
		public static event EventHandler TileClicked;
		private GameObject turret;

		private Color startColor;

		private Renderer rend;
		private bool selected;
		private bool active;
		private int id;

		public int Id { get => id; set => id = value; }

		void Start()
		{
			rend = GetComponent<Renderer>();
			active = false;
			startColor = rend.material.color;
			selected = false;
		}

		private void OnMouseEnter()
		{
			rend.material.color = hoverColor;
		}

		private void OnMouseExit()
		{
			rend.material.color = selected ? selectColor : startColor;
		}

		private void OnMouseUpAsButton()
		{
			TileClicked?.Invoke(this, EventArgs.Empty);
		}

		public void Deselect()
		{
			selected = false;
			rend.material.color = startColor;
			SelectionVFX.SetActive(false);

		}
		public void Select()
		{
			selected = true;
			rend.material.color = selectColor;
			SelectionVFX.SetActive(true);
		}

		public void AnableSelectionView()
		{
			SelectionVFX.SetActive(true);
		}

		public void SetActive(bool anable)
		{
			active = anable;
			gameObject.SetActive(active);
		}
	}
}