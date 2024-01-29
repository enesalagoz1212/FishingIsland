using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using System;
using TMPro;

namespace FishingIsland.Managers
{
	public class DockUpgradeManager : MonoBehaviour
	{
		public static DockUpgradeManager Instance { get; set; }

		public DockUpgrade dockUpgrade;
		public DockUpgradeData dockUpgradeData { get; set; }

		public static Action<int> OnBoatLevelUpdated;
		public static Action<int> OnSpeedLevelUpdated;
		public static Action<int> OnCapacityLevelUpdated;

		public TextMeshProUGUI boatLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI speedLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;

		private int _boatUpgradeCost;
		private int _speedUpgradeCost;
		private int _capacityUpgradeCost;

		public void Initialize()
		{
			UpdateUpgradeCosts();

			BoatLevelIncreaseMoneyText(_boatUpgradeCost);
			SpeedLevelIncreaseMoneyText(_speedUpgradeCost);
			CapacityLevelIncreaseMoneyText(_capacityUpgradeCost);		
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		public void UpdateUpgradeCosts()
		{
			_boatUpgradeCost = dockUpgrade.UpdateDockUpgradeBoatLevelCost(dockUpgradeData.boatLevel);
			_speedUpgradeCost = dockUpgrade.UpdateDockUpgradeTimerLevelCost(dockUpgradeData.speedLevel);
			_capacityUpgradeCost = dockUpgrade.UpdateDockUpgradeCapacityLevelCost(dockUpgradeData.capacityLevel);
		}

		public void UpgradeBoatLevel()
		{
			if (MoneyManager.Instance != null)
			{
				if (MoneyManager.Instance.money >= _boatUpgradeCost)
				{
					MoneyManager.Instance.money -= _boatUpgradeCost;
					dockUpgradeData.boatLevel++;

					int boatLevelCost = dockUpgrade.UpdateDockUpgradeBoatLevelCost(dockUpgradeData.boatLevel);

					OnBoatLevelUpdated?.Invoke(dockUpgradeData.boatLevel);
					BoatLevelIncreaseMoneyText(boatLevelCost);

					UpdateUpgradeCosts();

					SaveLoadManager.Instance.SaveGame();
					SaveLoadManager.Instance.SaveDockUpgradeData(dockUpgradeData);
					MoneyManager.Instance.UpdateMoneyText();
				}
				else
				{
					Debug.Log("insufficient money");
				}
			}
			else
			{
				Debug.LogError("MoneyManager instance is missing!");
			}
		}

		public void UpgradeSpeedLevel()
		{
			if (MoneyManager.Instance.money >= _speedUpgradeCost)
			{
				MoneyManager.Instance.money -= _speedUpgradeCost;
				dockUpgradeData.speedLevel++;

				int speedLevelCost = dockUpgrade.UpdateDockUpgradeTimerLevelCost(dockUpgradeData.speedLevel);
	
				OnSpeedLevelUpdated?.Invoke(dockUpgradeData.speedLevel);
				SpeedLevelIncreaseMoneyText(speedLevelCost);

				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveDockUpgradeData(dockUpgradeData);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeCapacityLevel()
		{
			if (MoneyManager.Instance.money >= _capacityUpgradeCost)
			{
				MoneyManager.Instance.money -= _capacityUpgradeCost;
				dockUpgradeData.capacityLevel++;

				int capacityLevelCost = dockUpgrade.UpdateDockUpgradeCapacityLevelCost(dockUpgradeData.capacityLevel);

				OnCapacityLevelUpdated?.Invoke(dockUpgradeData.capacityLevel);
				CapacityLevelIncreaseMoneyText(capacityLevelCost);


				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveDockUpgradeData(dockUpgradeData);
				MoneyManager.Instance.UpdateMoneyText();
			}

			else
			{
				Debug.Log("insufficient money");
			}
		}



		public int GetBoatLevel()
		{
			return dockUpgradeData.boatLevel;
		}

		public int GetTimerLevel()
		{
			return dockUpgradeData.speedLevel;
		}

		public int GetCapacityLevel()
		{
			return dockUpgradeData.capacityLevel;
		}

		public void BoatLevelIncreaseMoneyText(int moneyText)
		{
			boatLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void SpeedLevelIncreaseMoneyText(int moneyText)
		{
			speedLevelIncreaseMoneyAmountText.text = $"{moneyText}";
		}

		public void CapacityLevelIncreaseMoneyText(int moneyText)
		{
			capacityLevelIncreaseMoneyAmountText.text = $"{moneyText}";
		}

		public DockUpgrade GetDockUpgrade()
		{
			return dockUpgrade;
		}

		public void ResetGame()
		{
			dockUpgrade.ResetGameDockUpgrade();

			dockUpgradeData = dockUpgrade.dockUpgradeData;


			OnBoatLevelUpdated?.Invoke(dockUpgradeData.boatLevel);
			OnSpeedLevelUpdated?.Invoke(dockUpgradeData.speedLevel);
			OnCapacityLevelUpdated?.Invoke(dockUpgradeData.capacityLevel);

			MoneyManager.Instance.money = MoneyManager.Instance.startingMoney;
			MoneyManager.Instance.UpdateMoneyText();

			UpdateUpgradeCosts();
			BoatLevelIncreaseMoneyText(_boatUpgradeCost);
			SpeedLevelIncreaseMoneyText(_speedUpgradeCost);
			CapacityLevelIncreaseMoneyText(_capacityUpgradeCost);
		}
	}
}
