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
			if (MoneyManager.Instance.money >= shackUpgrade.timerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= shackUpgrade.timerLevelUpgradeCost;
				shackUpgrade.timerLevel++;
				UpdateUpgradeDockWorkerCost();

				OnShackUpgradeDockWorkerLevelUpdated?.Invoke(shackUpgrade.timerLevel);

				DockWorkerLevelIncreaseMoneyText(shackUpgrade.timerLevelUpgradeCost);

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
			if (MoneyManager.Instance.money >= shackUpgrade.timerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= shackUpgrade.timerLevelUpgradeCost;
				shackUpgrade.timerLevel++;
				UpdateUpgradeCapacityCost();

				OnShackUpgradeCapacityLevelUpdated?.Invoke(shackUpgrade.timerLevel);

				CapacityLevelIncreaseMoneyText(shackUpgrade.timerLevelUpgradeCost);

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

