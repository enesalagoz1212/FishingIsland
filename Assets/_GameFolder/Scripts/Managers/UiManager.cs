using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Canvases;

namespace FishingIsland.Managers
{
	public class UiManager : MonoBehaviour
	{
		public static UiManager Instance { get; private set; }

		[SerializeField] private MenuCanvas menuCanvas;
		[SerializeField] private GameCanvas gameCanvas;
		[SerializeField] private InputCanvas inputCanvas;
		[SerializeField] private EndCanvas endCanvas;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		public void Initialize(GameManager gameManager, InputManager inputManager)
		{
			menuCanvas.Initialize(gameManager);
			inputCanvas.Initialize(inputManager);
			endCanvas.Initialize();
		}
	}
}

