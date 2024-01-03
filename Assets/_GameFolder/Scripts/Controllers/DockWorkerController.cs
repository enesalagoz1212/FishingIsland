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
		public override void Initialize(string name, float speed, int initialCapacity)
		{
			base.Initialize(name, speed, initialCapacity);
			capacity = 10;
			speed = 1.5f;
			characterName = "DockWorker";
		}

		void Start()
		{
		  
		}

		public override void WorkerMouseDown()
		{
			base.WorkerMouseDown();
			ChangeState(DockWorkerState.GoToCollectFish);
		}

		public void MoveTo(Vector3 targetPosition)
		{
			transform.DOMove(targetPosition, speed).OnComplete(() =>
			{
				ChangeState(DockWorkerState.CollectingFish);
				StartCoroutine(CollectFish());

			}).OnComplete(() =>
			{
				ChangeState(DockWorkerState.ReturningFromCollectingFish);
			});
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
					//Debug.Log("Go to Collect Fish");
					MoveTo(_targetPos);
					break;
				case DockWorkerState.CollectingFish:
					//Debug.Log("Collecting Fish");				
					
					break;
				case DockWorkerState.ReturningFromCollectingFish:
					MoveTo(_initialPosition);
					break;
			}
		}

		private IEnumerator CollectFish()
		{
			for (int i = 0; i < capacity; i++)
			{
				yield return new WaitForSeconds(0.3f);
				_collectedFishCount++;
				UpdateFishCountText(_collectedFishCount);
			}

			ChangeState(DockWorkerState.ReturningFromCollectingFish);
		}

		private void UpdateFishCountText(int collectedFishCount)
		{
			dockWorkerFishCountText.text = $" {collectedFishCount}";
		}
	}
}

