using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Managers;

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
		private PlayerController _playerController;

		public Transform fishingDestination;
		public float movementSpeed;

		public void Initialize(PlayerController playerController)
		{
			_playerController = playerController;
		}
		private void Awake()
		{
			BoatState = BoatState.Idle;
		}

		public void ChangeState(BoatState boatState)
		{
			BoatState = boatState;
			Debug.Log($"BoatState: {boatState}");
		}

		public void PlayerBoarded()
		{
			ChangeState(BoatState.Moving);
			Debug.Log("Player Boarded");
		}

		private void Update()
		{
			if (GameManager.Instance.GameState==GameState.Playing)
			{
				switch (BoatState)
				{
					case BoatState.Idle:
						Debug.Log("Idle");

						break;
					case BoatState.Moving:
						MoveToDestination();
						break;
					case BoatState.Fishing:

						break;
					default:
						break;
				}
			}
			
		}

		private void MoveToDestination()
		{
			if (fishingDestination != null)
			{
				transform.position = Vector3.MoveTowards(transform.position, fishingDestination.position, movementSpeed * Time.deltaTime);

				if (Vector3.Distance(transform.position, fishingDestination.position) < 0.1f)
				{
					Debug.Log("fishing");
					ChangeState(BoatState.Fishing);
				}

				if (_playerController != null)
				{
					_playerController.FollowBoat(transform.position);
					Debug.Log("1");
				}
			}
			else
			{
				Debug.LogError("No fishing spot assigned");
			}
		}
	}

}
