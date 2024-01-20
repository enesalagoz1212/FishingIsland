using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using TMPro;
using System;
using FishingIsland.UpgradeDatas;

namespace FishingIsland.Managers
{
    public class HouseUpgradeManager : MonoBehaviour
    {
        public static HouseUpgradeManager Instance { get; private set; }
        public HouseUpgrade houseUpgrade;
		public HouseUpgradeData houseUpgradeData;

		public static Action<int> OnHouseUpgradeFishWorkerLevelUpdated;
		public static Action<int> OnHouseUpgradeTimerLevelUpdated;
		public static Action<int> OnHouseUpgradeCapacityLevelUpdated;

		public TextMeshProUGUI fishWorkerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI timerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;


		public void Initialize()
		{
			FishWorkerLevelIncreaseMoneyText(houseUpgrade.fishWorkerLevelUpgradeCost);
			TimerLevelIncreaseMoneyText(houseUpgrade.timerLevelUpgradeCost);
			CapacityLevelIncreaseMoneyText(houseUpgrade.capacityLevelUpgradeCost);
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

		public void UpgradeFishWorkerLevel()
		{
			if (MoneyManager.Instance.money >= houseUpgrade.fishWorkerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= houseUpgrade.fishWorkerLevelUpgradeCost;
				houseUpgradeData.fishWorkerLevel++;
				UpdateHouseUpgradeFishWorkerCost();

				OnHouseUpgradeFishWorkerLevelUpdated?.Invoke(houseUpgradeData.fishWorkerLevel);

				FishWorkerLevelIncreaseMoneyText(houseUpgrade.fishWorkerLevelUpgradeCost);

				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeTimerLevel()
		{
			if (MoneyManager.Instance.money >= houseUpgrade.timerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= houseUpgrade.timerLevelUpgradeCost;
				houseUpgradeData.timerLevel++;

				if (houseUpgradeData.timerLevel % 5 == 0)
				{
					houseUpgrade.initialTimerDurationHouse = Mathf.Max(houseUpgrade.minTimerDurationHouse, houseUpgrade.initialTimerDurationHouse - 1.0f);
				}

				UpdateHouseUpgradeTimerCost();

				OnHouseUpgradeTimerLevelUpdated?.Invoke(houseUpgradeData.timerLevel);

				TimerLevelIncreaseMoneyText(houseUpgrade.timerLevelUpgradeCost);

				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeCapacityLevel()
		{
			if (MoneyManager.Instance.money >= houseUpgrade.capacityLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= houseUpgrade.capacityLevelUpgradeCost;
				houseUpgradeData.capacityLevel++;
				houseUpgrade.fishWorkerFishCapacity += houseUpgrade.fishWorkerCapacityIncrease;

				UpdateHouseUpgradeCapacityCost();			
				OnHouseUpgradeCapacityLevelUpdated?.Invoke(houseUpgradeData.capacityLevel);
				CapacityLevelIncreaseMoneyText(houseUpgrade.capacityLevelUpgradeCost);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		private void UpdateHouseUpgradeFishWorkerCost()
		{
			houseUpgrade.fishWorkerLevelUpgradeCost = houseUpgradeData.fishWorkerLevel * 15;
		}

		private void UpdateHouseUpgradeTimerCost()
		{
			houseUpgrade.timerLevelUpgradeCost = houseUpgradeData.timerLevel * 25;
		}

		private void UpdateHouseUpgradeCapacityCost()
		{
			houseUpgrade.capacityLevelUpgradeCost = houseUpgradeData.capacityLevel * 30;
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
	}
}
