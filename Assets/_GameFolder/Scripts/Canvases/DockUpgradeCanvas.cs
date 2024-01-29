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
			UpdateSpeedLevelText(DockUpgradeManager.Instance.GetTimerLevel());
			UpdateCapacityLevelText(DockUpgradeManager.Instance.GetCapacityLevel());
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
			GameManager.OnCloseButton?.Invoke();
		}

		public void OnBoatButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeBoatLevel();
		}

		public void OnSpeedButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeSpeedLevel();
		}

		public void OnCapacityButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeCapacityLevel();
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
	}
}

