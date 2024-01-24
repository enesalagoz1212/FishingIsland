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
		private int _totalFishCount;
		private float _timePerFish;
		private bool _isFishCollectionCompletedBox = false;

		public TextMeshProUGUI boxFishText;
		public bool IsFishCollectionCompleted => _isFishCollectionCompletedBox;
		public bool HasFishBox => _totalFishCount > 0;
		public int startingFishCount;

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

		public int GetTotalFishCount()
		{
			return _totalFishCount;
		}

		public void SetTotalFishCount(int totalFishCount)
		{
			_totalFishCount = totalFishCount;
			UpdateFishCountText();
		}

		private void UpdateFishCountText()
		{
			boxFishText.text = $" {_totalFishCount}";
		}

		private void OnBoatArrivedBoxAction(BoatController boatController)
		{
			StartCoroutine(StartFishTransferFromBoat(boatController));
		}

		private void OnDockWorkerArrivedBoxAction(DockWorkerController dockWorkerController)
		{
			StartCoroutine(StartFishTransferFromDockWorker(dockWorkerController));
		}

		private IEnumerator StartFishTransferFromBoat(BoatController boatController)
		{
			while (boatController.FishCount > 0)
			{
				boatController.OnFishTransferredToFishBox();
				IncreaseFishCount(1);
				yield return new WaitForSeconds(0.02f);
			}
		}

		private IEnumerator StartFishTransferFromDockWorker(DockWorkerController dockWorkerController)
		{
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_dockCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();
			Debug.Log($"DockCapacity: {_dockCapacity}");
			if (_totalFishCount > _dockCapacity)
			{
				_timePerFish = dockWorkerController.GetCurrentTimerDuration() / _dockCapacity;
			}
			else
			{
				_timePerFish = dockWorkerController.GetCurrentTimerDuration() / _totalFishCount;
			}		

			for (int i = 0; i < _dockCapacity; i++)
			{
				if (HasFishBox)
				{
					dockWorkerController.OnFishCollectedFishBox();
					DecreaseFishCount(1);
					yield return new WaitForSeconds(_timePerFish);
				}
				else
				{
					break;
				}
			}
			_isFishCollectionCompletedBox = true;
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

		public void Reset()
		{
			_totalFishCount = 0;
			UpdateFishCountText();
		}
	}
}

