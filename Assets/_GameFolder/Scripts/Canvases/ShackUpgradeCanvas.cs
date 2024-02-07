using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FishingIsland.Managers;
using FishingIsland.Controllers;
using FishingIsland.UpgradeScriptableObjects;

namespace FishingIsland.Canvases
{
    public class ShackUpgradeCanvas : MonoBehaviour
    {
		private ShackUpgrade _shackUpgrade;

		[Header("Buttons")]
		public Button shackCloseButton;
		public Button dockWorkerButton;
		public Button speedButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI dockWorkerLevelText;
		public TextMeshProUGUI speedLevelText;
		public TextMeshProUGUI capacityLevelText;

		private bool _canDockWorkerButton = true;
		private bool _canSpeedButton = true;
		private bool _canCapacityButton = true;
		public void Initialize()
		{
			shackCloseButton.onClick.AddListener(OnCloseButtonClick);
			dockWorkerButton.onClick.AddListener(OnDockWorkerButtonClick);
			speedButton.onClick.AddListener(OnSpeedButtonClick);
			capacityButton.onClick.AddListener(OnCapacityButtonClick);

			ShackUpgradeManager.OnShackUpgradeDockWorkerLevelUpdated += ShackUpgradeUpdateDockWorkerLevelText;
			ShackUpgradeManager.OnShackUpgradeSpeedLevelUpdated += ShackUpgradeUpdateSpeedLevelText;
			ShackUpgradeManager.OnShackUpgradeCapacityLevelUpdated += ShackUpgradeUpdateCapacityLevelText;

			ShackUpgradeData savedData = SaveLoadManager.Instance.LoadShackUpgradeData();
			ShackUpgradeManager.Instance.shackUpgradeData = savedData;
			ShackUpgradeManager.Instance.UpdateUpgradeCosts();

			ShackUpgradeUpdateDockWorkerLevelText(ShackUpgradeManager.Instance.GetDockWorkerLevel());
			ShackUpgradeUpdateSpeedLevelText(ShackUpgradeManager.Instance.GetSpeedLevel());
			ShackUpgradeUpdateCapacityLevelText(ShackUpgradeManager.Instance.GetCapacityLevel());
		}

		private void OnEnable()
		{
			GameManager.OnButtonClickedShackUpgrade += OnButtonClickedShackUpgradeAction;
			GameManager.OnGameReset += OnGameResetAction;
		}

		private void OnDisable()
		{
			GameManager.OnButtonClickedShackUpgrade -= OnButtonClickedShackUpgradeAction;
			GameManager.OnGameReset -= OnGameResetAction;
			
		}
		private void Start()
		{
			_shackUpgrade = ShackUpgradeManager.Instance.shackUpgrade;
			UpdateButtonInteractivityDockWorkerButton();
			UpdateButtonInteractivitySpeedButton();
			UpdateButtonInteractivityCapacityButton();
		}

		private void OnButtonClickedShackUpgradeAction()
		{
			int dockWorkerLevel = _shackUpgrade.shackUpgradeData.dockWorkerLevel;
			int speedLevel = _shackUpgrade.shackUpgradeData.speedLevel;
			int capacityLevel = _shackUpgrade.shackUpgradeData.capacityLevel;

			if (dockWorkerLevel == 10 && _canDockWorkerButton)
			{
				dockWorkerButton.interactable = false;
				_canDockWorkerButton = false;
			}

			if (speedLevel == 10 && _canSpeedButton)
			{
				speedButton.interactable = false;
				_canSpeedButton = false;
			}

			if (capacityLevel == 10 && _canCapacityButton)
			{
				capacityButton.interactable = false;
				_canCapacityButton = false;
			}

			if (dockWorkerLevel == 10 && speedLevel == 10 && capacityLevel == 10)
			{
				dockWorkerButton.interactable = true;
				speedButton.interactable = true;
				capacityButton.interactable = true;				
			}
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
			GameManager.OnCloseButton?.Invoke();
		}

		public void OnDockWorkerButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeDockWorkerLevel();
			UpdateButtonInteractivityDockWorkerButton();
			GameManager.OnButtonClickedShackUpgrade?.Invoke();
		}

		public void OnSpeedButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeSpeedLevel();
			UpdateButtonInteractivitySpeedButton();
			GameManager.OnButtonClickedShackUpgrade?.Invoke();
		}

		public void OnCapacityButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeCapacityLevel();
			UpdateButtonInteractivityCapacityButton();
			GameManager.OnButtonClickedShackUpgrade?.Invoke();
		}

		private void ShackUpgradeUpdateDockWorkerLevelText(int newDockWorkerLevel)
		{
			dockWorkerLevelText.text = $" {newDockWorkerLevel}";
		}

		private void ShackUpgradeUpdateSpeedLevelText(int newSpeedLevel)
		{
			speedLevelText.text = $" {newSpeedLevel}";
		}

		private void ShackUpgradeUpdateCapacityLevelText(int newCapacityLevel)
		{
			capacityLevelText.text = $" {newCapacityLevel}";
		}

		public void UpdateButtonInteractivityDockWorkerButton()
		{
			int maxLevelDockWorker = _shackUpgrade.MaxLevelShack();

			bool canUpgradeBoat = _shackUpgrade.shackUpgradeData.dockWorkerLevel < maxLevelDockWorker;
			dockWorkerButton.interactable = canUpgradeBoat;
		}

		public void UpdateButtonInteractivitySpeedButton()
		{
			int maxLevelSpeed = _shackUpgrade.MaxLevelShack();

			bool canUpgradeSpeed = _shackUpgrade.shackUpgradeData.speedLevel < maxLevelSpeed;
			speedButton.interactable = canUpgradeSpeed;
		}

		public void UpdateButtonInteractivityCapacityButton()
		{
			int maxLevelCapacity = _shackUpgrade.MaxLevelShack();

			bool canUpgradeCapacity = _shackUpgrade.shackUpgradeData.capacityLevel < maxLevelCapacity;
			capacityButton.interactable = canUpgradeCapacity;
		}

		private void OnGameResetAction()
		{
			dockWorkerButton.interactable = true;
			speedButton.interactable = true;
			capacityButton.interactable = true;
		}
	}
}

