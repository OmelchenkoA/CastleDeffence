using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementTile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SelectionVFX;
    public Color hoverColor;
    public Color selectColor;
    public static event EventHandler TileClicked;
    private GameObject turret;
    
    private Color startColor;

    private Renderer rend;
    private bool active;
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        startColor = rend.material.color;
        active = false;
    }

	private void OnMouseEnter()
	{
        rend.material.color = hoverColor;
	}

	private void OnMouseExit()
	{
        rend.material.color = active ? selectColor : startColor;
    }

	private void OnMouseUpAsButton()
	{
        TileClicked?.Invoke(this, EventArgs.Empty);    
    }
    
	

	public void Deselect()
	{
        active = false;
        rend.material.color = startColor;
        SelectionVFX.SetActive(false);

    }
    public void Select()
    {
        active = true;
        rend.material.color = selectColor;
        SelectionVFX.SetActive(true);
    }

    public void AnableSelectionView()
	{
        SelectionVFX.SetActive(true);
    }
}
