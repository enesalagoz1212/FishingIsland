using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FishingIsland.UpgradeScriptableObjects
{
	[CreateAssetMenu(fileName = "New Dock Upgrade", menuName = "Dock Upgrade")]
	public class DockUpgrade : ScriptableObject
	{
		public int boatCapacityIncrease;

		public int boatLevelUpgradeCost;
		public int timerLevelUpgradeCost;
		public int capacityLevelUpgradeCost;


		public int boatFishCapacity;

		public float initialTimerDurationBoat;
		public float minTimerDurationBoat;


		public int ReturnBoatFishCapacity(int currentCapacityLevel)
		{
			return boatFishCapacity + (currentCapacityLevel - 1) * boatCapacityIncrease;
		}

		public void UpdateFishCapacity(int newCapacity)
		{
			boatFishCapacity = newCapacity;
		}

		public void UpdateUpgradeCapacityCost(int newLevel)
		{
			capacityLevelUpgradeCost = newLevel * 15 ;
		}
	}
}

