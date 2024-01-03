using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FishingIsland.Controllers
{
    public class FishWorkerController : BaseCharacterController
    {

		public override void Initialize(string name, float speed, int initialCapacity)
		{
			base.Initialize(name, speed, initialCapacity);

		}

		public override void WorkerMouseDown()
		{
			base.WorkerMouseDown();
			Debug.Log("FishWorker calisti");
		}

		//public override void MoveTo(Vector3 targetPosition)
		//{
		//	transform.DOMove(targetPosition);
		//}
	}

}
