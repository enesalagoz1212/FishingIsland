using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FishingIsland.Managers
{
	public class MoneyManager : MonoBehaviour
	{
		public static MoneyManager Instance { get; private set; }

		public float money;
		public float startingMoney;
		public TextMeshProUGUI moneyText;

		public void Initialize()
		{
			UpdateMoneyText();
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

		public void AddMoney(float amount)
		{
			money += amount;
			UpdateMoneyText();
		}

		public float GetMoney()
		{
			return money;
		}

		public void UpdateMoneyText()
		{
			moneyText.text = $" {money}";
		}

		public void Reset()
		{
			money = 0f;
			UpdateMoneyText();
		}
	}
}

