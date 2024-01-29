using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using TMPro;
using System;


namespace FishingIsland.Managers
{
	public class ShackUpgradeManager : MonoBehaviour
	{
		public static ShackUpgradeManager Instance { get; set; }

		public ShackUpgrade shackUpgrade;
		public ShackUpgradeData shackUpgradeData { get; set; }

		public static Action<int> OnShackUpgradeDockWorkerLevelUpdated;
		public static Action<int> OnShackUpgradeSpeedLevelUpdated;
		public static Action<int> OnShackUpgradeCapacityLevelUpdated;

		public TextMeshProUGUI dockWorkerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI speedLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;

		private int _dockWorkerUpgradeCost;
		private int _speedUpgradeCost;
		private int _capacityUpgradeCost;

		public void Initialize()
		{
			UpdateUpgradeCosts();

			DockWorkerLevelIncreaseMoneyText(_dockWorkerUpgradeCost);
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
			_dockWorkerUpgradeCost = shackUpgrade.UpdateShackUpgradeDockWorkerLevelCost(shackUpgradeData.dockWorkerLevel);
			_speedUpgradeCost = shackUpgrade.UpdateShackUpgradeSpeedLevelCost(shackUpgradeData.speedLevel);
			_capacityUpgradeCost = shackUpgrade.UpdateShackUpgradeCapacityLevelCost(shackUpgradeData.capacityLevel);
		}

		public void UpgradeDockWorkerLevel()
		{
			if (MoneyManager.Instance.money >= _dockWorkerUpgradeCost)
			{
				MoneyManager.Instance.money -= _dockWorkerUpgradeCost;
				shackUpgradeData.dockWorkerLevel++;

				int dockWorkerLevelCost = shackUpgrade.UpdateShackUpgradeDockWorkerLevelCost(shackUpgradeData.dockWorkerLevel);

				OnShackUpgradeDockWorkerLevelUpdated?.Invoke(shackUpgradeData.dockWorkerLevel);

				DockWorkerLevelIncreaseMoneyText(dockWorkerLevelCost);


				UpdateUpgradeCosts();
				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveShackUpgradeData(shackUpgradeData);

				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeSpeedLevel()
		{
			if (MoneyManager.Instance.money >= _speedUpgradeCost)
			{
				MoneyManager.Instance.money -= _speedUpgradeCost;
				shackUpgradeData.speedLevel++;

				int timerLevelCost = shackUpgrade.UpdateShackUpgradeSpeedLevelCost(shackUpgradeData.speedLevel);

				OnShackUpgradeSpeedLevelUpdated?.Invoke(shackUpgradeData.speedLevel);

				SpeedLevelIncreaseMoneyText(timerLevelCost);

				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveShackUpgradeData(shackUpgradeData);

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
				shackUpgradeData.capacityLevel++;

				int capacityLevelCost = shackUpgrade.UpdateShackUpgradeCapacityLevelCost(shackUpgradeData.capacityLevel);
		

				OnShackUpgradeCapacityLevelUpdated?.Invoke(shackUpgradeData.capacityLevel);

				CapacityLevelIncreaseMoneyText(capacityLevelCost);

				UpdateUpgradeCosts();
				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveShackUpgradeData(shackUpgradeData);

				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}


		public int GetDockWorkerLevel()
		{
			return shackUpgradeData.dockWorkerLevel;
		}

		public int GetSpeedLevel()
		{
			return shackUpgradeData.speedLevel;
		}

		public int GetCapacityLevel()
		{
			return shackUpgradeData.capacityLevel;
		}

		public ShackUpgrade GetShackUpgrade()
		{
			return shackUpgrade;
		}

		public void DockWorkerLevelIncreaseMoneyText(int moneyText)
		{
			dockWorkerLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void SpeedLevelIncreaseMoneyText(int moneyText)
		{
			speedLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void CapacityLevelIncreaseMoneyText(int moneyText)
		{
			capacityLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void ResetGame()
		{
			shackUpgrade.ResetGameShackUpgrade();
			shackUpgradeData = shackUpgrade.shackUpgradeData;


			OnShackUpgradeDockWorkerLevelUpdated?.Invoke(shackUpgradeData.dockWorkerLevel);
			OnShackUpgradeSpeedLevelUpdated?.Invoke(shackUpgradeData.speedLevel);
			OnShackUpgradeCapacityLevelUpdated?.Invoke(shackUpgradeData.capacityLevel);

			MoneyManager.Instance.money = MoneyManager.Instance.startingMoney;
			MoneyManager.Instance.UpdateMoneyText();

			UpdateUpgradeCosts();
			DockWorkerLevelIncreaseMoneyText(_dockWorkerUpgradeCost);
			SpeedLevelIncreaseMoneyText(_speedUpgradeCost);
			CapacityLevelIncreaseMoneyText(_capacityUpgradeCost);
		}
	}
}

