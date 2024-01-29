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
		[Header("Buttons")]
		public Button shackCloseButton;
		public Button dockWorkerButton;
		public Button speedButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI dockWorkerLevelText;
		public TextMeshProUGUI speedLevelText;
		public TextMeshProUGUI capacityLevelText;

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

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
			GameManager.OnCloseButton?.Invoke();
		}

		public void OnDockWorkerButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeDockWorkerLevel();
		}

		public void OnSpeedButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeSpeedLevel();
		}

		public void OnCapacityButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeCapacityLevel();
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
	}
}

