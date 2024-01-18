using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using FishingIsland.UpgradeScriptableObjects;

namespace FishingIsland.Controllers
{
	public enum BoatState
	{
		InThePort,
		GoingFishing,
		Fishing,
		ReturningToPort,
	}
	public class BoatController : MonoBehaviour
	{
		public BoatState BoatState { get; private set; }
		private DockUpgrade dockUpgrade;

		private Vector3 _initialPosition;
		private Vector3 _initialBoatDownPosition;
		private float _currentTimerDuration;
		private int _boatFishCapacity;
		private bool _canClick = true;
		public int FishCount { get; private set; }
		public GameObject dockTimerPanel;
		public GameObject boatFishPanel;
		public Image boatDownOkImage;
		public TextMeshProUGUI timerText;
		public TextMeshProUGUI fishCapacityText;
		public TextMeshProUGUI boxFishText;

		private Sequence _boatDownAnimation;
		public void Initialize()
		{
			_initialPosition = transform.position;
			ChangeState(BoatState.InThePort);
		}

		public void ChangeState(BoatState boatState)
		{
			BoatState = boatState;
			switch (BoatState)
			{
				case BoatState.InThePort:
					AnimateBoatDown();
					transform.position = _initialPosition;
					boatFishPanel.gameObject.SetActive(false);
					_canClick = true;
					break;
				case BoatState.GoingFishing:
					Transform randomSpawnPoint = LevelManager.Instance.GetRandomBoatSpawnPoint();
					if (randomSpawnPoint != null)
					{
						MoveToPosition(randomSpawnPoint.position, 2f, () =>
						 {
							 ChangeState(BoatState.Fishing);
						 });
					}
					break;
				case BoatState.Fishing:
					UpdateCollectTheFishText();
					break;
				case BoatState.ReturningToPort:
					MoveToPosition(_initialPosition, 1f, () =>
					 {
						 FishBoxController.OnBoatArrivedBox?.Invoke(this);
						 dockTimerPanel.gameObject.SetActive(false);
					 });
					break;
			}
		}

		private void Update()
		{
			TimerControlBoat();
		}

		private void AnimateBoatDown()
		{
			boatDownOkImage.gameObject.SetActive(true);

			float animationDistance = 0.6f;
			Vector3 initialPosition = boatDownOkImage.rectTransform.localPosition;
			_initialBoatDownPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y - animationDistance, initialPosition.z);

			_boatDownAnimation = DOTween.Sequence();
			_boatDownAnimation.Append(boatDownOkImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_boatDownAnimation.Append(boatDownOkImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

			_boatDownAnimation.SetLoops(-1, LoopType.Yoyo);
		}

		private void KillBoatDownAnimation()
		{
			if (_boatDownAnimation != null && _boatDownAnimation.IsActive())
			{
				_boatDownAnimation.Kill();
			}
			boatDownOkImage.rectTransform.anchoredPosition = _initialBoatDownPosition;
		}

		private void MoveToPosition(Vector3 targetPosition, float duration, Action onComplete)
		{
			transform.DOMove(targetPosition, duration).OnComplete(() =>
			{
				onComplete?.Invoke();
			});
		}

		private void OnMouseDown()
		{
			if (_canClick)
			{
				ChangeState(BoatState.GoingFishing);
				KillBoatDownAnimation();
				boatDownOkImage.gameObject.SetActive(false);
				_canClick = false;
			}
		}

		public void OnFishTransferredToFishBox()
		{
			dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
			_boatFishCapacity = dockUpgrade.boatFishCapacity;
			if (FishCount > 0)
			{
				FishCount--;
				UpdateFishCapacityText(FishCount);
			}

			if (FishCount <= 0)
			{
				ChangeState(BoatState.InThePort);
			}
		}

		private void UpdateFishCapacityText(int fishAmount)
		{
			fishCapacityText.text = $" {fishAmount}";
		}

		private void UpdateCollectTheFishText()
		{
			StartCoroutine(CollectFish());
		}

		private void TimerControlBoat()
		{
			if (BoatState == BoatState.Fishing)
			{
				dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();

				if (_currentTimerDuration <= 0f && FishCount >= _boatFishCapacity)
				{
					ChangeState(BoatState.ReturningToPort);
				}
				else
				{
					_currentTimerDuration -= Time.deltaTime;
					UpdateTimerText();
				}
			}
		}

		private void UpdateTimerText()
		{
			timerText.text = $" {(int)_currentTimerDuration}s";
		}

		private IEnumerator CollectFish()
		{
			dockTimerPanel.gameObject.SetActive(true);
			dockUpgrade = DockUpgradeManager.Instance.GetDockUpgrade();
			_boatFishCapacity = dockUpgrade.boatFishCapacity;

			_currentTimerDuration = dockUpgrade.initialTimerDurationBoat;
			boatFishPanel.SetActive(true);
			float timePerFish = _currentTimerDuration / _boatFishCapacity;

			for (int i = 0; i < _boatFishCapacity; i++)
			{
				yield return new WaitForSeconds(timePerFish);
				FishCount++;
				UpdateFishCapacityText(FishCount);
			}
		}
	}
}