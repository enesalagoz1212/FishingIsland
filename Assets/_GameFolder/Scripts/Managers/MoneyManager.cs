using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public static MoneyManager Instance { get; private set; }

		public float money;

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
	}
}

