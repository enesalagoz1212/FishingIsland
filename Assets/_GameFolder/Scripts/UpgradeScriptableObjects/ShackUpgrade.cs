using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishingIsland.Managers;

namespace FishingIsland.UpgradeScriptableObjects
{
    [Serializable]
    public class ShackUpgradeData
    {
        public int dockWorkerLevel = 1;
        public int timerLevel = 1;
        public int capacityLevel = 1;

        public float currentTimerDurationDockWorker = 7;
    }

    [CreateAssetMenu(fileName = "New Shack Upgrade", menuName = "Shack Upgrade")]
    public class ShackUpgrade : ScriptableObject
    {
        public ShackUpgradeData shackUpgradeData => SaveLoadManager.Instance.saveData.shackUpgradeData;
      
        [SerializeField] private int dockWorkerCapacityIncrease;

        [SerializeField] private int dockWorkerLevelUpgradeCost;
        [SerializeField] private int timerLevelUpgradeCost;
        [SerializeField] private int capacityLevelUpgradeCost;

        [SerializeField] private int dockWorkerFishCapacity;

        [SerializeField] private float initialTimerDurationFishWorker;
        [SerializeField] private float minTimerDurationFishWorker;


        public int ReturnDockWorkerFishCapacity()
        {
            return dockWorkerFishCapacity + (shackUpgradeData.capacityLevel - 1) * dockWorkerCapacityIncrease;
        }

        public int UpdateShackUpgradeDockWorkerLevelCost(int newLevel)
        {
            int dockWorkerLevelCost = dockWorkerLevelUpgradeCost * newLevel;
            return dockWorkerLevelCost;
        }

        public int UpdateShackUpgradeTimerLevelCost(int newLevel)
        {
            int timerLevelCost = timerLevelUpgradeCost * newLevel;
            return timerLevelCost;
        }

        public int UpdateShackUpgradeCapacityLevelCost(int newLevel)
        {
            int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
            return capacityLevelCost;
        }

        public float TimerLevelIncrease()
        {
            if (shackUpgradeData.timerLevel % 5 == 0)
            {
               shackUpgradeData.currentTimerDurationDockWorker = Mathf.Max(minTimerDurationFishWorker, initialTimerDurationFishWorker - 1.0f);
            }

            return shackUpgradeData.currentTimerDurationDockWorker;
        }

        public float CurrentTimerDuration()
		{
            return shackUpgradeData.currentTimerDurationDockWorker;
		}

        public void ResetGameShackUpgrade()
        {
            shackUpgradeData.currentTimerDurationDockWorker = 7;
            shackUpgradeData.dockWorkerLevel = 1;
            shackUpgradeData.timerLevel = 1;
            shackUpgradeData.capacityLevel = 1;


            if (DockUpgradeManager.Instance != null)
            {
                DockUpgradeManager.Instance.UpdateUpgradeCosts();
            }
        }
    }
}

