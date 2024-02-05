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
		private HouseUpgrade _houseUpgrade;

		private Vector3 _initialFishWorkerDownPosition;
		private int _fishCapacity;
		private int _totalFishCount;
		private int _fishPrice = 5;
		private bool _isSellingFish = false;
		private bool hasAnimationPlayed = false;
		private float _fishWorkerSpeed;

		private GameObject fishWorker;
		private List<GameObject> fishWorkerPrefabs;

		private Sequence _fishWorkerDownAnimation;

		public GameObject fishWorkerFishPanel;

		public TextMeshProUGUI totalMoneyText;
		public TextMeshProUGUI fishWorkerFishText;
		public Image fishWorkerDownOkImage;
		public Image fishWorkerBarImage;
		public Image circularProgressBar;

		public override void Initialize(string name, float speed, int initialCapacity)
		{
			base.Initialize(name, speed, initialCapacity);
		}

		private void OnEnable()
		{
			GameManager.OnButtonClickedHouseUpgrade += OnButtonClickedHouseUpgradeAction;
		}

		private void OnDisable()
		{
			GameManager.OnButtonClickedHouseUpgrade -= OnButtonClickedHouseUpgradeAction;

		}
		public override void Start()
		{
			_houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			fishWorkerPrefabs = _houseUpgrade.GetFishWorkerGameObjects();
			Instantiate(fishWorkerPrefabs[0]);
			ChangeState(FishWorkerState.Idle);
			GameManager.OnButtonClickedHouseUpgrade?.Invoke();
		}

		private void Update()
		{
			if (ShackController.Instance.HasFishShack && !hasAnimationPlayed && FishWorkerState == FishWorkerState.Idle)
			{
				AnimateFishWorkerDown();
				hasAnimationPlayed = true;
			}
		}


		private void Instantiate(GameObject fishWorkerPrefab)
		{
			if (fishWorker != null)
			{
				Destroy(fishWorker);
			}

			fishWorker = Instantiate(fishWorkerPrefab, transform.position, Quaternion.identity, transform);
		}

		private void OnButtonClickedHouseUpgradeAction()
		{
			int fishWorkerLevel = _houseUpgrade.houseUpgradeData.fishWorkerLevel;
			int speedLevel = _houseUpgrade.houseUpgradeData.speedLevel;
			int capacityLevel = _houseUpgrade.houseUpgradeData.capacityLevel;

			if (fishWorkerLevel >= 10 && speedLevel >= 10 && capacityLevel >= 10)
			{
				Destroy(fishWorker);
				Instantiate(fishWorkerPrefabs[1]);
			}
		}

		public override void WorkerMouseDown()
		{
			if (!_isSellingFish && ShackController.Instance.HasFishShack)
			{
				KillFishWorkerDownAnimation();
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
					hasAnimationPlayed = false;
					_totalFishCount = 0;
					fishWorkerFishPanel.gameObject.SetActive(false);
					_isSellingFish = false;
					break;
				case FishWorkerState.CollectingFish:
					fishWorkerDownOkImage.gameObject.SetActive(false);
					fishWorkerBarImage.gameObject.SetActive(true);
					fishWorkerFishPanel.gameObject.SetActive(true);
					ShackController.OnFishWorkerArrivedBox?.Invoke(this);
					break;
				case FishWorkerState.GoingToSellFish:
					fishWorkerBarImage.gameObject.SetActive(false);
					circularProgressBar.fillAmount = 0f;
					GoToSellFish();
					break;
				case FishWorkerState.ReturnsFromSellingFish:
					ReturnToInitialPoint();
					break;
			}
		}

		public void AnimateFishWorkerDown()
		{
			fishWorkerDownOkImage.gameObject.SetActive(true);

			float animationDistance = 0.6f;
			Vector3 initialPosition = fishWorkerDownOkImage.rectTransform.localPosition;
			_initialFishWorkerDownPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y - animationDistance, initialPosition.z);

			_fishWorkerDownAnimation = DOTween.Sequence();
			_fishWorkerDownAnimation.Append(fishWorkerDownOkImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_fishWorkerDownAnimation.Append(fishWorkerDownOkImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

			_fishWorkerDownAnimation.SetLoops(-1, LoopType.Yoyo);

		}

		private void KillFishWorkerDownAnimation()
		{
			if (_fishWorkerDownAnimation != null && _fishWorkerDownAnimation.IsActive())
			{
				_fishWorkerDownAnimation.Kill();
			}
			fishWorkerDownOkImage.rectTransform.anchoredPosition = _initialFishWorkerDownPosition;
		}


		public void OnFishTransferredToFishWorker()
		{

			_fishCapacity = _houseUpgrade.ReturnFishWorkerFishCapacity();

			_totalFishCount++;
			UpdateFishCountText(_totalFishCount);

			var currentProgress = (float)_totalFishCount / _fishCapacity;

			circularProgressBar.fillAmount = currentProgress;
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
				_houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
				_fishWorkerSpeed = _houseUpgrade.UpdateDockUpgradeFishWorkerLevelSpeed(_houseUpgrade.houseUpgradeData.fishWorkerLevel);

				Vector3 currentPosition = transform.position;
				Sequence pathSequence = DOTween.Sequence();

				pathSequence.Append(transform.DOMove(pathList[0].position, _fishWorkerSpeed).SetEase(Ease.Linear));

				for (int i = 1; i < pathList.Count; i++)
				{
					pathSequence.Append(transform.DOMove(pathList[i].position, _fishWorkerSpeed).SetEase(Ease.Linear));
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

		private void UpdateFishWorkerSpeed(float newSpeed)
		{
			_fishWorkerSpeed = newSpeed;
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
				yield return new WaitForSeconds(0.01f);
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
