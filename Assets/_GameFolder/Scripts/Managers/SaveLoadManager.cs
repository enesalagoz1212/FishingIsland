using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using System.IO;
using System;
using FishingIsland.Managers;
using FishingIsland.Controllers;

namespace FishingIsland.Managers
{

    [System.Serializable]
    public class SaveData
	{
        public DockUpgrade dockUpgrade;
        public HouseUpgrade houseUpgrade;
        public ShackUpgrade shackUpgrade;
        public float money;
        public int totalFishCountFishBox;
        public int totalFishCountShack;

        public SaveData(DockUpgrade dockUpgrade, HouseUpgrade houseUpgrade, ShackUpgrade shackUpgrade, float money,int totalFishCountFishBox,int totalFishCountShack)
		{
            this.dockUpgrade = dockUpgrade;
            this.houseUpgrade = houseUpgrade;
            this.shackUpgrade = shackUpgrade;
            this.money = money;
            this.totalFishCountFishBox = totalFishCountFishBox;
            this.totalFishCountShack = totalFishCountShack;
        }
	}

    public class SaveLoadManager : MonoBehaviour
    {
        public DockUpgrade dockUpgrade;
        public HouseUpgrade houseUpgrade;
        public ShackUpgrade shackUpgrade;
        public MoneyManager moneyManager;
        public FishBoxController fishBoxController;
        public ShackController shackController;

        public void Initialize()
		{
            LoadSave();
            //ResetSaveData();
		}

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        public void SaveGame()
		{
            SaveData saveData = new SaveData(dockUpgrade, houseUpgrade, shackUpgrade, moneyManager.money, fishBoxController.GetTotalFishCount(),shackController.GetTotalFishCount());


            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText("save.json", json);
		}


		private void LoadSave()
		{
			if (File.Exists("save.json"))
			{
                string json = File.ReadAllText("save.json");
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                LoadUpgrade(dockUpgrade, saveData.dockUpgrade);
                LoadUpgrade(dockUpgrade, saveData.houseUpgrade);
                LoadUpgrade(dockUpgrade, saveData.shackUpgrade);

                moneyManager.money = saveData.money;
                moneyManager.UpdateMoneyText();

                fishBoxController.SetTotalFishCount(saveData.totalFishCountFishBox);
                shackController.SetTotalFishCount(saveData.totalFishCountShack);
            }
			else
			{
                Debug.Log("Save file not found");
			}
		}

        private void LoadUpgrade(ScriptableObject upgrade, ScriptableObject savedUpgrade)
        {
            if (savedUpgrade != null)
            {
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(savedUpgrade), upgrade);
            }
        }

        public void ResetSaveData()
        {
            Debug.Log("ResetSaveData worked");
            dockUpgrade.Reset();
            houseUpgrade.Reset();
            shackUpgrade.Reset();
            moneyManager.Reset();
            fishBoxController.Reset();
            shackController.Reset();

            SaveData saveData = new SaveData(dockUpgrade, houseUpgrade, shackUpgrade, moneyManager.money, fishBoxController.GetTotalFishCount(), shackController.GetTotalFishCount());

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText("save.json", json);
        }
    }
}


