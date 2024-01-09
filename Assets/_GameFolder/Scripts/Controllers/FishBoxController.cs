using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace FishingIsland.Controllers
{
    public class FishBoxController : MonoBehaviour
    {
        public static FishBoxController Instance;

        public TextMeshProUGUI boxFishText;
        private int _totalFishCount = 0;

        public bool HasFishBox => _totalFishCount > 0;

        public static Action<BoatController> OnBoatArrivedBox;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            OnBoatArrivedBox += OnBoatArrivedBoxAction;
        }
        public void Initialize()
		{

		}

        public IEnumerator TransferFish()
        {
            while (_totalFishCount > 0)
            {
                yield return new WaitForSeconds(0.3f);
                _totalFishCount--;
                UpdateFishCountText();
            }
        }

        private void UpdateFishCountText()
        {
            boxFishText.text = $" {_totalFishCount}";
        }

        private void OnBoatArrivedBoxAction(BoatController boatController)
		{
            // Boat came to the Fish Box ( dock ) 
            StartCoroutine(StartFishTransferFromBoat(boatController));
		}

        private IEnumerator StartFishTransferFromBoat(BoatController boatController)
		{
            while(boatController.FishCount > 0)
			{
                boatController.OnFishTransferredToFishBox();
                IncreaseFishCount(1);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void IncreaseFishCount(int amount)
		{
            _totalFishCount += amount;
            UpdateFishCountText();
        }
    }
}

