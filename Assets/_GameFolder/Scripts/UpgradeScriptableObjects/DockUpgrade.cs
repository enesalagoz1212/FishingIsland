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
	}
}

