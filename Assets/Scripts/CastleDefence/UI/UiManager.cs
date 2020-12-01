using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
	public Camera m_Camera;
    public static UiManager instance;

    public GameObject newTowerMenu;
    public GameObject towerMenu;
    public GameObject currencyMenu;

	public Button newTowerButton;

	public LayerMask towerSelectionLayer;
    public PlacementTile selectedTile;
	public Tower currentSelectedTower;

	private InputControls inputActions;


	private Text coinsText;

	private void Awake()
	{
        if (instance == null)
            instance = this;

		coinsText = currencyMenu.GetComponentInChildren<Text>();
		CurrencyManager.instance.OnCurrencyChanged += UI_OnCurrencyChanged;

		inputActions = new InputControls();
		inputActions.UI.MouseClick.performed += _ => MouseClick_performed();


		InitButtons();
	}
	private void OnEnable()
	{
		inputActions.Enable();
	}

	private void OnDisable()
	{
		inputActions.Disable();
	}


	private void MouseClick_performed()
	{

		Vector2 mousePos = inputActions.UI.MousePosition.ReadValue<Vector2>();
		RaycastHit output;
		Ray ray = m_Camera.ScreenPointToRay(mousePos);
		bool hasHit = Physics.Raycast(ray, out output, float.MaxValue, towerSelectionLayer);
		if (!hasHit)
		{
			return;
		}

		Tower tower = output.collider.GetComponent<TowerLevel>()?.ParentTower;
		if (tower != null)
		{
			SelectTower(tower);
			return;
		}

		PlacementTile tile = output.collider.GetComponent<PlacementTile>();
		if (tile != null)
		{
			SelectTile(tile);
		}
	}

	private void InitButtons()
	{
		foreach (TowerData towerData in GameManager.instance.towerConfigurations)
		{
			Button button = Instantiate(newTowerButton);
			button.transform.SetParent(newTowerMenu.transform);
			button.GetComponentInChildren<Text>().text = towerData.name;
			button.onClick.AddListener(() => {BuildManager.instance.BuildTower(towerData); });
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		

		
    }

	private void UI_OnCurrencyChanged(int currentCoins)
	{
		coinsText.text = "Coins: " + currentCoins;
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void SelectTile(PlacementTile tile)
	{
		ClearSelection();
		newTowerMenu.SetActive(true);
		selectedTile = tile;
		selectedTile.Select();
	}

	public void SelectTower(Tower tower)
	{
		ClearSelection();
		tower.PlacementTile.AnableSelectionView();
		towerMenu.SetActive(true);
		currentSelectedTower = tower;
	}


	internal void ClearSelection()
	{
		if(currentSelectedTower != null)
		{
			towerMenu.SetActive(false);
			currentSelectedTower.PlacementTile.Deselect();
			currentSelectedTower = null;
		}

		if (selectedTile != null)
		{
			newTowerMenu.SetActive(false);
			selectedTile.Deselect();
			selectedTile = null;
		}
	}
}
