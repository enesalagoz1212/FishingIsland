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
				shackUpgrade.dockWorkerLevel++;
				UpdateUpgradeDockWorkerCost();

				OnShackUpgradeDockWorkerLevelUpdated?.Invoke(shackUpgrade.dockWorkerLevel);

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
				shackUpgrade.timerLevel++;

				if (shackUpgrade.timerLevel % 5 == 0)
				{
					shackUpgrade.initialTimerDurationFishWorker = Mathf.Max(shackUpgrade.minTimerDurationFishWorker, shackUpgrade.initialTimerDurationFishWorker - 1.0f);
				}

				UpdateUpgradeTimerCost();

				OnShackUpgradeTimerLevelUpdated?.Invoke(shackUpgrade.timerLevel);

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
				shackUpgrade.capacityLevel++;
				shackUpgrade.dockWorkerFishCapacity += shackUpgrade.dockWorkerCapacityIncrease;

				UpdateUpgradeCapacityCost();
				OnShackUpgradeCapacityLevelUpdated?.Invoke(shackUpgrade.capacityLevel);
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
			shackUpgrade.dockWorkerLevelUpgradeCost = shackUpgrade.dockWorkerLevel * 15;
		}


		private void UpdateUpgradeTimerCost()
		{
			shackUpgrade.timerLevelUpgradeCost = shackUpgrade.timerLevel * 25;
		}

		private void UpdateUpgradeCapacityCost()
		{
			shackUpgrade.capacityLevelUpgradeCost = shackUpgrade.capacityLevel * 30;
		}

		public int GetDockWorkerLevel()
		{
			return shackUpgrade.dockWorkerLevel;
		}


		public int GetTimerLevel()
		{
			return shackUpgrade.timerLevel;
		}


		public int GetCapacityLevel()
		{
			return shackUpgrade.capacityLevel;
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

