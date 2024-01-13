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
				houseUpgrade.fishWorkerLevel++;
				UpdateHouseUpgradeFishWorkerCost();

				OnHouseUpgradeFishWorkerLevelUpdated?.Invoke(houseUpgrade.fishWorkerLevel);

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
				houseUpgrade.timerLevel++;
				UpdateHouseUpgradeTimerCost();

				OnHouseUpgradeTimerLevelUpdated?.Invoke(houseUpgrade.timerLevel);

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
				houseUpgrade.capacityLevel++;
				houseUpgrade.fishWorkerFishCapacity += houseUpgrade.fishWorkerCapacityIncrease;

				UpdateHouseUpgradeCapacityCost();			
				OnHouseUpgradeCapacityLevelUpdated?.Invoke(houseUpgrade.capacityLevel);
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
			houseUpgrade.fishWorkerLevelUpgradeCost = houseUpgrade.fishWorkerLevel * 15;
		}


		private void UpdateHouseUpgradeTimerCost()
		{
			houseUpgrade.timerLevelUpgradeCost = houseUpgrade.timerLevel * 25;
		}

		private void UpdateHouseUpgradeCapacityCost()
		{
			houseUpgrade.capacityLevelUpgradeCost = houseUpgrade.capacityLevel * 30;
		}

		public int GetFishWorkerLevel()
		{
			return houseUpgrade.fishWorkerLevel;
		}


		public int GetTimerLevel()
		{
			return houseUpgrade.timerLevel;
		}


		public int GetCapacityLevel()
		{
			return houseUpgrade.capacityLevel;
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
