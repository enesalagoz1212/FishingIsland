using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using FishingIsland.Managers;
using System;
using FishingIsland.UpgradeScriptableObjects;

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
		private HouseUpgrade houseUpgrade;
		private int _fishCapacity;
		public TextMeshProUGUI fishWorkerFishText;
		private int _totalFishCount;

		private int _fishPrice = 5;
		public TextMeshProUGUI totalMoneyText;
		public Image fishWorkerDownOkImage;
		public GameObject fishWorkerFishPanel;

		private float _currentTimeDuration;
		public TextMeshProUGUI fishWorkerTimerText;
		public GameObject timerPanel;

		private bool _isSellingFish = false;
		public override void Initialize(string name, float speed, int initialCapacity)
		{
			base.Initialize(name, speed, initialCapacity);
		}

		public override void Start()
		{
			ChangeState(FishWorkerState.Idle);
		}


		private void Update()
		{
			if (FishWorkerState == FishWorkerState.CollectingFish)
			{
				if (_currentTimeDuration <= 0 && ShackController.Instance.IsFishCollectionCompleted)
				{
					OnFishCollectionCompleted();
				}
				else
				{
					_currentTimeDuration -= Time.deltaTime;
					UpdateTimerDurationText();
				}
			}
		}


		public float GetCurrentTimerDuration()
		{
			houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			_currentTimeDuration = houseUpgrade.initialTimerDurationHouse;
			return _currentTimeDuration;
		}

		public void UpdateTimerDurationText()
		{
			fishWorkerTimerText.text = $" {(int)_currentTimeDuration}s";
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
					_currentTimeDuration = GetCurrentTimerDuration();
					_totalFishCount = 0;
					fishWorkerDownOkImage.gameObject.SetActive(true);
					fishWorkerFishPanel.gameObject.SetActive(false);
					_isSellingFish = false;
					Debug.Log($"isSellingFish: {_isSellingFish}");
					break;
				case FishWorkerState.CollectingFish:
					timerPanel.SetActive(true);
					fishWorkerDownOkImage.gameObject.SetActive(false);
					fishWorkerFishPanel.gameObject.SetActive(true);

					ShackController.OnFishWorkerArrivedBox?.Invoke(this);

					break;
				case FishWorkerState.GoingToSellFish:
					timerPanel.SetActive(false);
					Debug.Log("GointToSellFish");
					GoToSellFish();
					break;
				case FishWorkerState.ReturnsFromSellingFish:
					ReturnToInitialPoint();
					break;
			}
		}


		public void OnFishTransferredToFishWorker()
		{
			houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			_fishCapacity = houseUpgrade.fishWorkerFishCapacity;

			_totalFishCount++;
			UpdateFishCountText(_totalFishCount);
		}

		public void OnFishCollectionCompleted()
		{
			ChangeState(FishWorkerState.GoingToSellFish);
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
			MoneyManager.Instance.AddMoney(earnedMoney);
			UpdateTotalMoneyText();

			while (_totalFishCount > 0)
			{
				yield return new WaitForSeconds(0.02f);
				_totalFishCount--;
				UpdateFishCountText(_totalFishCount);
			}
			ChangeState(FishWorkerState.ReturnsFromSellingFish);
		}

		private void UpdateTotalMoneyText()
		{
			totalMoneyText.text = $" {MoneyManager.Instance.GetMoney()}";
		}


	}
}
