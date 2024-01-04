using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

		public override void Initialize(string name, float speed, int initialCapacity)
		{
			base.Initialize(name, speed, initialCapacity);

		}

		public override void WorkerMouseDown()
		{
			base.WorkerMouseDown();
		
		}

		public void ChangeState(FishWorkerState state)
		{
			FishWorkerState = state;

			switch (FishWorkerState)
			{
				case FishWorkerState.Idle:
					break;
				case FishWorkerState.CollectingFish:
					break;
				case FishWorkerState.GoingToSellFish:
					break;
				case FishWorkerState.ReturnsFromSellingFish:
					break;
			}

		}
	}

}
