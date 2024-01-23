using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishingIsland.Managers;

namespace FishingIsland.UpgradeScriptableObjects
{
	[Serializable]
	public class DockUpgradeData
	{
		public int boatLevel = 1;
		public int timerLevel = 1;
		public int capacityLevel = 1;

		public float currentTimerDurationBoat = 7;

	}

	[CreateAssetMenu(fileName = "New Dock Upgrade", menuName = "Dock Upgrade")]
	public class DockUpgrade : ScriptableObject
	{
		public DockUpgradeData dockUpgradeData => SaveLoadManager.Instance.saveData.dockUpgradeData;

		[SerializeField] private int boatCapacityIncrease;

		[SerializeField] private int boatLevelUpgradeCost;
		[SerializeField] private int timerLevelUpgradeCost;
		[SerializeField] private int capacityLevelUpgradeCost;


		[SerializeField] private int boatFishCapacity;

		[SerializeField] private float initialTimerDurationBoat;
		[SerializeField] private float minTimerDurationBoat;


		public int ReturnBoatFishCapacity() 
		{
			return boatFishCapacity + (dockUpgradeData.capacityLevel - 1) * boatCapacityIncrease;
		}

		public int UpdateDockUpgradeBoatLevelCost(int newLevel)
		{
			int boatLevelCost = boatLevelUpgradeCost * newLevel;
			return boatLevelCost;
		}

		public int UpdateDockUpgradeTimerLevelCost(int newLevel)
		{
			int timerLevelCost = timerLevelUpgradeCost * newLevel;
			return timerLevelCost;
		}

		public int UpdateDockUpgradeCapacityLevelCost(int newLevel)
		{
			int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
			return capacityLevelCost;
		}


		public float TimerLevelIncrease()
		{

			if (dockUpgradeData.timerLevel % 5 == 0)
			{
				dockUpgradeData.currentTimerDurationBoat = Mathf.Max(minTimerDurationBoat, initialTimerDurationBoat - 1.0f);
			}

			return dockUpgradeData.currentTimerDurationBoat;
		}

	}
}

