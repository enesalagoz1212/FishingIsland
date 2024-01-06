using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

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

		private void GoToSellFish()
		{

			Vector3[] targetPositions = new Vector3[]
			{
				new Vector3(-15f, 0f, -13f),
				new Vector3(8f, 0f, -13f),
				new Vector3(8f, 0f, 0f),
			};

			transform.DOPath(targetPositions, 5f, PathType.CatmullRom)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					StartCoroutine(SellFish());

				});

		}

		private void ReturnToInitialPoint()
		{
			Vector3[] targetPositions = new Vector3[]
			{
				new Vector3(8f, 0f, 8f),
				new Vector3(-15f, 0f, 8f),
				new Vector3(-15f, 0f, 0f),
			};
			transform.DOPath(targetPositions, 5f, PathType.CatmullRom)
				.SetEase(Ease.Linear)
				.OnComplete(() => ChangeState(FishWorkerState.Idle));
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
