using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FishingIsland.UpgradeScriptableObjects;
using FishingIsland.Managers;
using UnityEngine.UI;

namespace FishingIsland.Controllers
{
	public class FishBoxController : MonoBehaviour
	{
		public static FishBoxController Instance;
		private ShackUpgrade shackUpgrade;

		private int _dockCapacity;
		private int _totalFishCount;
		private int _fishCount = 0;
		
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
			Debug.Log("5");
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
			Debug.Log("6");
			_isFishCollectionCompletedBox = false;
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_dockCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();
			Debug.Log("7");


			float oneFishGatherSpeed = shackUpgrade.UpdateShackUpgradeSpeed(shackUpgrade.shackUpgradeData.speedLevel);
			float oneFishGatherTime;
			float timer = 0f;

			Debug.Log("8");

			while (_fishCount < shackUpgrade.ReturnDockWorkerFishCapacity() && HasFishBox && !_isFishCollectionCompletedBox)
			{
				Debug.Log("9");
				if (oneFishGatherSpeed != shackUpgrade.UpdateShackUpgradeSpeed(shackUpgrade.shackUpgradeData.speedLevel))
				{
					Debug.Log("10");
					oneFishGatherSpeed = shackUpgrade.UpdateShackUpgradeSpeed(shackUpgrade.shackUpgradeData.speedLevel);
				}

				oneFishGatherTime = 1 / oneFishGatherSpeed;
				timer += Time.deltaTime;

				Debug.Log("11");
				if (timer >= oneFishGatherTime)
				{
					Debug.Log("12");
					dockWorkerController.OnFishCollectedFishBox();
					DecreaseFishCount(1);
					_fishCount++;
					Debug.Log("13");
					timer = 0f;
				}

				if (_fishCount >= shackUpgrade.ReturnDockWorkerFishCapacity() || !HasFishBox)
				{
					Debug.Log("14");
					dockWorkerController.OnFishCollectionCompleted();
				}
				yield return null;

			}
			_fishCount = 0;
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

