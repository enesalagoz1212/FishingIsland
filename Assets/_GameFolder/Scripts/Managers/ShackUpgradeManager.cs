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
		public ShackUpgradeData shackUpgradeData;

		public static Action<int> OnShackUpgradeDockWorkerLevelUpdated;
		public static Action<int> OnShackUpgradeTimerLevelUpdated;
		public static Action<int> OnShackUpgradeCapacityLevelUpdated;

		public TextMeshProUGUI dockWorkerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI timerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;

		public void Initialize()
		{
			DockWorkerLevelIncreaseMoneyText(shackUpgrade.dockWorkerLevelUpgradeCost);
			TimerLevelIncreaseMoneyText(shackUpgrade.timerLevelUpgradeCost);
			CapacityLevelIncreaseMoneyText(shackUpgrade.capacityLevelUpgradeCost);
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

		public void UpgradeDockWorkerLevel()
		{
			if (MoneyManager.Instance.money >= shackUpgrade.dockWorkerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= shackUpgrade.dockWorkerLevelUpgradeCost;
				shackUpgradeData.dockWorkerLevel++;
				UpdateUpgradeDockWorkerCost();

				OnShackUpgradeDockWorkerLevelUpdated?.Invoke(shackUpgradeData.dockWorkerLevel);

				DockWorkerLevelIncreaseMoneyText(shackUpgrade.dockWorkerLevelUpgradeCost);

				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeTimerLevel()
		{
			if (MoneyManager.Instance.money >= shackUpgrade.timerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= shackUpgrade.timerLevelUpgradeCost;
				shackUpgradeData.timerLevel++;

				if (shackUpgradeData.timerLevel % 5 == 0)
				{
					shackUpgrade.initialTimerDurationFishWorker = Mathf.Max(shackUpgrade.minTimerDurationFishWorker, shackUpgrade.initialTimerDurationFishWorker - 1.0f);
				}

				UpdateUpgradeTimerCost();

				OnShackUpgradeTimerLevelUpdated?.Invoke(shackUpgradeData.timerLevel);

				TimerLevelIncreaseMoneyText(shackUpgrade.timerLevelUpgradeCost);

				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeCapacityLevel()
		{
			if (MoneyManager.Instance.money >= shackUpgrade.capacityLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= shackUpgrade.capacityLevelUpgradeCost;
				shackUpgradeData.capacityLevel++;
				shackUpgrade.dockWorkerFishCapacity += shackUpgrade.dockWorkerCapacityIncrease;

				UpdateUpgradeCapacityCost();
				OnShackUpgradeCapacityLevelUpdated?.Invoke(shackUpgradeData.capacityLevel);
				CapacityLevelIncreaseMoneyText(shackUpgrade.capacityLevelUpgradeCost);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		private void UpdateUpgradeDockWorkerCost()
		{
			shackUpgrade.dockWorkerLevelUpgradeCost = shackUpgradeData.dockWorkerLevel * 15;
		}


		private void UpdateUpgradeTimerCost()
		{
			shackUpgrade.timerLevelUpgradeCost = shackUpgradeData.timerLevel * 25;
		}

		private void UpdateUpgradeCapacityCost()
		{
			shackUpgrade.capacityLevelUpgradeCost = shackUpgradeData.capacityLevel * 30;
		}

		public int GetDockWorkerLevel()
		{
			return shackUpgradeData.dockWorkerLevel;
		}

		public int GetTimerLevel()
		{
			return shackUpgradeData.timerLevel;
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

		public void TimerLevelIncreaseMoneyText(int moneyText)
		{
			timerLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void CapacityLevelIncreaseMoneyText(int moneyText)
		{
			capacityLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}
	}
}

