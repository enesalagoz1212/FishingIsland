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

		public GameObject dockWorkerFishPanel;
		private int _collectedFishCount = 0;

		public int _dockCapacity;

		public int CollectedFishCount
		{
			get { return _collectedFishCount; }
		}
		public int Capacity
		{
			get { return _dockCapacity; }
			set { _dockCapacity = value; }
		}

		public Image dockWorkerDownOkImage;
		private bool _isBusy = false;
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

		public override void WorkerMouseDown()
		{
			if (!_isBusy && FishBoxController.Instance.HasFishBox)
			{
				ChangeState(DockWorkerState.GoToCollectFish);
				_isBusy = true;
			}
		}

		public void ChangeState(DockWorkerState state)
		{
			DockWorkerState = state;
			//Debug.Log($"DockWorkerState: {state}");
			switch (DockWorkerState)
			{
				case DockWorkerState.Idle:
					dockWorkerDownOkImage.gameObject.SetActive(true);
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
					dockWorkerFishPanel.gameObject.SetActive(true);
					FishBoxController.OnDockWorkerArrivedBox?.Invoke(this);
					break;
				case DockWorkerState.ReturningFromCollectingFish:
					transform.DOMove(_initialPosition, speed).OnComplete(() =>
					{
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

