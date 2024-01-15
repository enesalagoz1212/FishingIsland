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
		public bool HasFishShack => _shackFishCount > 0;

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

			for (int i = 0; i < _fishCapacity; i++)
			{
				fishWorkerController.OnFishTransferredToFishWorker();
				DecreaseFishCount(1);
				yield return new WaitForSeconds(0.1f);
			}
		}

		private IEnumerator StartFishCollectFromDockWorker(DockWorkerController dockWorkerController)
		{
			while (dockWorkerController.CollectedFishCount > 0)
			{
				dockWorkerController.OnFishTransferedFishShack();
				IncreaseFishCount(1);
				yield return new WaitForSeconds(0.2f);
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
