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
		private int _capacity = 10;
		private int _fishPrice = 5;
		private int _totalMoney;
		public TextMeshProUGUI totalMoneyText;
		public Image fishWorkerDownOkImage;
		public GameObject fishWorkerFishPanel;

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
					_capacity = 10;
					fishWorkerDownOkImage.gameObject.SetActive(true);
					fishWorkerFishPanel.gameObject.SetActive(false);
					_isSellingFish = false;
					Debug.Log($"isSellingFish: {_isSellingFish}");
					break;
				case FishWorkerState.CollectingFish:
					fishWorkerDownOkImage.gameObject.SetActive(false);
					fishWorkerFishPanel.gameObject.SetActive(true);
					StartCoroutine(ShackController.Instance.TransferFish(_capacity));
					StartCoroutine(CollectFish());
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

		public IEnumerator CollectFish()
		{

			for (int i = 0; i < _capacity; i++)
			{
				yield return new WaitForSeconds(0.3f);
				_totalFishCount++;
				UpdateFishCountText(_totalFishCount);
			}
			ChangeState(FishWorkerState.GoingToSellFish);
		}

		public IEnumerator TransferFish()
		{
			while (_capacity > 0)
			{
				yield return new WaitForSeconds(0.3f);
				_capacity--;
				UpdateFishCountText(_capacity);
			}
			_capacity = 10;
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
		{
			MoveOnPath(LevelManager.Instance.fishWorkerSellPath, () => StartCoroutine(SellFish()));
		}

		private void ReturnToInitialPoint()
		{
			MoveOnPath(LevelManager.Instance.fishWorkerReturnPath, () => ChangeState(FishWorkerState.Idle));
		}

		private IEnumerator SellFish()
		{
			int earnedMoney = _totalFishCount * _fishPrice;
			_totalMoney += earnedMoney;
			UpdateTotalMoneyText(_totalMoney);

			Debug.Log($"Earned money: {earnedMoney}, Total money: {_totalMoney}");
			StartCoroutine(TransferFish());
			yield return new WaitForSeconds(3f);

			ChangeState(FishWorkerState.ReturnsFromSellingFish);
		}

		private void UpdateTotalMoneyText(int totalMoney)
		{
			totalMoneyText.text = $" {totalMoney}";
		}
	}
}
