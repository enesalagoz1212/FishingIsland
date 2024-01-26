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

		private Vector3 _initialPosition;
		private Vector3 _targetPosition;
		private Vector3 _initialDockWorkerDownPosition;
		private int _collectedFishCount = 0;
		private int _dockCapacity;
		private float _currentTimeDuration;
		private bool _isBusy = false;
		private bool hasAnimationPlayed = false;

		private Sequence _dockWorkerDownAnimation;
		
		public GameObject timerPanel;
		public GameObject dockWorkerFishPanel;
		public TextMeshProUGUI dockWorkerFishCountText;
		public TextMeshProUGUI dockWorkerTimerText;
		public Image dockWorkerDownOkImage;
		public float speed = 1f;
		public int CollectedFishCount
		{
			get { return _collectedFishCount; }
		}

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
					Debug.Log("if blogu calisti");
					OnFishCollectionCompleted();
				}
				else
				{
					Debug.Log("else blogu calisti");
					_currentTimeDuration -= Time.deltaTime;
					UpdateTimerDurationText();
				}
			}

			if (FishBoxController.Instance.HasFishBox && !hasAnimationPlayed && DockWorkerState == DockWorkerState.Idle)
			{
				AnimateDockWorkerDown();
				hasAnimationPlayed = true;
			}
		}

		public float GetCurrentTimerDuration()
		{
			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			_currentTimeDuration = shackUpgrade.TimerLevelIncrease();
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
				KillDockWorkerDownAnimation();
				ChangeState(DockWorkerState.GoToCollectFish);
				_isBusy = true;
			}
		}

		public void AnimateDockWorkerDown()
		{
			dockWorkerDownOkImage.gameObject.SetActive(true);

			float animationDistance = 0.6f;
			Vector3 initialPosition = dockWorkerDownOkImage.rectTransform.localPosition;
			_initialDockWorkerDownPosition = initialPosition;

			Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y - animationDistance, initialPosition.z);

			_dockWorkerDownAnimation = DOTween.Sequence();
			_dockWorkerDownAnimation.Append(dockWorkerDownOkImage.rectTransform.DOLocalMove(targetPosition, 0.7f).SetEase(Ease.OutQuad));
			_dockWorkerDownAnimation.Append(dockWorkerDownOkImage.rectTransform.DOLocalMove(initialPosition, 0.7f).SetEase(Ease.InQuad));

			_dockWorkerDownAnimation.SetLoops(-1, LoopType.Yoyo);
		}

		private void KillDockWorkerDownAnimation()
		{
			if (_dockWorkerDownAnimation != null && _dockWorkerDownAnimation.IsActive())
			{
				_dockWorkerDownAnimation.Kill();
			}
			dockWorkerDownOkImage.rectTransform.anchoredPosition = _initialDockWorkerDownPosition;
		}

		public void ChangeState(DockWorkerState state)
		{
			DockWorkerState = state;
			switch (DockWorkerState)
			{
				case DockWorkerState.Idle:
					hasAnimationPlayed = false;
					_currentTimeDuration = GetCurrentTimerDuration();
					_collectedFishCount = 0;
					_isBusy = false;
					dockWorkerFishPanel.gameObject.SetActive(false);
					break;
				case DockWorkerState.GoToCollectFish:
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
			_collectedFishCount++;
			UpdateFishCountText(_collectedFishCount);
		}

		public void OnFishCollectionCompleted()          
		{
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

