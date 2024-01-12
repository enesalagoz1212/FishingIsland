using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;
using TMPro;

namespace FishingIsland.Canvases
{
	public class HouseUpgradeCanvas : MonoBehaviour
	{
		public Button houseCloseButton;
		public Button fishWorkerButton;
		public Button timerButton;
		public Button capacityButton;

		public TextMeshProUGUI fishWorkerLevelText;
		public TextMeshProUGUI timerLevelText;
		public TextMeshProUGUI capacityLevelText;

		public void Initialize()
		{
			houseCloseButton.onClick.AddListener(OnCloseButtonClick);
			fishWorkerButton.onClick.AddListener(OnDockWorkerButtonClick);
			timerButton.onClick.AddListener(OnTimerButtonClick);
			capacityButton.onClick.AddListener(OnCapacityButtonClick);

			HouseUpgradeManager.OnHouseUpgradeFishWorkerLevelUpdated += HouseUpgradeUpdateFishWorkerLevelText;
			HouseUpgradeManager.OnHouseUpgradeTimerLevelUpdated += HouseUpgradeUpdateTimerLevelText;
			HouseUpgradeManager.OnHouseUpgradeCapacityLevelUpdated += HouseUpgradeUpdateCapacityLevelText;

			HouseUpgradeUpdateFishWorkerLevelText(HouseUpgradeManager.Instance.GetFishWorkerLevel());
			HouseUpgradeUpdateTimerLevelText(HouseUpgradeManager.Instance.GetTimerLevel());
			HouseUpgradeUpdateCapacityLevelText(HouseUpgradeManager.Instance.GetCapacityLevel());
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
		}

		public void OnDockWorkerButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeFishWorkerLevel();
		}

		public void OnTimerButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeTimerLevel();
		}

		public void OnCapacityButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeCapacityLevel();
		}

		private void HouseUpgradeUpdateFishWorkerLevelText(int newFishWorkerLevel)
		{
			fishWorkerLevelText.text = $" {newFishWorkerLevel}";
		}

		private void HouseUpgradeUpdateTimerLevelText(int newTimerLevel)
		{

			timerLevelText.text = $" {newTimerLevel}";
		}

		private void HouseUpgradeUpdateCapacityLevelText(int newCapacityLevel)
		{
			capacityLevelText.text = $" {newCapacityLevel}";
		}

	}
}

