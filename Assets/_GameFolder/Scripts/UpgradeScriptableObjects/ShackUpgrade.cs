using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishingIsland.Managers;

namespace FishingIsland.UpgradeScriptableObjects
{
	[Serializable]
	public class ShackUpgradeData
	{
		public int dockWorkerLevel;
		public int speedLevel;
		public int capacityLevel;

		public ShackUpgradeData(int levelDockWorker, int levelSpeed, int levelCapacity)
		{
			dockWorkerLevel = levelDockWorker;
			speedLevel = levelSpeed;
			capacityLevel = levelCapacity;
		}
	}

	[CreateAssetMenu(fileName = "New Shack Upgrade", menuName = "Shack Upgrade")]
	public class ShackUpgrade : ScriptableObject
	{
		public ShackUpgradeData shackUpgradeData => SaveLoadManager.Instance.saveData.shackUpgradeData;

		[SerializeField] private int dockWorkerCapacityIncrease;

		[SerializeField] private int dockWorkerLevelUpgradeCost;
		[SerializeField] private int speedLevelUpgradeCost;
		[SerializeField] private int capacityLevelUpgradeCost;

		[SerializeField] private int dockWorkerFishCapacity;

		[SerializeField] private float initialTimerDurationFishWorker;
		[SerializeField] private float minTimerDurationFishWorker;


		[SerializeField] private float speedIncreasePerLevel;
		[SerializeField] private float initialSpeed;

		public int ReturnDockWorkerFishCapacity()
		{
			return dockWorkerFishCapacity + (shackUpgradeData.capacityLevel - 1) * dockWorkerCapacityIncrease;
		}

		public int UpdateShackUpgradeDockWorkerLevelCost(int newLevel)
		{
			int dockWorkerLevelCost = dockWorkerLevelUpgradeCost * newLevel;
			return dockWorkerLevelCost;
		}

		public int UpdateShackUpgradeSpeedLevelCost(int newLevel)
		{
			int speedLevelCost = speedLevelUpgradeCost * newLevel;
			return speedLevelCost;
		}

		public int UpdateShackUpgradeCapacityLevelCost(int newLevel)
		{
			int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
			return capacityLevelCost;
		}

		public float UpdateShackUpgradeSpeed(int newLevel)
		{
			return initialSpeed + (speedIncreasePerLevel * (newLevel - 1));
		}

		public void ResetGameShackUpgrade()
		{
			shackUpgradeData.dockWorkerLevel = 1;
			shackUpgradeData.speedLevel = 1;
			shackUpgradeData.capacityLevel = 1;

			if (DockUpgradeManager.Instance != null)
			{
				DockUpgradeManager.Instance.UpdateUpgradeCosts();
			}
		}
	}
}

