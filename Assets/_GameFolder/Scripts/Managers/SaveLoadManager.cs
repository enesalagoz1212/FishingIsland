using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using System.IO;
using System;
using FishingIsland.Managers;

namespace FishingIsland.Managers
{

    [System.Serializable]
    public class SaveData
	{
        public DockUpgrade dockUpgrade;
        public HouseUpgrade houseUpgrade;
        public ShackUpgrade shackUpgrade;
        public float money;

        public SaveData(DockUpgrade dockUpgrade, HouseUpgrade houseUpgrade, ShackUpgrade shackUpgrade, float money)
		{
            this.dockUpgrade = dockUpgrade;
            this.houseUpgrade = houseUpgrade;
            this.shackUpgrade = shackUpgrade;
            this.money = money;
		}
	}

    public class SaveLoadManager : MonoBehaviour
    {
        public DockUpgrade dockUpgrade;
        public HouseUpgrade houseUpgrade;
        public ShackUpgrade shackUpgrade;
        public MoneyManager moneyManager;

        public void Initialize()
		{
            LoadSave();

		}

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        public void SaveGame()
		{
            SaveData saveData = new SaveData(dockUpgrade, houseUpgrade, shackUpgrade, moneyManager.money);

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
    }
}


