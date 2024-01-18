using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FishingIsland.Managers;
using FishingIsland.Controllers;

namespace FishingIsland.Canvases
{
    public class ShackUpgradeCanvas : MonoBehaviour
    {
		[Header("Buttons")]
		public Button shackCloseButton;
		public Button dockWorkerButton;
		public Button timerButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI dockWorkerLevelText;
		public TextMeshProUGUI timerLevelText;
		public TextMeshProUGUI capacityLevelText;

		public void Initialize()
		{
			shackCloseButton.onClick.AddListener(OnCloseButtonClick);
			dockWorkerButton.onClick.AddListener(OnDockWorkerButtonClick);
			timerButton.onClick.AddListener(OnTimerButtonClick);
			capacityButton.onClick.AddListener(OnCapacityButtonClick);

			ShackUpgradeManager.OnShackUpgradeDockWorkerLevelUpdated += ShackUpgradeUpdateDockWorkerLevelText;
			ShackUpgradeManager.OnShackUpgradeTimerLevelUpdated += ShackUpgradeUpdateTimerLevelText;
			ShackUpgradeManager.OnShackUpgradeCapacityLevelUpdated += ShackUpgradeUpdateCapacityLevelText;

			ShackUpgradeUpdateDockWorkerLevelText(ShackUpgradeManager.Instance.GetDockWorkerLevel());
			ShackUpgradeUpdateTimerLevelText(ShackUpgradeManager.Instance.GetTimerLevel());
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

		public void OnTimerButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeTimerLevel();
		}

		public void OnCapacityButtonClick()
		{
			ShackUpgradeManager.Instance.UpgradeCapacityLevel();
		}

		private void ShackUpgradeUpdateDockWorkerLevelText(int newDockWorkerLevel)
		{
			dockWorkerLevelText.text = $" {newDockWorkerLevel}";
		}

		private void ShackUpgradeUpdateTimerLevelText(int newTimerLevel)
		{
			timerLevelText.text = $" {newTimerLevel}";
		}

		private void ShackUpgradeUpdateCapacityLevelText(int newCapacityLevel)
		{
			capacityLevelText.text = $" {newCapacityLevel}";
		}
	}
}

