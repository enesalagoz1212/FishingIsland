using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.UpgradeScriptableObjects;
using System.IO;
using System;
using FishingIsland.Managers;
using FishingIsland.Controllers;
using FishingIsland.UpgradeDatas;

namespace FishingIsland.Managers
{

    [System.Serializable]
    public class SaveData
	{
   
        public float money;
        public int totalFishCountFishBox;
        public int totalFishCountShack;

        
	}

    public class SaveLoadManager : MonoBehaviour
    {

        public MoneyManager moneyManager;
        public FishBoxController fishBoxController;
        public ShackController shackController;

        public void Initialize()
		{
          
          
		}

        private void OnApplicationQuit()
        {
          
        }




        
    }
}


