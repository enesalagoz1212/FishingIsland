using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;
using FishingIsland.UpgradeScriptableObjects;
using TMPro;

namespace FishingIsland.Canvases
{
	public class HouseUpgradeCanvas : MonoBehaviour
	{
		[Header("Buttons")]
		public Button houseCloseButton;
		public Button fishWorkerButton;
		public Button speedButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI fishWorkerLevelText;
		public TextMeshProUGUI speedLevelText;
		public TextMeshProUGUI capacityLevelText;

		public void Initialize()
		{
			houseCloseButton.onClick.AddListener(OnCloseButtonClick);
			fishWorkerButton.onClick.AddListener(OnDockWorkerButtonClick);
			speedButton.onClick.AddListener(OnSpeedButtonClick);
			capacityButton.onClick.AddListener(OnCapacityButtonClick);

			HouseUpgradeManager.OnHouseUpgradeFishWorkerLevelUpdated += HouseUpgradeUpdateFishWorkerLevelText;
			HouseUpgradeManager.OnHouseUpgradeSpeedLevelUpdated += HouseUpgradeUpdateSpeedLevelText;
			HouseUpgradeManager.OnHouseUpgradeCapacityLevelUpdated += HouseUpgradeUpdateCapacityLevelText;

			HouseUpgradeData savedData = SaveLoadManager.Instance.LoadHouseUpgradeData();
			HouseUpgradeManager.Instance.houseUpgradeData = savedData;
			HouseUpgradeManager.Instance.UpdateUpgradeCosts();

			HouseUpgradeUpdateFishWorkerLevelText(HouseUpgradeManager.Instance.GetFishWorkerLevel());
			HouseUpgradeUpdateSpeedLevelText(HouseUpgradeManager.Instance.GetSpeedLevel());
			HouseUpgradeUpdateCapacityLevelText(HouseUpgradeManager.Instance.GetCapacityLevel());
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
			GameManager.OnCloseButton?.Invoke();
		}

		public void OnDockWorkerButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeFishWorkerLevel();
		}

		public void OnSpeedButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeSpeedLevel();
		}

		public void OnCapacityButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeCapacityLevel();
		}

		private void HouseUpgradeUpdateFishWorkerLevelText(int newFishWorkerLevel)
		{
			fishWorkerLevelText.text = $" {newFishWorkerLevel}";
		}

		private void HouseUpgradeUpdateSpeedLevelText(int newTimerLevel)
		{
			speedLevelText.text = $" {newTimerLevel}";
		}

		private void HouseUpgradeUpdateCapacityLevelText(int newCapacityLevel)
		{
			capacityLevelText.text = $" {newCapacityLevel}";
		}
	}
}

