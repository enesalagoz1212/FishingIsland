using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using System;
using TMPro;

namespace FishingIsland.Managers
{
	public class DockUpgradeManager : MonoBehaviour
	{
		public static DockUpgradeManager Instance { get; set; }

		public DockUpgrade dockUpgrade;
		public DockUpgradeData dockUpgradeData;

		public static Action<int> OnBoatLevelUpdated;
		public static Action<int> OnTimerLevelUpdated;
		public static Action<int> OnCapacityLevelUpdated;

		public TextMeshProUGUI boatLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI timerLevelIncreaseMoneyAmountText;
		public TextMeshProUGUI capacityLevelIncreaseMoneyAmountText;

		private int _boatUpgradeCost;
		private int _timerUpgradeCost;
		private int _capacityUpgradeCost;

		public void Initialize()
		{
			UpdateUpgradeCosts();

			BoatLevelIncreaseMoneyText(_boatUpgradeCost);
			TimerLevelIncreaseMoneyText(_timerUpgradeCost);
			CapacityLevelIncreaseMoneyText(_capacityUpgradeCost);

			
		}

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
		}

		public void UpdateUpgradeCosts()
		{
			_boatUpgradeCost = dockUpgrade.UpdateDockUpgradeBoatLevelCost(dockUpgradeData.boatLevel);
			_timerUpgradeCost = dockUpgrade.UpdateDockUpgradeTimerLevelCost(dockUpgradeData.timerLevel);
			_capacityUpgradeCost = dockUpgrade.UpdateDockUpgradeCapacityLevelCost(dockUpgradeData.capacityLevel);
		}

		public void UpgradeBoatLevel()
		{

			if (MoneyManager.Instance != null)
			{
				if (MoneyManager.Instance.money >= _boatUpgradeCost)
				{
					MoneyManager.Instance.money -= _boatUpgradeCost;
					dockUpgradeData.boatLevel++;

					int boatLevelCost = dockUpgrade.UpdateDockUpgradeBoatLevelCost(dockUpgradeData.boatLevel);

					OnBoatLevelUpdated?.Invoke(dockUpgradeData.boatLevel);
					BoatLevelIncreaseMoneyText(boatLevelCost);

					UpdateUpgradeCosts();

					SaveLoadManager.Instance.SaveGame();
					SaveLoadManager.Instance.SaveDockUpgradeData(dockUpgradeData);
					MoneyManager.Instance.UpdateMoneyText();
				}
				else
				{
					Debug.Log("insufficient money");
				}
			}
			else
			{
				Debug.LogError("MoneyManager instance is missing!");
			}
		}

		public void UpgradeTimerLevel()
		{

			if (MoneyManager.Instance.money >= _timerUpgradeCost)
			{
				MoneyManager.Instance.money -= _timerUpgradeCost;
				dockUpgradeData.timerLevel++;

				int timerLevelCost = dockUpgrade.UpdateDockUpgradeTimerLevelCost(dockUpgradeData.timerLevel);

				OnTimerLevelUpdated?.Invoke(dockUpgradeData.timerLevel);
				TimerLevelIncreaseMoneyText(timerLevelCost);

				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveDockUpgradeData(dockUpgradeData);
				MoneyManager.Instance.UpdateMoneyText();
			}
			else
			{
				Debug.Log("insufficient money");
			}
		}

		public void UpgradeCapacityLevel()
		{
			if (MoneyManager.Instance.money >= _capacityUpgradeCost)
			{
				MoneyManager.Instance.money -= _capacityUpgradeCost;
				dockUpgradeData.capacityLevel++;

				int totalFishCapacity = dockUpgrade.ReturnBoatFishCapacity();
				Debug.Log(totalFishCapacity);

				int capacityLevelCost = dockUpgrade.UpdateDockUpgradeCapacityLevelCost(dockUpgradeData.capacityLevel);

				Debug.Log(capacityLevelCost);

				OnCapacityLevelUpdated?.Invoke(dockUpgradeData.capacityLevel);
				CapacityLevelIncreaseMoneyText(capacityLevelCost);


				UpdateUpgradeCosts();

				SaveLoadManager.Instance.SaveGame();
				SaveLoadManager.Instance.SaveDockUpgradeData(dockUpgradeData);
				MoneyManager.Instance.UpdateMoneyText();
			}

			else
			{
				Debug.Log("insufficient money");
			}
		}



		public int GetBoatLevel()
		{
			return dockUpgradeData.boatLevel;
		}

		public int GetTimerLevel()
		{
			return dockUpgradeData.timerLevel;
		}

		public int GetCapacityLevel()
		{
			return dockUpgradeData.capacityLevel;
		}

		public void BoatLevelIncreaseMoneyText(int moneyText)
		{
			boatLevelIncreaseMoneyAmountText.text = $" {moneyText}";
		}

		public void TimerLevelIncreaseMoneyText(int moneyText)
		{
			timerLevelIncreaseMoneyAmountText.text = $"{moneyText}";
		}

		public void CapacityLevelIncreaseMoneyText(int moneyText)
		{
			capacityLevelIncreaseMoneyAmountText.text = $"{moneyText}";
		}

		public DockUpgrade GetDockUpgrade()
		{
			return dockUpgrade;
		}

		public void ResetGame()
		{
			dockUpgrade.ResetGame();
			dockUpgradeData = dockUpgrade.dockUpgradeData;


			OnBoatLevelUpdated?.Invoke(dockUpgradeData.boatLevel);
			OnTimerLevelUpdated?.Invoke(dockUpgradeData.timerLevel);
			OnCapacityLevelUpdated?.Invoke(dockUpgradeData.capacityLevel);

			MoneyManager.Instance.money = MoneyManager.Instance.startingMoney;
			MoneyManager.Instance.UpdateMoneyText();

			UpdateUpgradeCosts();
			BoatLevelIncreaseMoneyText(_boatUpgradeCost);
			TimerLevelIncreaseMoneyText(_timerUpgradeCost);
			CapacityLevelIncreaseMoneyText(_capacityUpgradeCost);
		}
	}
}
