using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
	public Camera m_Camera;
    public static UiManager instance;

	[Header("Gameplay UI")]
	public GameObject newTowerMenu;
    public GameObject towerMenuGO;
    public GameObject currencyMenu;
    public GameObject levelPanel;

	[Header("Main menu")]
	public GameObject MenuPanel;
	public GameObject MainMenuPanel;
	public GameObject GameplayPanel;
	public GameObject StartButton;
	public GameObject UpgradesButton;

	[Header("GameOver")]
	public GameObject GameOverPanel;
	public GameObject MaxLevel;

	[Header("Other")]
	public Button newTowerButton;

	public LayerMask towerSelectionLayer;
    public PlacementTile selectedTile;
	public Tower currentSelectedTower;

	private InputControls inputActions;
	private TowerMenu towerMenu;

	private Text coinsText;
	private Text levelText;

	private enum UiStates
	{
		MainMenu,
		Play,
		Pause,
		GameOver
	};

	private void Awake()
	{
        if (instance == null)
            instance = this;

		SwitchUiState(UiStates.MainMenu);

		coinsText = currencyMenu.GetComponentInChildren<Text>();
		levelText = levelPanel.GetComponentInChildren<Text>();

		CurrencyManager.instance.OnCurrencyChanged += UI_OnCurrencyChanged;
		GameManager.instance.OnGameStarted += OnGameStarted;
		GameManager.instance.OnGoToMainMenu += OnGoToMainMenu;
		GameManager.instance.OnGameOver += OnGameOver;
		GameManager.instance.OnLevelChanged += OnLevelChanged;

		inputActions = new InputControls();
		inputActions.UI.MouseClick.performed += _ => MouseClick_performed();
		towerMenu = towerMenuGO.GetComponent<TowerMenu>();

		InitButtons();
	}

	private void OnGoToMainMenu()
	{
		SwitchUiState(UiStates.MainMenu);
		ClearSelection();
	}

	private void SwitchUiState(UiStates uiState)
	{
		switch (uiState)
		{
			case UiStates.MainMenu:
				MenuPanel.SetActive(true);
				MainMenuPanel.SetActive(true);
				GameOverPanel.SetActive(false);
				GameplayPanel.SetActive(false);
				break;
			case UiStates.Play:
				MenuPanel.SetActive(false);
				MainMenuPanel.SetActive(true);
				GameOverPanel.SetActive(false);
				GameplayPanel.SetActive(true);
				break;
			case UiStates.Pause:
				break;
			case UiStates.GameOver:
				MenuPanel.SetActive(true);
				MainMenuPanel.SetActive(false);
				GameOverPanel.SetActive(true);
				GameplayPanel.SetActive(false);
				break;
		}
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


	private void UI_OnCurrencyChanged(int currentCoins)
	{
		coinsText.text = "Coins: " + currentCoins;
	}
	private void OnLevelChanged(int level)
	{
		levelText.text = "Level: " + level;
	}
	private void OnGameOver(int level)
	{
		SwitchUiState(UiStates.GameOver);

		MaxLevel.GetComponent<Text>().text = "on Level: " + level;
	}
	private void OnGameStarted()
	{
		SwitchUiState(UiStates.Play);
	}



	public void SelectTile(PlacementTile tile)
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
		currentSelectedTower = tower;
		towerMenuGO.SetActive(true);
		UpdateTowerMenu(currentSelectedTower.upgradeCost, currentSelectedTower.destroyCost);
	}
	internal void ClearSelection()
	{
		if(currentSelectedTower != null)
		{
			towerMenuGO.SetActive(false);
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
	public void UpdateTowerMenu(int upgradePrice, int destroyPrice)
	{
		towerMenu.SetTowerCost(upgradePrice, destroyPrice);
	}
}
