using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FishingIsland.UpgradeScriptableObjects
{
    [Serializable]
    public class ShackUpgradeData
    {
        public int dockWorkerLevel = 1;
        public int timerLevel = 1;
        public int capacityLevel = 1;
    }

    [CreateAssetMenu(fileName = "New Shack Upgrade", menuName = "Shack Upgrade")]
    public class ShackUpgrade : ScriptableObject
    {
      

        public int dockWorkerCapacityIncrease;

        public int dockWorkerLevelUpgradeCost;
        public int timerLevelUpgradeCost;
        public int capacityLevelUpgradeCost;

        public int dockWorkerFishCapacity;

        public float initialTimerDurationFishWorker;
        public float minTimerDurationFishWorker;
 
    }
}

