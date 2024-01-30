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
		private bool _isBusy = false;
		private bool hasAnimationPlayed = false;

		public Image dockWorkerBarImage;
		public Image circularProgressBar;

		private Sequence _dockWorkerDownAnimation;

		public GameObject dockWorkerFishPanel;
		public TextMeshProUGUI dockWorkerFishCountText;
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
			if (FishBoxController.Instance.HasFishBox && !hasAnimationPlayed && DockWorkerState == DockWorkerState.Idle)
			{
				AnimateDockWorkerDown();
				hasAnimationPlayed = true;
			}
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
					Debug.Log("1");
					dockWorkerBarImage.gameObject.SetActive(true);
					Debug.Log("2");
					dockWorkerFishPanel.gameObject.SetActive(true);
					Debug.Log("3");
					FishBoxController.OnDockWorkerArrivedBox?.Invoke(this);
					Debug.Log("4");
					break;
				case DockWorkerState.ReturningFromCollectingFish:
					dockWorkerBarImage.gameObject.SetActive(false);
					circularProgressBar.fillAmount = 0f;

					transform.DOMove(_initialPosition, speed).OnComplete(() =>
					{
						ShackController.OnDockWorkerArrivedShack?.Invoke(this);
					});
					break;
			}
		}


		public void OnFishCollectedFishBox()
		{
			_collectedFishCount++;
			UpdateFishCountText(_collectedFishCount);

			shackUpgrade = ShackUpgradeManager.Instance.GetShackUpgrade();
			int maxCapacity = shackUpgrade.ReturnDockWorkerFishCapacity();

			var currentProgress = (float)_collectedFishCount / maxCapacity;

			circularProgressBar.fillAmount = currentProgress;

		
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

