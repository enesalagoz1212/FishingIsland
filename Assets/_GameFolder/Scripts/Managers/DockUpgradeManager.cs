using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;

namespace FishingIsland.Managers
{
	public class DockUpgradeManager : MonoBehaviour
	{
		public static DockUpgradeManager Instance { get; private set; }

		[SerializeField] private DockUpgrade dockUpgrade;


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
			if (MoneyManager.Instance.money >= dockUpgrade.boatLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= dockUpgrade.boatLevelUpgradeCost;
				dockUpgrade.boatLevel++;
				UpdateUpgradeCost();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeTimerLevel()
		{
			if (MoneyManager.Instance.money >= dockUpgrade.timerLevelUpgradeCost)
			{
				MoneyManager.Instance.money -= dockUpgrade.timerLevelUpgradeCost;
				dockUpgrade.timerLevel++;
				UpdateUpgradeCost();
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
				UpdateUpgradeCost();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		private void UpdateUpgradeCost()
		{
			dockUpgrade.boatLevelUpgradeCost = dockUpgrade.boatLevel * 50;
			dockUpgrade.timerLevelUpgradeCost = dockUpgrade.timerLevel * 30;
			dockUpgrade.capacityLevelUpgradeCost = dockUpgrade.capacityLevel * 40;
		}

	}

}
