using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FishingIsland.UpgradeScriptableObjects
{
    [Serializable]
    public class HouseUpgradeData
    {
        public int fishWorkerLevel = 1;
        public int timerLevel = 1;
        public int capacityLevel = 1;

    }

    [CreateAssetMenu(fileName = "New House Upgrade", menuName = "House Upgrade")]
    public class HouseUpgrade : ScriptableObject
    {
       
        public int fishWorkerCapacityIncrease;

        public int fishWorkerLevelUpgradeCost;
        public int timerLevelUpgradeCost;
        public int capacityLevelUpgradeCost;

        public int fishWorkerFishCapacity;

        public float initialTimerDurationHouse;
        public float minTimerDurationHouse;

       
    }
}

