using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public static MoneyManager Instance { get;  set; }

		public float money;

		public float GetMoney()
		{
			return money;
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
		}
	}
}

