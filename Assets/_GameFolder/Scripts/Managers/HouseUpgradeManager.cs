using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using TMPro;
using System;


namespace FishingIsland.Managers
{
    public class HouseUpgradeManager : MonoBehaviour
    {
        public static HouseUpgradeManager Instance { get; private set; }
        public HouseUpgrade houseUpgrade;
		public HouseUpgradeData houseUpgradeData { get; set; }

		public static Action<int> OnHouseUpgradeFishWorkerLevelUpdated;
		public static Action<int> OnHouseUpgradeTimerLevelUpdated;
		public static Action<int> OnHouseUpgradeCapacityLevelUpdated;

		public TextMeshProUGUI fishWorkerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI timerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;

		private int _fishWorkerUpgradeCost;
		private int _timerUpgradeCost;
		private int _capacityUpgradeCost;

		public void Initialize()
		{
			UpdateUpgradeCosts();

			FishWorkerLevelIncreaseMoneyText(_fishWorkerUpgradeCost);
			TimerLevelIncreaseMoneyText(_timerUpgradeCost);
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
			_fishWorkerUpgradeCost = houseUpgrade.UpdateHouseUpgradeFishWorkerLevelCost(houseUpgradeData.fishWorkerLevel);
			_timerUpgradeCost = houseUpgrade.UpdateHouseUpgradeTimerLevelCost(houseUpgradeData.timerLevel);
			_capacityUpgradeCost = houseUpgrade.UpdateHouseUpgradeCapacityLevelCost(houseUpgradeData.capacityLevel);
		}

		public void UpgradeFishWorkerLevel()
		{
			if (MoneyManager.Instance.money >= _fishWorkerUpgradeCost)
			{
				MoneyManager.Instance.money -= _fishWorkerUpgradeCost;
				houseUpgradeData.fishWorkerLevel++;

				int fishWorkerLevelCost = houseUpgrade.UpdateHouseUpgradeFishWorkerLevelCost(houseUpgradeData.fishWorkerLevel);
				OnHouseUpgradeFishWorkerLevelUpdated?.Invoke(houseUpgradeData.fishWorkerLevel);

				FishWorkerLevelIncreaseMoneyText(fishWorkerLevelCost);
				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveHouseUpgradeData(houseUpgradeData);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeTimerLevel()
		{
			if (MoneyManager.Instance.money >= _timerUpgradeCost)
			{
				MoneyManager.Instance.money -= _timerUpgradeCost;
				houseUpgradeData.timerLevel++;

				int timerLevelCost = houseUpgrade.UpdateHouseUpgradeTimerLevelCost(houseUpgradeData.timerLevel);

				OnHouseUpgradeTimerLevelUpdated?.Invoke(houseUpgradeData.timerLevel);

				TimerLevelIncreaseMoneyText(timerLevelCost);

				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveHouseUpgradeData(houseUpgradeData);
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
				houseUpgradeData.capacityLevel++;

				int capacityLevelCost = houseUpgrade.UpdateHouseUpgradeCapacityLevelCost(houseUpgradeData.capacityLevel);

				OnHouseUpgradeCapacityLevelUpdated?.Invoke(houseUpgradeData.capacityLevel);
			
				CapacityLevelIncreaseMoneyText(capacityLevelCost);

				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveHouseUpgradeData(houseUpgradeData);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public int GetFishWorkerLevel()
		{
			return houseUpgradeData.fishWorkerLevel;
		}

		public int GetTimerLevel()
		{
			return houseUpgradeData.timerLevel;
		}

		public int GetCapacityLevel()
		{
			return houseUpgradeData.capacityLevel;
		}

		public void FishWorkerLevelIncreaseMoneyText(int moneyText)
		{
			fishWorkerLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void TimerLevelIncreaseMoneyText(int moneyText)
		{
			timerLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void CapacityLevelIncreaseMoneyText(int moneyText)
		{
			capacityLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public HouseUpgrade GetHouseUpgrade()
		{
			return houseUpgrade;
		}

		public void ResetGame()
		{
			houseUpgrade.ResetGameHouseUpgrade();

			houseUpgradeData = houseUpgrade.houseUpgradeData;


			OnHouseUpgradeFishWorkerLevelUpdated?.Invoke(houseUpgradeData.fishWorkerLevel);
			OnHouseUpgradeTimerLevelUpdated?.Invoke(houseUpgradeData.timerLevel);
			OnHouseUpgradeCapacityLevelUpdated?.Invoke(houseUpgradeData.capacityLevel);

			MoneyManager.Instance.money = MoneyManager.Instance.startingMoney;
			MoneyManager.Instance.UpdateMoneyText();

			UpdateUpgradeCosts();

			FishWorkerLevelIncreaseMoneyText(_fishWorkerUpgradeCost);
			TimerLevelIncreaseMoneyText(_timerUpgradeCost);
			CapacityLevelIncreaseMoneyText(_capacityUpgradeCost);
		}
	}
}
