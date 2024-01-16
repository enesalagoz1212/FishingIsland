using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FishingIsland.UpgradeScriptableObjects;
using FishingIsland.Managers;

namespace FishingIsland.Controllers
{
	public class ShackController : MonoBehaviour
	{
		public static ShackController Instance;
		private HouseUpgrade houseUpgrade;
		private int _fishCapacity;

		public GameObject shackUpgradeCanvas;
		public TextMeshProUGUI shackFishCountText;
		private int _shackFishCount = 0;
		private float _timePerFish;
		public bool HasFishShack => _shackFishCount > 0;

		private bool _isFishCollectionCompletedShack = false;
		public bool IsFishCollectionCompleted => _isFishCollectionCompletedShack;

		public static Action<FishWorkerController> OnFishWorkerArrivedBox;
		public static Action<DockWorkerController> OnDockWorkerArrivedShack;
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

		private void OnEnable()
		{
			OnFishWorkerArrivedBox += OnFishWorkerArrivedBoxAction;
			OnDockWorkerArrivedShack += OnFishWorkerArrivedShackAction;
		}

		private void OnDisable()
		{
			OnFishWorkerArrivedBox -= OnFishWorkerArrivedBoxAction;
			OnDockWorkerArrivedShack -= OnFishWorkerArrivedShackAction;
		}
		private void OnMouseDown()
		{
			Debug.Log("ShackController");
			shackUpgradeCanvas.SetActive(true);
		}


		private IEnumerator StartFishTransferFromFishWorker(FishWorkerController fishWorkerController)
		{
			houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			_fishCapacity = houseUpgrade.fishWorkerFishCapacity;
			_isFishCollectionCompletedShack = false;

			if (_shackFishCount > _fishCapacity)
			{
				_timePerFish = fishWorkerController.GetCurrentTimerDuration() / _fishCapacity;
			}
			else
			{
				_timePerFish = fishWorkerController.GetCurrentTimerDuration() / _shackFishCount;
			}


			for (int i = 0; i < _fishCapacity; i++)
			{
				if (HasFishShack)
				{
					fishWorkerController.OnFishTransferredToFishWorker();
					DecreaseFishCount(1);
					yield return new WaitForSeconds(_timePerFish);
				}
				else
				{
					break;
				}
			}
			_isFishCollectionCompletedShack = true;
		}

		private IEnumerator StartFishCollectFromDockWorker(DockWorkerController dockWorkerController)
		{
			while (dockWorkerController.CollectedFishCount > 0)
			{
				dockWorkerController.OnFishTransferedFishShack();
				IncreaseFishCount(1);
				yield return new WaitForSeconds(0.02f);
			}
		}

		private void UpdateFishCountText()
		{
			shackFishCountText.text = $" {_shackFishCount}";
		}

		private void OnFishWorkerArrivedBoxAction(FishWorkerController fishWorkerController)
		{
			StartCoroutine(StartFishTransferFromFishWorker(fishWorkerController));
		}

		private void OnFishWorkerArrivedShackAction(DockWorkerController dockWorkerController)
		{
			StartCoroutine(StartFishCollectFromDockWorker(dockWorkerController));
		}

		private void DecreaseFishCount(int amount)
		{
			_shackFishCount -= amount;
			UpdateFishCountText();
		}

		private void IncreaseFishCount(int amount)
		{
			_shackFishCount += amount;
			UpdateFishCountText();
		}
	}

}
