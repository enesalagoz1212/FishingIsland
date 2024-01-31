using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;
using TMPro;
using FishingIsland.Controllers;
using FishingIsland.UpgradeScriptableObjects;

namespace FishingIsland.Canvases
{
	public class DockUpgradeCanvas : MonoBehaviour
	{
		private DockUpgrade _dockUpgrade;

		[Header("Buttons")]
		public Button dockCloseButton;
		public Button boatButton;
		public Button speedButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI boatLevelText;
		public TextMeshProUGUI speedLevelText;
		public TextMeshProUGUI capacityLevelText;

		public void Initialize()
		{
			dockCloseButton.onClick.AddListener(OnCloseButtonClick);
			boatButton.onClick.AddListener(OnBoatButtonClick);
			speedButton.onClick.AddListener(OnSpeedButtonClick);
			capacityButton.onClick.AddListener(OnCapacityButtonClick);

			DockUpgradeManager.OnBoatLevelUpdated += UpdateBoatLevelText;
			DockUpgradeManager.OnSpeedLevelUpdated += UpdateSpeedLevelText;
			DockUpgradeManager.OnCapacityLevelUpdated += UpdateCapacityLevelText;

			DockUpgradeData savedData = SaveLoadManager.Instance.LoadDockUpgradeData();
			DockUpgradeManager.Instance.dockUpgradeData = savedData;
			DockUpgradeManager.Instance.UpdateUpgradeCosts();

			UpdateBoatLevelText(DockUpgradeManager.Instance.GetBoatLevel());
			UpdateSpeedLevelText(DockUpgradeManager.Instance.GetSpeedLevel());
			UpdateCapacityLevelText(DockUpgradeManager.Instance.GetCapacityLevel());

		
		}

		private void Start()
		{
			_dockUpgrade = DockUpgradeManager.Instance.dockUpgrade;
			UpdateButtonInteractivityBoatButton();
			UpdateButtonInteractivitySpeedButton();
			UpdateButtonInteractivityCapacityButton();
		}
		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
			GameManager.OnCloseButton?.Invoke();
		}

		public void OnBoatButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeBoatLevel();
			UpdateButtonInteractivityBoatButton();
		}

		public void OnSpeedButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeSpeedLevel();
			UpdateButtonInteractivitySpeedButton();
		}

		public void OnCapacityButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeCapacityLevel();
			UpdateButtonInteractivityCapacityButton();
		}

		private void UpdateBoatLevelText(int newBoatLevel)
		{
			boatLevelText.text = $" {newBoatLevel}";
		}

		private void UpdateSpeedLevelText(int newSpeedLevel)
		{
			speedLevelText.text = $" {newSpeedLevel}";
		}

		private void UpdateCapacityLevelText(int newCapacityLevel)
		{
			capacityLevelText.text = $" {newCapacityLevel}";
		}

		public void UpdateButtonInteractivityBoatButton()
		{
			int maxLevelBoat = _dockUpgrade.MaxLevelDock();

			bool canUpgradeBoat = _dockUpgrade.dockUpgradeData.boatLevel < maxLevelBoat;
			boatButton.interactable = canUpgradeBoat;
		}

		public void UpdateButtonInteractivitySpeedButton()
		{
			int maxLevelSpeed = _dockUpgrade.MaxLevelDock();

			bool canUpgradeSpeed = _dockUpgrade.dockUpgradeData.speedLevel < maxLevelSpeed;
			speedButton.interactable = canUpgradeSpeed;
		}

		public void UpdateButtonInteractivityCapacityButton()
		{
			int maxLevelCapacity = _dockUpgrade.MaxLevelDock();

			bool canUpgradeCapacity = _dockUpgrade.dockUpgradeData.capacityLevel < maxLevelCapacity;
			capacityButton.interactable = canUpgradeCapacity;
		}
	}
}

