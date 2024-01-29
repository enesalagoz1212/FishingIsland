using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishingIsland.Managers;

namespace FishingIsland.UpgradeScriptableObjects
{
	[Serializable]
	public class HouseUpgradeData
	{
		public int fishWorkerLevel;
		public int speedLevel;
		public int capacityLevel;
		public HouseUpgradeData(int levelFishWorker,int levelSpeed,int levelCapacity)
		{
			fishWorkerLevel = levelFishWorker;
			speedLevel = levelSpeed;
			capacityLevel = levelCapacity;
		}
	}

	[CreateAssetMenu(fileName = "New House Upgrade", menuName = "House Upgrade")]
	public class HouseUpgrade : ScriptableObject
	{
		public HouseUpgradeData houseUpgradeData => SaveLoadManager.Instance.saveData.houseUpgradeData;

		[SerializeField] private int fishWorkerCapacityIncrease;

		[SerializeField] private int fishWorkerLevelUpgradeCost;
		[SerializeField] private int speedLevelUpgradeCost;
		[SerializeField] private int capacityLevelUpgradeCost;

		[SerializeField] private int fishWorkerFishCapacity;

		[SerializeField] private float initialTimerDurationHouse;
		[SerializeField] private float minTimerDurationHouse;

		[SerializeField] private float speedIncreasePerLevel;
		[SerializeField] private float initialSpeed;

		public int ReturnFishWorkerFishCapacity()
		{
			return fishWorkerFishCapacity + (houseUpgradeData.capacityLevel - 1) * fishWorkerCapacityIncrease;
		}

		public int UpdateHouseUpgradeFishWorkerLevelCost(int newLevel)
		{
			int fishWorkerLevelCost = fishWorkerLevelUpgradeCost * newLevel;
			return fishWorkerLevelCost;
		}

		public int UpdateHouseUpgradeSpeedLevelCost(int newLevel)
		{
			int speedLevelCost = speedLevelUpgradeCost * newLevel;
			return speedLevelCost;
		}

		public int UpdateHouseUpgradeCapacityLevelCost(int newLevel)
		{
			int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
			return capacityLevelCost;
		}

		public float UpdateHouseUpgradeSpeed(int newLevel)
		{
			return initialSpeed + (speedIncreasePerLevel * (newLevel - 1));
		}

		public void ResetGameHouseUpgrade()
		{
			houseUpgradeData.fishWorkerLevel = 1;
			houseUpgradeData.speedLevel = 1;
			houseUpgradeData.capacityLevel = 1;


			if (HouseUpgradeManager.Instance != null)
			{
				HouseUpgradeManager.Instance.UpdateUpgradeCosts();
			}
		}
	}
}

