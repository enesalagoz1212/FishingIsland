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
		public static Action<DockWorkerController> OnDockWorkerArrivedBox;
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
			OnDockWorkerArrivedBox += OnDockWorkerArrivedBoxAction;
		}
		public void Initialize()
		{

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

		private void OnDockWorkerArrivedBoxAction(DockWorkerController dockWorkerController)
		{
			// dockWorker came to the fish Box
			StartCoroutine(StartFishTransferFromDockWorker(dockWorkerController));
		}

		private IEnumerator StartFishTransferFromBoat(BoatController boatController)
		{
			while (boatController.FishCount > 0)
			{
				boatController.OnFishTransferredToFishBox();
				IncreaseFishCount(1);
				yield return new WaitForSeconds(0.1f);
			}
		}

		private IEnumerator StartFishTransferFromDockWorker(DockWorkerController dockWorkerController)
		{

			for (int i = 0; i < dockWorkerController.Capacity; i++)
			{
				dockWorkerController.OnFishCollectedFishBox();
				DecreaseFishCount(1);
				yield return new WaitForSeconds(0.1f);
			}
		}


		private void IncreaseFishCount(int amount)
		{
			_totalFishCount += amount;
			UpdateFishCountText();
		}

		private void DecreaseFishCount(int amount)
		{
			_totalFishCount -= amount;
			UpdateFishCountText();
		}
	}
}

