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
		public int timerLevel;
		public int capacityLevel;

		public DockUpgradeData(int levelBoat, int levelTimer, int levelCapacity)
		{
			boatLevel = levelBoat;
			timerLevel = levelTimer;
			capacityLevel = levelCapacity;
		}
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
			float divisionDock = dockUpgradeData.timerLevel / 5f;
			divisionDock = MathF.Floor(divisionDock);
			var time = initialTimerDurationBoat - divisionDock;
			return Mathf.Max(time, minTimerDurationBoat);
		}


		public void ResetGameDockUpgrade()
		{
			dockUpgradeData.boatLevel = 1;
			dockUpgradeData.timerLevel = 1;
			dockUpgradeData.capacityLevel = 1;


			if (DockUpgradeManager.Instance != null)
			{
				DockUpgradeManager.Instance.UpdateUpgradeCosts();
			}
		}
	}
}

