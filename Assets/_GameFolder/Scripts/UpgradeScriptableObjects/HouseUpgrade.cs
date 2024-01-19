using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
    [CreateAssetMenu(fileName = "New House Upgrade", menuName = "House Upgrade")]
    public class HouseUpgrade : ScriptableObject
    {
        public int fishWorkerLevel;
        public int timerLevel;
        public int capacityLevel;

        public int fishWorkerCapacityIncrease;

        public int fishWorkerLevelUpgradeCost;
        public int timerLevelUpgradeCost;
        public int capacityLevelUpgradeCost;

        public int fishWorkerFishCapacity;

        public float initialTimerDurationHouse;
        public float minTimerDurationHouse;

        public void Reset()
        {
            fishWorkerLevel = 1;
            timerLevel = 1;
            capacityLevel = 1;

            fishWorkerCapacityIncrease = 8;

            fishWorkerLevelUpgradeCost = 35;
            timerLevelUpgradeCost = 45;
            capacityLevelUpgradeCost = 40;

            fishWorkerFishCapacity = 10;

            initialTimerDurationHouse = 7f;
            minTimerDurationHouse = 3f;
        }
    }
}

