using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FishingIsland.UpgradeScriptableObjects;
using FishingIsland.Managers;

namespace FishingIsland.Controllers
{
	public class FishBoxController : MonoBehaviour
	{
		public static FishBoxController Instance;

		private ShackUpgrade shackUpgrade;
		private int _dockCapacity;

		public TextMeshProUGUI boxFishText;
		private int _totalFishCount = 0;

		public bool HasFishBox => _totalFishCount > 0;

		private bool _isFishCollectionCompleted = false;
		public bool IsFishCollectionCompleted => _isFishCollectionCompleted;

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
		}
		public void Initialize()
		{

		}

		private void OnEnable()
		{
			OnBoatArrivedBox += OnBoatArrivedBoxAction;
			OnDockWorkerArrivedBox += OnDockWorkerArrivedBoxAction;
		}

		private void OnDisable()
		{
			OnBoatArrivedBox -= OnBoatArrivedBoxAction;
			OnDockWorkerArrivedBox -= OnDockWorkerArrivedBoxAction;
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
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_dockCapacity = shackUpgrade.dockWorkerFishCapacity;

			for (int i = 0; i < _dockCapacity; i++)
			{
				if (HasFishBox)
				{
					dockWorkerController.OnFishCollectedFishBox();
					DecreaseFishCount(1);
					yield return new WaitForSeconds(0.1f);
				}
			}
			_isFishCollectionCompleted = true;
			if (_isFishCollectionCompleted)
			{
				dockWorkerController.OnFishCollectionCompleted();
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

