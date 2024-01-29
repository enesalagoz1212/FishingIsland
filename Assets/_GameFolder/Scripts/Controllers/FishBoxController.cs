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
		private float _currentProgress;
		private bool _isFishCollectionCompletedBox = false;

		public TextMeshProUGUI boxFishText;
		public Image dockWorkerBarImage;
		public Image circularProgressBar;
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
			dockWorkerBarImage.gameObject.SetActive(true);
			_isFishCollectionCompletedBox = false;
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_dockCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();



			float oneFishGatherSpeed = shackUpgrade.UpdateShackUpgradeSpeed(shackUpgrade.shackUpgradeData.speedLevel);
			float oneFishGatherTime;
			float timer = 0f;

			while (_fishCount < shackUpgrade.ReturnDockWorkerFishCapacity() && HasFishBox && !_isFishCollectionCompletedBox)
			{
				if (oneFishGatherSpeed != shackUpgrade.UpdateShackUpgradeSpeed(shackUpgrade.shackUpgradeData.speedLevel))
				{
					oneFishGatherSpeed = shackUpgrade.UpdateShackUpgradeSpeed(shackUpgrade.shackUpgradeData.speedLevel);
				}

				oneFishGatherTime = 1 / oneFishGatherSpeed;
				timer += Time.deltaTime;

				if (timer >= oneFishGatherTime)
				{
					dockWorkerController.OnFishCollectedFishBox();
					DecreaseFishCount(1);
					_fishCount++;

					UpdateCircularProgressBar();
					timer = 0f;
				}

				if (_fishCount >= shackUpgrade.ReturnDockWorkerFishCapacity() || !HasFishBox)
				{
					dockWorkerController.OnFishCollectionCompleted();
				}
				yield return null;

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

		private void UpdateCircularProgressBar()
		{
			if (circularProgressBar != null)
			{
				shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
				int maxCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();
				
				if (maxCapacity >=_totalFishCount )
				{
					maxCapacity =_totalFishCount;
				}				

				_currentProgress = (float)_fishCount / maxCapacity;

				circularProgressBar.fillAmount = _currentProgress;

				if (circularProgressBar.fillAmount >= 1f)
				{
					dockWorkerBarImage.gameObject.SetActive(false);
					circularProgressBar.fillAmount = 0f;
				}
			}

		}
	}
}

