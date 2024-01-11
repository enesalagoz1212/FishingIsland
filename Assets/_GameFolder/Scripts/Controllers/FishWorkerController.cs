using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using FishingIsland.Managers;
using System;

namespace FishingIsland.Controllers
{
	public enum FishWorkerState
	{
		Idle,
		CollectingFish,
		GoingToSellFish,
		ReturnsFromSellingFish,
	}
	public class FishWorkerController : BaseCharacterController
	{
		public FishWorkerState FishWorkerState { get; private set; }
		public TextMeshProUGUI fishWorkerFishText;
		private int _totalFishCount;
		private int _fishCapacity = 10;
		private int _fishPrice = 5;
		private int _totalMoney;
		public TextMeshProUGUI totalMoneyText;
		public Image fishWorkerDownOkImage;
		public GameObject fishWorkerFishPanel;

		public int FishWorkerFishCapacity
		{
			get { return _fishCapacity; }
			set { _fishCapacity = value; }
		}

		private bool _isSellingFish = false;
		public override void Initialize(string name, float speed, int initialCapacity)
		{
			base.Initialize(name, speed, initialCapacity);
		}

		public override void Start()
		{
			ChangeState(FishWorkerState.Idle);
		}

		public override void WorkerMouseDown()
		{
			if (!_isSellingFish && ShackController.Instance.HasFishShack)
			{
				ChangeState(FishWorkerState.CollectingFish);
				_isSellingFish = true;
			}
		}

		public void ChangeState(FishWorkerState state)
		{
			FishWorkerState = state;

			switch (FishWorkerState)
			{
				case FishWorkerState.Idle:
					_totalFishCount = 0;
					_fishCapacity = 10;
					fishWorkerDownOkImage.gameObject.SetActive(true);
					fishWorkerFishPanel.gameObject.SetActive(false);
					_isSellingFish = false;
					Debug.Log($"isSellingFish: {_isSellingFish}");
					break;
				case FishWorkerState.CollectingFish:
					fishWorkerDownOkImage.gameObject.SetActive(false);
					fishWorkerFishPanel.gameObject.SetActive(true);

					ShackController.OnFishWorkerArrivedBox?.Invoke(this);

					break;
				case FishWorkerState.GoingToSellFish:
					Debug.Log("GointToSellFish");
					GoToSellFish();
					break;
				case FishWorkerState.ReturnsFromSellingFish:
					ReturnToInitialPoint();

					break;
			}

		}


		public IEnumerator TransferFish()
		{
			while (_fishCapacity > 0)
			{
				yield return new WaitForSeconds(0.3f);
				_fishCapacity--;
				UpdateFishCountText(_fishCapacity);
			}
			_fishCapacity = 10;
		}

		public void OnFishTransferredToFishWorker()
		{
			_totalFishCount++;
			UpdateFishCountText(_totalFishCount);
			if (_totalFishCount >= FishWorkerFishCapacity)
			{
				ChangeState(FishWorkerState.GoingToSellFish);
			}
		}

		private void UpdateFishCountText(int collectedFishCount)
		{
			fishWorkerFishText.text = $" {collectedFishCount}";
		}

		private void MoveOnPath(List<Transform> pathList, Action onCompleteAction)
		{
			if (pathList != null && pathList.Count > 0)
			{
				Vector3 currentPosition = transform.position;
				Sequence pathSequence = DOTween.Sequence();

				pathSequence.Append(transform.DOMove(pathList[0].position, 2f).SetEase(Ease.Linear));

				for (int i = 1; i < pathList.Count; i++)
				{
					pathSequence.Append(transform.DOMove(pathList[i].position, 2f).SetEase(Ease.Linear));
				}

				pathSequence.OnComplete(() => onCompleteAction?.Invoke());
			}
			else
			{
				Debug.LogError("Path is null or empty.");
			}
		}

		private void GoToSellFish()
		{ // E�er fishWorkerSellPath null de�ilse ve en az bir eleman i�eriyorsa devam et
			if (LevelManager.Instance.fishWorkerSellPath != null && LevelManager.Instance.fishWorkerSellPath.Count > 0)
			{
				// Bal�k i��isinin sat�� yolu �zerinde hareket et
				MoveOnPath(LevelManager.Instance.fishWorkerSellPath, () => StartCoroutine(SellFish()));
			}
			else
			{
				// Hata durumunda uygun �ekilde i�lem yap, �rne�in durumu de�i�tir veya bir hata mesaj� yazd�r
				Debug.LogError("fishWorkerSellPath null veya bo�.");
			}
		}

		private void ReturnToInitialPoint()
		{
			MoveOnPath(LevelManager.Instance.fishWorkerReturnPath, () => ChangeState(FishWorkerState.Idle));
		}

		private IEnumerator SellFish()
		{
			int earnedMoney = _totalFishCount * _fishPrice;
			Debug.Log("1");
			MoneyManager.Instance.AddMoney(earnedMoney);
			Debug.Log("2");
			UpdateTotalMoneyText();
			Debug.Log("3");
			Debug.Log($"Earned money: {earnedMoney}, Total money: {_totalMoney}");
			StartCoroutine(TransferFish());
			yield return new WaitForSeconds(3f);

			ChangeState(FishWorkerState.ReturnsFromSellingFish);
		}

		private void UpdateTotalMoneyText()
		{
			totalMoneyText.text = $" {MoneyManager.Instance.GetMoney()}";
		}
	}
}
