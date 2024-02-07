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
		private HouseUpgrade _houseUpgrade;

		[Header("Buttons")]
		public Button houseCloseButton;
		public Button fishWorkerButton;
		public Button speedButton;
		public Button capacityButton;

		[Header("Texts")]
		public TextMeshProUGUI fishWorkerLevelText;
		public TextMeshProUGUI speedLevelText;
		public TextMeshProUGUI capacityLevelText;

		private bool _canFishWorkerButton = true;
		private bool _canSpeedButton = true;
		private bool _canCapacityButton = true;

		public void Initialize()
		{
			houseCloseButton.onClick.AddListener(OnCloseButtonClick);
			fishWorkerButton.onClick.AddListener(OnFishWorkerButtonClick);
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

		private void OnEnable()
		{
			GameManager.OnButtonClickedHouseUpgrade += OnButtonClickedHouseUpgradeAction;
			GameManager.OnGameReset += OnGameResetAction;
		}

		private void OnDisable()
		{
			GameManager.OnButtonClickedHouseUpgrade -= OnButtonClickedHouseUpgradeAction;
			GameManager.OnGameReset -= OnGameResetAction;

		}
		private void Start()
		{
			_houseUpgrade = HouseUpgradeManager.Instance.houseUpgrade;
			UpdateButtonInteractivityFishWorkerButton();
			UpdateButtonInteractivitySpeedButton();
			UpdateButtonInteractivityCapacityButton();
		}

		private void OnButtonClickedHouseUpgradeAction()
		{
			int fishWorkerLevel = _houseUpgrade.houseUpgradeData.fishWorkerLevel;
			int speedLevel = _houseUpgrade.houseUpgradeData.speedLevel;
			int capacityLevel = _houseUpgrade.houseUpgradeData.capacityLevel;

			if (fishWorkerLevel == 10 && _canFishWorkerButton)
			{
				fishWorkerButton.interactable = false;
				_canFishWorkerButton = false;
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

			if (fishWorkerLevel == 10 && speedLevel == 10 && capacityLevel == 10)
			{
				fishWorkerButton.interactable = true;
				speedButton.interactable = true;
				capacityButton.interactable = true;		
			}
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
			GameManager.OnCloseButton?.Invoke();
		}

		public void OnFishWorkerButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeFishWorkerLevel();
			UpdateButtonInteractivityFishWorkerButton();
			GameManager.OnButtonClickedHouseUpgrade?.Invoke();
		}

		public void OnSpeedButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeSpeedLevel();
			UpdateButtonInteractivitySpeedButton();
			GameManager.OnButtonClickedHouseUpgrade?.Invoke();
		}

		public void OnCapacityButtonClick()
		{
			HouseUpgradeManager.Instance.UpgradeCapacityLevel();
			UpdateButtonInteractivityCapacityButton();
			GameManager.OnButtonClickedHouseUpgrade?.Invoke();
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

		public void UpdateButtonInteractivityFishWorkerButton()
		{
			int maxLevelFishWorker = _houseUpgrade.MaxLevelHouse();

			bool canUpgradeBoat = _houseUpgrade.houseUpgradeData.fishWorkerLevel < maxLevelFishWorker;
			fishWorkerButton.interactable = canUpgradeBoat;
		}

		public void UpdateButtonInteractivitySpeedButton()
		{
			int maxLevelSpeed = _houseUpgrade.MaxLevelHouse();

			bool canUpgradeSpeed = _houseUpgrade.houseUpgradeData.speedLevel < maxLevelSpeed;
			speedButton.interactable = canUpgradeSpeed;
		}

		public void UpdateButtonInteractivityCapacityButton()
		{
			int maxLevelCapacity = _houseUpgrade.MaxLevelHouse();

			bool canUpgradeCapacity = _houseUpgrade.houseUpgradeData.capacityLevel < maxLevelCapacity;
			capacityButton.interactable = canUpgradeCapacity;
		}

		private void OnGameResetAction()
		{
			fishWorkerButton.interactable = true;
			speedButton.interactable = true;
			capacityButton.interactable = true;
		}
	}
}

