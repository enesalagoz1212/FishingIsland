using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using System;

namespace FishingIsland.Managers
{
	public class DockUpgradeManager : MonoBehaviour
	{
		public static DockUpgradeManager Instance { get; private set; }

		[SerializeField] private DockUpgrade dockUpgrade;

		public static Action<int> OnBoatLevelUpdated;
		public static Action<int> OnTimerLevelUpdated;
		public static Action<int> OnCapacityLevelUpdated;

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
					UpdateUpgradeCost();

					OnBoatLevelUpdated?.Invoke(dockUpgrade.boatLevel);
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
				UpdateUpgradeTimerCost();

				OnTimerLevelUpdated?.Invoke(dockUpgrade.timerLevel);
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
				UpdateUpgradeCapacityCost();

				OnCapacityLevelUpdated?.Invoke(dockUpgrade.capacityLevel);
			}

			else
			{
				Debug.Log("insufficient money");
			}
		}

		private void UpdateUpgradeCost()
		{
			dockUpgrade.boatLevelUpgradeCost = dockUpgrade.boatLevel * 2;
		}

		private void UpdateUpgradeTimerCost()
		{
			dockUpgrade.timerLevelUpgradeCost = dockUpgrade.timerLevel * 3;
		}

		private void UpdateUpgradeCapacityCost()
		{
			dockUpgrade.capacityLevelUpgradeCost = dockUpgrade.capacityLevel * 4;
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
	}

}
