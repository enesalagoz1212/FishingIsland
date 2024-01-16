using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using FishingIsland.Managers;
using FishingIsland.UpgradeScriptableObjects;

namespace FishingIsland.Controllers
{
	public enum DockWorkerState
	{
		Idle,
		GoToCollectFish,
		CollectingFish,
		ReturningFromCollectingFish,
	}

	public class DockWorkerController : BaseCharacterController
	{
		public DockWorkerState DockWorkerState { get; private set; }
		private ShackUpgrade shackUpgrade;
		public float speed = 1f;
		public TextMeshProUGUI dockWorkerFishCountText;
		private Vector3 _initialPosition;
		private Vector3 _targetPosition;
		private Vector3 _initialDockWorkerDownPosition;
		public GameObject dockWorkerFishPanel;
		private int _collectedFishCount = 0;

		private int _dockCapacity;
		public int CollectedFishCount
		{
			get { return _collectedFishCount; }
		}

		public Image dockWorkerDownOkImage;
		private bool _isBusy = false;

		private float _currentTimeDuration;
		public TextMeshProUGUI dockWorkerTimerText;
		public GameObject timerPanel;
		private Sequence _dockWorkerDownAnimation;
		public override void Initialize(string name, float initialSpeed, int initialCapacity)
		{
			characterName = name;
			speed = initialSpeed;
			capacity = initialCapacity;
		}

		public override void Start()
		{
			_initialPosition = transform.position;
			_targetPosition = LevelManager.Instance.dockWorkerGoesFishing[0].position;
			ChangeState(DockWorkerState.Idle);
		}

		private void Update()
		{
			if (DockWorkerState == DockWorkerState.CollectingFish)
			{
				if (_currentTimeDuration <= 0 && FishBoxController.Instance.IsFishCollectionCompleted)
				{
					OnFishCollectionCompleted();
				}
				else
				{
					_currentTimeDuration -= Time.deltaTime;
					UpdateTimerDurationText();
				}
			}

			if (DockWorkerState == DockWorkerState.Idle)
			{
				if (FishBoxController.Instance.HasFishBox)
				{
					dockWorkerDownOkImage.gameObject.SetActive(true);
					AnimateDockWorkerDown();
				}
				else
				{
					KillBoatDownAnimation();
					dockWorkerDownOkImage.gameObject.SetActive(false);
				}
			}
		}

		public float GetCurrentTimerDuration()
		{
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_currentTimeDuration = shackUpgrade.initialTimerDurationFishWorker;
			return _currentTimeDuration;
		}

		public void UpdateTimerDurationText()
		{
			dockWorkerTimerText.text = $" {(int)_currentTimeDuration}s";
		}

		public override void WorkerMouseDown()
		{
			if (!_isBusy && FishBoxController.Instance.HasFishBox)
			{
				KillBoatDownAnimation();
				ChangeState(DockWorkerState.GoToCollectFish);
				_isBusy = true;
			}
		}

		private void AnimateDockWorkerDown()
		{
			float animationDistance = 0.7f;
			Vector3 initialPosition = dockWorkerDownOkImage.rectTransform.localPosition;
			_initialDockWorkerDownPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y - animationDistance, initialPosition.z);

			_dockWorkerDownAnimation = DOTween.Sequence();
			_dockWorkerDownAnimation.Append(dockWorkerDownOkImage.rectTransform.DOLocalMove(targetPosition, 1.0f).SetEase(Ease.OutQuad));
			_dockWorkerDownAnimation.Append(dockWorkerDownOkImage.rectTransform.DOLocalMove(initialPosition, 1.0f).SetEase(Ease.InQuad));

			_dockWorkerDownAnimation.SetLoops(-1, LoopType.Yoyo);
		}

		private void KillBoatDownAnimation()
		{
			if (_dockWorkerDownAnimation != null && _dockWorkerDownAnimation.IsActive())
			{
				_dockWorkerDownAnimation.Kill();
			}
			dockWorkerDownOkImage.rectTransform.anchoredPosition = new Vector3(-560.8f, -1165.5f, 4.468f);
		}

		public void ChangeState(DockWorkerState state)
		{
			DockWorkerState = state;
			//Debug.Log($"DockWorkerState: {state}");
			switch (DockWorkerState)
			{
				case DockWorkerState.Idle:
					//if (FishBoxController.Instance.HasFishBox)
					//{
					//	Debug.Log(FishBoxController.Instance.HasFishBox);
					//	dockWorkerDownOkImage.gameObject.SetActive(true);
					//	AnimateDockWorkerDown();
					//}
					Debug.Log(FishBoxController.Instance.HasFishBox);
					_currentTimeDuration = GetCurrentTimerDuration();
					//dockWorkerDownOkImage.gameObject.SetActive(true);
					_collectedFishCount = 0;
					_isBusy = false;
					dockWorkerFishPanel.gameObject.SetActive(false);
					break;
				case DockWorkerState.GoToCollectFish:
					Debug.Log("GotoCollectFish");
					dockWorkerDownOkImage.gameObject.SetActive(false);
					transform.DOMove(_targetPosition, speed).OnComplete(() =>
					{
						ChangeState(DockWorkerState.CollectingFish);
					});
					break;
				case DockWorkerState.CollectingFish:
					timerPanel.SetActive(true);
					dockWorkerFishPanel.gameObject.SetActive(true);
					FishBoxController.OnDockWorkerArrivedBox?.Invoke(this);
					break;
				case DockWorkerState.ReturningFromCollectingFish:
					transform.DOMove(_initialPosition, speed).OnComplete(() =>
					{
						timerPanel.SetActive(false);
						ShackController.OnDockWorkerArrivedShack?.Invoke(this);
					});
					break;
			}
		}


		public void OnFishCollectedFishBox()
		{
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_dockCapacity = shackUpgrade.dockWorkerFishCapacity;

			_collectedFishCount++;
			UpdateFishCountText(_collectedFishCount);
		}

		public void OnFishCollectionCompleted()
		{
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_currentTimeDuration = shackUpgrade.initialTimerDurationFishWorker;

			ChangeState(DockWorkerState.ReturningFromCollectingFish);
		}

		public void OnFishTransferedFishShack()
		{
			_collectedFishCount--;
			UpdateFishCountText(_collectedFishCount);

			if (_collectedFishCount <= 0)
			{
				ChangeState(DockWorkerState.Idle);
			}
		}

		private void UpdateFishCountText(int collectedFishCount)
		{
			dockWorkerFishCountText.text = $" {collectedFishCount}";
		}
	}
}

