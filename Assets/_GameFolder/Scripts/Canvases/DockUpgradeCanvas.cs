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

		private bool _canBoatButton = true;
		private bool _canSpeedButton = true;
		private bool _canCapacityButton = true;
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

		private void OnEnable()
		{
			GameManager.OnButtonClickedDockUpgrade += OnButtonClickedDockUpgradeAction;
		}

		private void OnDisable()
		{

			GameManager.OnButtonClickedDockUpgrade -= OnButtonClickedDockUpgradeAction;
		}

		private void OnButtonClickedDockUpgradeAction()
		{
			Debug.Log("Action oldu");
			int boatLevel = _dockUpgrade.dockUpgradeData.boatLevel;
			int speedLevel = _dockUpgrade.dockUpgradeData.speedLevel;
			int capacityLevel = _dockUpgrade.dockUpgradeData.capacityLevel;

			if (boatLevel == 10 && _canBoatButton)
			{
				boatButton.interactable = false;
			}

			if (speedLevel == 10 && _canSpeedButton)
			{
				speedButton.interactable = false;
			}

			if (capacityLevel == 10 && _canCapacityButton)
			{
				capacityButton.interactable = false;
			}

			if (boatLevel == 10 && speedLevel == 10 && capacityLevel == 10)
			{
				boatButton.interactable = true;
				speedButton.interactable = true;
				capacityButton.interactable = true;

				_canBoatButton = false;
				_canSpeedButton = false;
				_canCapacityButton = false;
			}
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
			_canBoatButton = true;
			DockUpgradeManager.Instance.UpgradeBoatLevel();
			UpdateButtonInteractivityBoatButton();
			GameManager.OnButtonClickedDockUpgrade?.Invoke();
		}

		public void OnSpeedButtonClick()
		{
			_canSpeedButton = true;
			DockUpgradeManager.Instance.UpgradeSpeedLevel();
			UpdateButtonInteractivitySpeedButton();
			GameManager.OnButtonClickedDockUpgrade?.Invoke();
		}

		public void OnCapacityButtonClick()
		{
			_canCapacityButton = true;
			DockUpgradeManager.Instance.UpgradeCapacityLevel();
			UpdateButtonInteractivityCapacityButton();
			GameManager.OnButtonClickedDockUpgrade?.Invoke();
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

