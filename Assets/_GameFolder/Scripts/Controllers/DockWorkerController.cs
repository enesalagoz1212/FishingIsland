using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
		private Vector3 _targetPos= new Vector3(-23, 0, 1);
		private Vector3 _initialPosition = new Vector3(-23, 0, 6);

		private int _collectedFishCount = 0;
		private int _capacity = 10;

		public override void Initialize(string name, float initialSpeed, int initialCapacity)
		{
			characterName = name;
			speed = initialSpeed;
			capacity = initialCapacity;
			Debug.Log("2");
		}

		public override void WorkerMouseDown()
		{
			base.WorkerMouseDown();
			ChangeState(DockWorkerState.GoToCollectFish);
		}

		public void ChangeState(DockWorkerState state)
		{
			DockWorkerState = state;
			//Debug.Log($"DockWorkerState: {state}");

			switch (DockWorkerState)
			{
				case DockWorkerState.Idle:
					break;
				case DockWorkerState.GoToCollectFish:
					Debug.Log("GotoCollectFish");
					transform.DOMove(_targetPos, speed).OnComplete(() =>
					{
						ChangeState(DockWorkerState.CollectingFish);
					});
					break;
				case DockWorkerState.CollectingFish:			
					StartCoroutine(CollectFish());
					StartCoroutine(FishBoxController.Instance.TransferFish());
					break;
				case DockWorkerState.ReturningFromCollectingFish:
					transform.DOMove(_initialPosition, speed).OnComplete(() =>
					{
						StartCoroutine(ShackController.Instance.CollectFish(_capacity));
						StartCoroutine(TransferFish());
					});
					break;
			}
		}

		public IEnumerator CollectFish()
		{
			Debug.Log("CollectFish Coroutine Started"); 

			for (int i = 0; i < _capacity; i++)
			{
				yield return new WaitForSeconds(0.3f);
				_collectedFishCount++;
				UpdateFishCountText(_collectedFishCount);
				Debug.Log("Fish Collected");
			}

			Debug.Log("CollectFish Coroutine Completed");

			ChangeState(DockWorkerState.ReturningFromCollectingFish);
		}

		public IEnumerator TransferFish()
		{
			while (_capacity > 0)
			{
				yield return new WaitForSeconds(0.3f);
				_capacity--;
				UpdateFishCountText(_capacity);
			}
		}
		private void UpdateFishCountText(int collectedFishCount)
		{
			dockWorkerFishCountText.text = $" {collectedFishCount}";
		}
	}
}

