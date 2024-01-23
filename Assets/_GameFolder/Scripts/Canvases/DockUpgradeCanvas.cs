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
		public Button timerButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI boatLevelText;
		public TextMeshProUGUI timerLevelText;
		public TextMeshProUGUI capacityLevelText;

		public void Initialize()
		{
			dockCloseButton.onClick.AddListener(OnCloseButtonClick);
			boatButton.onClick.AddListener(OnBoatButtonClick);
			timerButton.onClick.AddListener(OnTimerButtonClick);
			capacityButton.onClick.AddListener(OnCapacityButtonClick);

			DockUpgradeManager.OnBoatLevelUpdated += UpdateBoatLevelText;
			DockUpgradeManager.OnTimerLevelUpdated += UpdateTimerLevelText;
			DockUpgradeManager.OnCapacityLevelUpdated += UpdateCapacityLevelText;

			DockUpgradeData savedData = SaveLoadManager.Instance.LoadDockUpgradeData();
			DockUpgradeManager.Instance.dockUpgradeData = savedData;
			DockUpgradeManager.Instance.UpdateUpgradeCosts();

			UpdateBoatLevelText(DockUpgradeManager.Instance.GetBoatLevel());
			UpdateTimerLevelText(DockUpgradeManager.Instance.GetTimerLevel());
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

		public void OnTimerButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeTimerLevel();
		}

		public void OnCapacityButtonClick()
		{
			DockUpgradeManager.Instance.UpgradeCapacityLevel();
		}

		private void UpdateBoatLevelText(int newBoatLevel)
		{
			boatLevelText.text = $" {newBoatLevel}";
		}

		private void UpdateTimerLevelText(int newTimerLevel)
		{
			timerLevelText.text = $" {newTimerLevel}";
		}

		private void UpdateCapacityLevelText(int newCapacityLevel)
		{
			capacityLevelText.text = $" {newCapacityLevel}";
		}
	}
}

