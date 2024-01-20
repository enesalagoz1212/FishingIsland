using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
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

