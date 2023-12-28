using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
	public enum BoatState
	{
		Idle,
		Moving,
		Fishing,
	}
	public class BoatController : MonoBehaviour
	{
		
		public BoatState BoatState { get; private set; }

		private void Awake()
		{
			BoatState = BoatState.Idle;
		}

		public void ChangeState(BoatState boatState)
		{
			BoatState = boatState;
			Debug.Log($"BoatState: {boatState}");
		}

		private void Update()
		{
			switch (BoatState)
			{
				case BoatState.Idle:
					Debug.Log("Idle");
					break;
				case BoatState.Moving:

					break;
				case BoatState.Fishing:

					break;
				default:
					break;
			}
		}
	}

}
