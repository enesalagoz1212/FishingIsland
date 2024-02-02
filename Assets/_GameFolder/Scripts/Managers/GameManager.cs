using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishingIsland.Controllers;

namespace FishingIsland.Managers
{
	public enum GameState
	{
		Menu = 0,
		Start = 1,
		Playing = 2,
		Reset = 3,
		LevelCompleted = 4,
	}
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		public GameState GameState { get; private set; }

		public static Action OnMenuOpen;
		public static Action OnGameStarted;
		public static Action<bool> OnGameLevelCompleted;
		public static Action OnGameReset;
		public static Action<int> OnDiamondScored;
		public static Action<int> OnMoneyScored;
		public static Action OnFishCollectionAndTransfer;
		public static Action OnCloseButton;
		public static Action OnButtonClickedDockUpgrade;
		public static Action OnButtonClickedShackUpgrade;
		public static Action OnButtonClickedHouseUpgrade;


		[SerializeField] private UiManager uiManager;
		[SerializeField] private InputManager inputManager;
		[SerializeField] private CameraController cameraController;
		[SerializeField] private BoatController boatController;
		[SerializeField] private FishBoxController fishBoxController;
		[SerializeField] private UpgradeManager upgradeManager;
		[SerializeField] private LevelManager levelManager;
		[SerializeField] private DockUpgradeManager dockUpgradeManager;
		[SerializeField] private ShackUpgradeManager shackUpgradeManager;
		[SerializeField] private HouseUpgradeManager houseUpgradeManager;
		[SerializeField] private MoneyManager moneyManager;
		[SerializeField] private SaveLoadManager saveLoadManager;
		[SerializeField] private DockController dockController;
		[SerializeField] private ShackController shackController;
		[SerializeField] private HouseController houseController;
		[SerializeField] private BaseCharacterController baseCharacterController;


		private void Awake()
		{
			if ( Instance !=null && Instance!=this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void Start()
		{
			GameInitialize();
		}

		private void GameInitialize()
		{
			uiManager.Initialize(this,inputManager);
			inputManager.Initialize(cameraController);
			cameraController.Initialize();
			boatController.Initialize();
			fishBoxController.Initialize();
			upgradeManager.Initialize(this);
			levelManager.Initialize();
			moneyManager.Initialize();
			dockUpgradeManager.Initialize();
			shackUpgradeManager.Initialize();
			houseUpgradeManager.Initialize();
			saveLoadManager.Initialize(boatController,dockController,houseController,shackController);
			dockController.Initialize(upgradeManager);
			shackController.Initialize(upgradeManager);
			houseController.Initialize(upgradeManager);


			ChangeState(GameState.Menu);
		}

		public void OnGameStart()
		{
			ChangeState(GameState.Start);
		}

		public void ResetGame()
		{

		}

		public void LevelCompleted(bool isSuccessful)
		{

		}

		public void ChangeState(GameState gameState)
		{
			GameState = gameState;
			Debug.Log($"GameState: {gameState}");
			switch (gameState)
			{
				case GameState.Menu:
					OnMenuOpen?.Invoke();
					break;
				case GameState.Start:
					OnGameStarted?.Invoke();
					ChangeState(GameState.Playing);
					break;
				case GameState.Playing:
					break;
				case GameState.Reset:
					break;
				case GameState.LevelCompleted:
					break;
				default:
					break;
			}

		}
	}
}

