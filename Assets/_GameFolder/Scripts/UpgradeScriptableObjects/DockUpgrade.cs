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
		public int boatLevel;
		public int speedLevel;
		public int capacityLevel;

		public DockUpgradeData(int levelBoat, int levelSpeed, int levelCapacity)
		{
			boatLevel = levelBoat;
			speedLevel = levelSpeed;
			capacityLevel = levelCapacity;
		}
	}

	[CreateAssetMenu(fileName = "New Dock Upgrade", menuName = "Dock Upgrade")]
	public class DockUpgrade : ScriptableObject
	{
		public DockUpgradeData dockUpgradeData => SaveLoadManager.Instance.saveData.dockUpgradeData;

		[SerializeField] private int boatCapacityIncrease;

		[SerializeField] private int boatLevelUpgradeCost;
		[SerializeField] private int speedLevelUpgradeCost;
		[SerializeField] private int capacityLevelUpgradeCost;


		[SerializeField] private int boatFishCapacity;

		[SerializeField] private float speedLevelIncreasePerLevel;
		[SerializeField] private float initialSpeedLevel;

		[SerializeField] private float boatLevelIncreasePerLevel;
		[SerializeField] private float initialBoatLevel;

		public int ReturnBoatFishCapacity() 
		{
			return boatFishCapacity + (dockUpgradeData.capacityLevel - 1) * boatCapacityIncrease;
		}

		public int UpdateDockUpgradeBoatLevelCost(int newLevel)
		{
			int boatLevelCost = boatLevelUpgradeCost * newLevel;
			return boatLevelCost;
		}

		public int UpdateDockUpgradeSpeedLevelCost(int newLevel)
		{
			int speedLevelCost = speedLevelUpgradeCost * newLevel;
			return speedLevelCost;
		}

		public int UpdateDockUpgradeCapacityLevelCost(int newLevel)
		{
			int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
			return capacityLevelCost;
		}

		public float UpdateDockUpgradeSpeedLevel(int newLevel)
		{
			return initialSpeedLevel + (speedLevelIncreasePerLevel * (newLevel - 1));
		}

		public float UpdateDockUpgradeBoatLevel(int newLevel)
		{
		     return initialBoatLevel - (boatLevelIncreasePerLevel * (newLevel - 1));
		}


		public void ResetGameDockUpgrade()
		{
			dockUpgradeData.boatLevel = 1;
			dockUpgradeData.speedLevel = 1;
			dockUpgradeData.capacityLevel = 1;


			if (DockUpgradeManager.Instance != null)
			{
				DockUpgradeManager.Instance.UpdateUpgradeCosts();
			}
		}
	}
}

