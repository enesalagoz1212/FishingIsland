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
		}

		private void OnMouseDown()
		{
			Debug.Log("ShackController");
			shackUpgradeCanvas.SetActive(true);
		}

		public IEnumerator CollectFish(int fishCount)
		{
			for (int i = 0; i < fishCount; i++)
			{
				yield return new WaitForSeconds(0.3f);
				_shackFishCount++;
				UpdateFishCountText();
			}
		}

		public IEnumerator TransferFish(int capasity)
		{
			for (int i = 1; i <= capasity; i++)
			{
				yield return new WaitForSeconds(0.3f);
				_shackFishCount--;
				UpdateFishCountText();
			}

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
		private void UpdateFishCountText()
		{
			shackFishCountText.text = $" {_shackFishCount}";
		}

		private void OnFishWorkerArrivedBoxAction(FishWorkerController fishWorkerController)
		{
			StartCoroutine(StartFishTransferFromFishWorker(fishWorkerController));
		}

		private void DecreaseFishCount(int amount)
		{
			_shackFishCount -= amount;
			UpdateFishCountText();
		}
	}

}
