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
		End = 4,
	}
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		public GameState GameState { get; private set; }

		public static Action OnMenuOpen;
		public static Action OnGameStarted;
		public static Action<bool> OnGameEnd;
		public static Action OnGameReset;
		public static Action<int> OnDiamondScored;
		public static Action<int> OnMoneyScored;

		[SerializeField] private UiManager uiManager;
		[SerializeField] private InputManager inputManager;
		[SerializeField] private CameraController cameraController;
		[SerializeField] private CharacterManager characterManager;
		[SerializeField] private PlayerController playerController;
		[SerializeField] private BoatController boatController;
		[SerializeField] private Player player;
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
			uiManager.Initialize(this,inputManager,characterManager);
			inputManager.Initialize(cameraController);
			cameraController.Initialize();
			characterManager.Initialize();
			playerController.Initialize(boatController);
			boatController.Initialize(playerController);

			ChangeState(GameState.Menu);
		}

		public void OnGameStart()
		{
			ChangeState(GameState.Start);
		}

		public void ResetGame()
		{

		}

		public void GameEnd(bool isSuccessful)
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
				case GameState.End:
					break;
				default:
					break;
			}

		}
	}
}
