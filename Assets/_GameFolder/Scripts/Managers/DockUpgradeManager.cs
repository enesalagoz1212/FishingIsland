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
		public static DockUpgradeManager Instance { get;  set; }

		public DockUpgrade dockUpgrade;

		public static Action<int> OnBoatLevelUpdated;
		public static Action<int> OnTimerLevelUpdated;
		public static Action<int> OnCapacityLevelUpdated;

		public TextMeshProUGUI boatLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI timerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;

		public void Initialize()
		{
			BoatLevelIncreaseMoneyText(dockUpgrade.boatLevelUpgradeCost);
			TimerLevelIncreaseMoneyText(dockUpgrade.timerLevelUpgradeCost);
			CapacityLevelIncreaseMoneyText(dockUpgrade.capacityLevelUpgradeCost);
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

		public void UpgradeBoatLevel()
		{
			if (MoneyManager.Instance != null)
			{
				if (MoneyManager.Instance.money >= dockUpgrade.boatLevelUpgradeCost)
				{
					MoneyManager.Instance.money -= dockUpgrade.boatLevelUpgradeCost;
					dockUpgrade.boatLevel++;
					UpdateDockUpgradeLevelCost();

					OnBoatLevelUpdated?.Invoke(dockUpgrade.boatLevel);
					BoatLevelIncreaseMoneyText(dockUpgrade.boatLevelUpgradeCost);

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
		public void UpgradeTimerLevel()
		{
			if (MoneyManager.Instance.money >= dockUpgrade.timerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= dockUpgrade.timerLevelUpgradeCost;
				dockUpgrade.timerLevel++;

				if (dockUpgrade.timerLevel % 5 == 0)
				{
					dockUpgrade.initialTimerDurationBoat = Mathf.Max(dockUpgrade.minTimerDurationBoat, dockUpgrade.initialTimerDurationBoat - 1.0f);
				}

				UpdateDockUpgradeTimerCost();

				OnTimerLevelUpdated?.Invoke(dockUpgrade.timerLevel);
				TimerLevelIncreaseMoneyText(dockUpgrade.timerLevelUpgradeCost);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeCapacityLevel()
		{
			if (MoneyManager.Instance.money >= dockUpgrade.capacityLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= dockUpgrade.capacityLevelUpgradeCost;
				dockUpgrade.capacityLevel++;

				dockUpgrade.boatFishCapacity += dockUpgrade.boatCapacityIncrease;

				UpdateDockUpgradeCapacityCost();
				OnCapacityLevelUpdated?.Invoke(dockUpgrade.capacityLevel);
				CapacityLevelIncreaseMoneyText(dockUpgrade.capacityLevelUpgradeCost);

				MoneyManager.Instance.UpdateMoneyText();
			}

			else
			{
				Debug.Log("insufficient money");
			}
		}

		private void UpdateDockUpgradeLevelCost()
		{
			dockUpgrade.boatLevelUpgradeCost = dockUpgrade.boatLevel * 15;
		}

		private void UpdateDockUpgradeTimerCost()
		{
			dockUpgrade.timerLevelUpgradeCost = dockUpgrade.timerLevel * 25;
		}

		private void UpdateDockUpgradeCapacityCost()
		{
			dockUpgrade.capacityLevelUpgradeCost = dockUpgrade.capacityLevel * 30;
		}

		
		public int GetBoatLevel()
		{
			return dockUpgrade.boatLevel;
		}

	
		public int GetTimerLevel()
		{
			return dockUpgrade.timerLevel;
		}


		public int GetCapacityLevel()
		{
			return dockUpgrade.capacityLevel;
		}

		public void BoatLevelIncreaseMoneyText(int moneyText)
		{
			boatLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void TimerLevelIncreaseMoneyText(int moneyText)
		{
			timerLevelIncreaseMoneyAmountText.text = $"{moneyText}";
		}
		public void CapacityLevelIncreaseMoneyText(int moneyText)
		{
			capacityLevelIncreaseMoneyAmountText.text = $"{moneyText}";
		}

		public DockUpgrade GetDockUpgrade()
		{
			return dockUpgrade;
		}
	}

}
