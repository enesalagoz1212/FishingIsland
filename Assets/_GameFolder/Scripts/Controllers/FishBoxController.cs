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
		private bool _numberOfFish = true;

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
			_numberOfFish = true;
			_isFishCollectionCompletedBox = false;
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_dockCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();

			float initialTime = dockWorkerController.GetCurrentTimerDuration();
			float currentTime = initialTime;
			float elapsedTime = 0;
			int fishCount = 0;
			while (fishCount < _dockCapacity && HasFishBox && !_isFishCollectionCompletedBox)
			{
				elapsedTime += Time.deltaTime;
				int currentDockCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();
				if (currentDockCapacity != _dockCapacity)
				{
					_dockCapacity = currentDockCapacity;
					_numberOfFish = false;
					float newCurrentTime = shackUpgrade.TimerLevelIncrease() - elapsedTime;
					currentTime = newCurrentTime;
				}

				if (_numberOfFish)
				{
					if (_totalFishCount > _dockCapacity)
					{
						_timePerFish = currentTime / _dockCapacity;
					}
					else
					{
						_timePerFish = currentTime / _totalFishCount;
					}
				}
				else
				{
					if (_totalFishCount > _dockCapacity)
					{
						_timePerFish = currentTime / _dockCapacity;
					}
					else
					{
						_timePerFish = currentTime / _totalFishCount;
					}
				}

				dockWorkerController.OnFishCollectedFishBox();
				DecreaseFishCount(1);
				fishCount++;
				yield return new WaitForSeconds(_timePerFish);
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

