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
		public DockUpgradeData dockUpgradeData;
		public ShackUpgradeData shackUpgradeData;
		public HouseUpgradeData houseUpgradeData;

		public float money;
		public int totalFishCountFishBox;
		public int totalFishCountShack;



	}

	public class SaveLoadManager : MonoBehaviour
	{
		public static SaveLoadManager Instance;
		public SaveData saveData { get; set; }

		public MoneyManager moneyManager;
		public FishBoxController fishBoxController;
		public ShackController shackController;

		private string savePath;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
			savePath = Application.persistentDataPath + "/saveData.json";
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				ResetGame();
				Debug.Log("Reset Game");
			}
		}

		public void Initialize()
		{
			LoadGame();

			DockUpgradeData savedDataDock = LoadDockUpgradeData();
			DockUpgradeManager.Instance.dockUpgradeData = savedDataDock;
			DockUpgradeManager.Instance.UpdateUpgradeCosts();

			ShackUpgradeData savedDataShack = LoadShackUpgradeData();
			ShackUpgradeManager.Instance.shackUpgradeData = savedDataShack;
			ShackUpgradeManager.Instance.UpdateUpgradeCosts();
		}
		public void SaveGame()
		{
			saveData.money = MoneyManager.Instance.money;
			saveData.totalFishCountFishBox = FishBoxController.Instance.GetTotalFishCount();
			saveData.totalFishCountShack = ShackController.Instance.GetTotalFishCount();
			saveData.dockUpgradeData = DockUpgradeManager.Instance.dockUpgradeData;
			saveData.shackUpgradeData = ShackUpgradeManager.Instance.shackUpgradeData;

			string jsonData = JsonUtility.ToJson(saveData);
			File.WriteAllText(savePath, jsonData);
		}

		public void LoadGame()
		{
			if (File.Exists(savePath))
			{
				string jsonData = File.ReadAllText(savePath);
				saveData = JsonUtility.FromJson<SaveData>(jsonData);

				int loadedFishCountFishBox = saveData.totalFishCountFishBox;
				FishBoxController.Instance.SetTotalFishCount(loadedFishCountFishBox);

				int loadedFishCountShack = saveData.totalFishCountShack;
				ShackController.Instance.SetTotalFishCount(loadedFishCountShack);

				if (MoneyManager.Instance != null)
				{
					MoneyManager.Instance.money = saveData.money;
					MoneyManager.Instance.UpdateMoneyText();
				}
				else
				{
					Debug.LogError("MoneyManager instance is missing!");
				}

			}
			else
			{
				saveData = new SaveData();
				saveData.money = MoneyManager.Instance.money;
				saveData.dockUpgradeData = new DockUpgradeData();
				saveData.shackUpgradeData = new ShackUpgradeData();
			}
		}

		public void OnApplicationQuit()
		{
			SaveGame();
		}

		public void SaveDockUpgradeData(DockUpgradeData dockUpgradeData)
		{
			saveData.dockUpgradeData = dockUpgradeData;

			string jsonData = JsonUtility.ToJson(saveData);
			File.WriteAllText(savePath, jsonData);
		}

		public DockUpgradeData LoadDockUpgradeData()
		{
			if (File.Exists(savePath))
			{
				string jsonData = File.ReadAllText(savePath);
				SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonData);
				return loadedData.dockUpgradeData;
			}
			else
			{
				return new DockUpgradeData();
			}
		}

		public void SaveShackUpgradeData(ShackUpgradeData shackUpgradeData)
		{
			saveData.shackUpgradeData = shackUpgradeData;

			string jsonData = JsonUtility.ToJson(saveData);
			File.WriteAllText(savePath, jsonData);
		}

		public ShackUpgradeData LoadShackUpgradeData()
		{
			if (File.Exists(savePath))
			{
				string jsonData = File.ReadAllText(savePath);
				SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonData);
				return loadedData.shackUpgradeData;
			}
			else
			{
				return new ShackUpgradeData();
			}
		}

		public void ResetGame()
		{
			saveData = new SaveData();
			saveData.dockUpgradeData = new DockUpgradeData();
			saveData.shackUpgradeData = new ShackUpgradeData();

			FishBoxController.Instance.Reset();
			ShackController.Instance.Reset();

			if (DockUpgradeManager.Instance != null)
			{
				DockUpgradeManager.Instance.ResetGame();
			}

			if (ShackUpgradeManager.Instance!=null)
			{
				ShackUpgradeManager.Instance.ResetGame();
			}

			SaveGame();

			Debug.Log("Game reset successfully");
		}

	}
}


