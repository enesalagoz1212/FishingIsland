using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace FishingIsland.Controllers
{
	public class ShackController : MonoBehaviour
	{
		public static ShackController Instance;

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
			OnFishWorkerArrivedBox += OnFishWorkerArrivedBoxAction;
			OnDockWorkerArrivedShack += OnFishWorkerArrivedShackAction;
		}

		private void OnMouseDown()
		{
			Debug.Log("ShackController");
			shackUpgradeCanvas.SetActive(true);
		}


		private IEnumerator StartFishTransferFromFishWorker(FishWorkerController fishWorkerController)
		{
			while (_shackFishCount > 0)
			{
				fishWorkerController.OnFishTransferredToFishWorker();
				DecreaseFishCount(1);
				yield return new WaitForSeconds(0.2f);
			}
		}

		private IEnumerator StartFishCollectFromDockWorker(DockWorkerController dockWorkerController)
		{
			while (dockWorkerController.CollectedFishCount >= 0)
			{
				dockWorkerController.OnFishTransferedFishShack();
				_shackFishCount++;
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
