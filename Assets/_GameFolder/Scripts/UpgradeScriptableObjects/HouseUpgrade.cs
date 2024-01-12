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


        public float fishWorkerSpeedDecrease;
        public int fishWorkerCapacityIncrease;

        public int fishWorkerLevelUpgradeCost;
        public int timerLevelUpgradeCost;
        public int capacityLevelUpgradeCost;

        public int fishWorkerFishCapacity;
    }
}

