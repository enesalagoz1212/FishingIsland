using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
    [CreateAssetMenu(fileName = "New Shack Upgrade", menuName = "Shack Upgrade")]
    public class ShackUpgrade : ScriptableObject
    {
        public int dockWorkerLevel;
        public int timerLevel;
        public int capacityLevel;

        public int dockWorkerCapacityIncrease;

        public int dockWorkerLevelUpgradeCost;
        public int timerLevelUpgradeCost;
        public int capacityLevelUpgradeCost;

        public int dockWorkerFishCapacity;

        public float initialTimerDurationFishWorker;
        public float minTimerDurationFishWorker;

        public void Reset()
        {
            dockWorkerLevel = 1;
            timerLevel = 1;
            capacityLevel = 1;

            dockWorkerCapacityIncrease = 6;

            dockWorkerLevelUpgradeCost = 35;
            timerLevelUpgradeCost = 45;
            capacityLevelUpgradeCost =40;

            dockWorkerFishCapacity = 10;

            initialTimerDurationFishWorker = 7f;
            minTimerDurationFishWorker = 3f;
        }
    }
}

