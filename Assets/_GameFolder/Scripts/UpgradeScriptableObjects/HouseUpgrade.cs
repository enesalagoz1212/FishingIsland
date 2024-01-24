using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishingIsland.Managers;

namespace FishingIsland.UpgradeScriptableObjects
{
    [Serializable]
    public class HouseUpgradeData
    {
        public int fishWorkerLevel = 1;
        public int timerLevel = 1;
        public int capacityLevel = 1;

        public float currentTimerDurationFishWorker = 7;
    }

    [CreateAssetMenu(fileName = "New House Upgrade", menuName = "House Upgrade")]
    public class HouseUpgrade : ScriptableObject
    {
        public HouseUpgradeData houseUpgradeData => SaveLoadManager.Instance.saveData.houseUpgradeData;

        [SerializeField] private int fishWorkerCapacityIncrease;

        [SerializeField] private int fishWorkerLevelUpgradeCost;
        [SerializeField] private int timerLevelUpgradeCost;
        [SerializeField] private int capacityLevelUpgradeCost;

        [SerializeField] private int fishWorkerFishCapacity;

        [SerializeField] private float initialTimerDurationHouse;
        [SerializeField] private float minTimerDurationHouse;



        public int ReturnFishWorkerFishCapacity()
        {
            return fishWorkerFishCapacity + (houseUpgradeData.capacityLevel - 1) * fishWorkerCapacityIncrease;
        }

        public int UpdateHouseUpgradeFishWorkerLevelCost(int newLevel)
        {
            int fishWorkerLevelCost = fishWorkerLevelUpgradeCost * newLevel;
            return fishWorkerLevelCost;
        }

        public int UpdateHouseUpgradeTimerLevelCost(int newLevel)
        {
            int timerLevelCost = timerLevelUpgradeCost * newLevel;
            return timerLevelCost;
        }

        public int UpdateHouseUpgradeCapacityLevelCost(int newLevel)
        {
            int capacityLevelCost = capacityLevelUpgradeCost * newLevel;
            return capacityLevelCost;
        }

        public float TimerLevelIncrease()
        {

            if (houseUpgradeData.timerLevel % 5 == 0)
            {
                houseUpgradeData.currentTimerDurationFishWorker = Mathf.Max(minTimerDurationHouse, initialTimerDurationHouse - 1.0f);
            }

            return houseUpgradeData.currentTimerDurationFishWorker;
        }

        public void ResetGameHouseUpgrade()
        {
            houseUpgradeData.currentTimerDurationFishWorker = 7;
            houseUpgradeData.fishWorkerLevel = 1;
            houseUpgradeData.timerLevel = 1;
            houseUpgradeData.capacityLevel = 1;


            if (HouseUpgradeManager.Instance != null)
            {
                HouseUpgradeManager.Instance.UpdateUpgradeCosts();
            }
        }
    }
}

