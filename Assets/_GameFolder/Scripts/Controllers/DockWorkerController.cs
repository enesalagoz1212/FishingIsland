using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using FishingIsland.Managers;

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
		public float speed = 1f;
		public TextMeshProUGUI dockWorkerFishCountText;
		private Vector3 _initialPosition;
		private Vector3 _targetPosition;

		public GameObject dockWorkerFishPanel;
		private int _collectedFishCount = 0;
		private int _dockCapacity = 10;

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
			_dockCapacity = 10;
			ChangeState(DockWorkerState.Idle);
		}

		public override void WorkerMouseDown()
		{
			if (!_isBusy&& FishBoxController.Instance.HasFishBox)
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
					_dockCapacity = 10;
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
			_collectedFishCount++;
			UpdateFishCountText(_collectedFishCount);

			Debug.Log($"Collected Fish Count: {_collectedFishCount}, Capacity: {_dockCapacity}");

			if (_collectedFishCount >= _dockCapacity)
			{
				ChangeState(DockWorkerState.ReturningFromCollectingFish);
			}
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

