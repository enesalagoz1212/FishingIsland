using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
	[CreateAssetMenu(fileName = "New Dock Upgrade", menuName = "Dock Upgrade")]
	public class DockUpgrade : ScriptableObject
	{
		public int boatLevel;
		public int timerLevel;
		public int capacityLevel;

		public int boatCapacityIncrease;

		public int boatLevelUpgradeCost;
		public int timerLevelUpgradeCost;
		public int capacityLevelUpgradeCost;

		public int boatFishCapacity;

		public float initialTimerDurationBoat;
		public float minTimerDurationBoat;

		public void Reset()
		{
			boatLevel = 1;
			timerLevel = 1;
			capacityLevel = 1;

			boatCapacityIncrease = 12;

			boatLevelUpgradeCost = 35;
			timerLevelUpgradeCost = 45;
			capacityLevelUpgradeCost = 40;

			boatFishCapacity = 10;

			initialTimerDurationBoat = 7f;
			minTimerDurationBoat = 3f;
		}
	}
}

