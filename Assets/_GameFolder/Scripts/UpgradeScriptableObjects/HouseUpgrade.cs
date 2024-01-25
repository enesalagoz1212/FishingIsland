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
		public int timerLevel;
		public int capacityLevel;
		public HouseUpgradeData(int levelFishWorker,int levelTimer,int levelCapacity)
		{
			fishWorkerLevel = levelFishWorker;
			timerLevel = levelTimer;
			capacityLevel = levelCapacity;
		}
	}

	[CreateAssetMenu(fileName = "New House Upgrade", menuName = "House Upgrade")]
	public class HouseUpgrade : ScriptableObject
	{
		public HouseUpgradeData houseUpgradeData => SaveLoadManager.Instance.saveData.houseUpgradeData;

		[SerializeField] private int fishWorkerCapacityIncrease;

		[SerializeField] private int fishWorkerLevelUpgradeCost;
		[SerializeField] private int timerLevelUpgradeCost;
		[SerializeField] private int capacityLevelUpgradeCost;

		[SerializeField] private int fishWorkerFishCapacity;

		[SerializeField] private float initialTimerDurationHouse;
		[SerializeField] private float minTimerDurationHouse;



		public int ReturnFishWorkerFishCapacity()
		{
			return fishWorkerFishCapacity + (houseUpgradeData.capacityLevel - 1) * fishWorkerCapacityIncrease;
		}

		public int UpdateHouseUpgradeFishWorkerLevelCost(int newLevel)
		{
			int fishWorkerLevelCost = fishWorkerLevelUpgradeCost * newLevel;
			return fishWorkerLevelCost;
		}

		public int UpdateHouseUpgradeTimerLevelCost(int newLevel)
		{
			int timerLevelCost = timerLevelUpgradeCost * newLevel;
			return timerLevelCost;
		}

		public int UpdateHouseUpgradeCapacityLevelCost(int newLevel)
		{
			int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
			return capacityLevelCost;
		}

		public float TimerLevelIncrease()
		{
			float divisionHouse = houseUpgradeData.timerLevel / 5f;
			divisionHouse = MathF.Floor(divisionHouse);
			var time = initialTimerDurationHouse - divisionHouse;
			return Mathf.Max(time, minTimerDurationHouse);
		}

		public void ResetGameHouseUpgrade()
		{
			houseUpgradeData.fishWorkerLevel = 1;
			houseUpgradeData.timerLevel = 1;
			houseUpgradeData.capacityLevel = 1;


			if (HouseUpgradeManager.Instance != null)
			{
				HouseUpgradeManager.Instance.UpdateUpgradeCosts();
			}
		}
	}
}

