using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FishingIsland.UpgradeScriptableObjects;
using FishingIsland.Managers;
using UnityEngine.UI;
using DG.Tweening;

namespace FishingIsland.Controllers
{
	public class ShackController : MonoBehaviour
	{
		public static ShackController Instance;
		private HouseUpgrade _houseUpgrade;
		private ShackUpgrade _shackUpgrade;
		private int _fishCapacity;

		public GameObject shackUpgradeCanvas;
		public TextMeshProUGUI shackFishCountText;
		private int _shackFishCount = 0;
		private float _timePerFish;
		public bool HasFishShack => _shackFishCount > 0;

		private bool _isFishCollectionCompletedShack = false;
		public bool IsFishCollectionCompleted => _isFishCollectionCompletedShack;
		public Image shackUpImage;
		private Sequence _shackUpAnimation;
		private Vector3 _initialShackUpPosition;
		private bool hasAnimationPlayed = false;

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
			GameManager.OnCloseButton += OnCloseButtonAction;
		}

		private void OnDisable()
		{
			OnFishWorkerArrivedBox -= OnFishWorkerArrivedBoxAction;
			OnDockWorkerArrivedShack -= OnFishWorkerArrivedShackAction;
			GameManager.OnCloseButton -= OnCloseButtonAction;
		}

		private void Update()
		{
			if (CanUpgradeShack() && !hasAnimationPlayed)
			{
				AnimateShackUp();
				hasAnimationPlayed = true;
			}
			else
			{
				
			}
		}


		private void OnMouseDown()
		{
			if (CanUpgradeShack())
			{
				shackUpImage.gameObject.SetActive(false);
				KillShackUpAnimation();
				shackUpgradeCanvas.SetActive(true);
			}

		}

		private void OnCloseButtonAction()
		{
			ResetAnimation();
			if (!CanUpgradeShack())
			{
				shackUpImage.gameObject.SetActive(false);
			}
		}

		private IEnumerator StartFishTransferFromFishWorker(FishWorkerController fishWorkerController)
		{
			_houseUpgrade = HouseUpgradeManager.Instance.GetHouseUpgrade();
			_fishCapacity = _houseUpgrade.fishWorkerFishCapacity;
			_isFishCollectionCompletedShack = false;

			if (_shackFishCount > _fishCapacity)
			{
				_timePerFish = fishWorkerController.GetCurrentTimerDuration() / _fishCapacity;
			}
			else
			{
				_timePerFish = fishWorkerController.GetCurrentTimerDuration() / _shackFishCount;
			}


			for (int i = 0; i < _fishCapacity; i++)
			{
				if (HasFishShack)
				{
					fishWorkerController.OnFishTransferredToFishWorker();
					DecreaseFishCount(1);
					yield return new WaitForSeconds(_timePerFish);
				}
				else
				{
					break;
				}
			}
			_isFishCollectionCompletedShack = true;
		}

		private IEnumerator StartFishCollectFromDockWorker(DockWorkerController dockWorkerController)
		{
			while (dockWorkerController.CollectedFishCount > 0)
			{
				dockWorkerController.OnFishTransferedFishShack();
				IncreaseFishCount(1);
				yield return new WaitForSeconds(0.02f);
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

		public void AnimateShackUp()
		{
			shackUpImage.gameObject.SetActive(true);

			float animationDistance = 0.7f;
			Vector3 initialPosition = shackUpImage.rectTransform.localPosition;
			_initialShackUpPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + animationDistance, initialPosition.z);

			_shackUpAnimation = DOTween.Sequence();
			_shackUpAnimation.Append(shackUpImage.rectTransform.DOLocalMove(targetPosition, 1.0f).SetEase(Ease.OutQuad));
			_shackUpAnimation.Append(shackUpImage.rectTransform.DOLocalMove(initialPosition, 1.0f).SetEase(Ease.InQuad));

			_shackUpAnimation.SetLoops(-1, LoopType.Yoyo);

		}

		private void KillShackUpAnimation()
		{
			if (_shackUpAnimation != null && _shackUpAnimation.IsActive())
			{
				_shackUpAnimation.Kill();
			}
			shackUpImage.rectTransform.anchoredPosition = _initialShackUpPosition;
		}

		private bool CanUpgradeShack()
		{
			_shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			return MoneyManager.Instance.GetMoney() >= _shackUpgrade.dockWorkerLevelUpgradeCost
				|| MoneyManager.Instance.GetMoney() >= _shackUpgrade.timerLevelUpgradeCost
				|| MoneyManager.Instance.GetMoney() >= _shackUpgrade.capacityLevelUpgradeCost;
		}

		public void ResetAnimation()
		{
			hasAnimationPlayed = false;
		}
	}

}
