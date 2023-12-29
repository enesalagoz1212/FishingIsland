using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Managers;

namespace FishingIsland.Controllers
{
	public enum PlayerState
	{
		OnLand,
		OnBoat,
	}
	public class Player : MonoBehaviour
	{
		private BoatController boatController;

		public float movementSpeed = 5f;
		public Transform target;
		public PlayerState PlayerState { get; private set; }

		private void Awake()
		{
			PlayerState = PlayerState.OnLand;
		}
		private void Start()
		{
			boatController = FindObjectOfType<BoatController>();
		}

		private void Update()
		{
			if (GameManager.Instance.GameState==GameState.Playing)
			{
				switch (PlayerState)
				{
					case PlayerState.OnLand:
						MoveTowardsTarget();

						break;
					case PlayerState.OnBoat:
						boatController.PlayerBoarded();
						break;
					default:
						break;
				}
			}
			

		}

		public void SetTargetForPlayer(Transform newTarget)
		{
			target = newTarget;
		}


		public void ChangeState(PlayerState playerState)
		{
			PlayerState = playerState;
			Debug.Log($"PlayerState: {playerState}");
		
		}

		private void MoveTowardsTarget()
		{
			if (target != null)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

				if (Vector3.Distance(transform.position, target.position) < 0.1f)
				{
					Debug.Log("reached the boat");
					ChangeState(PlayerState.OnBoat);
				}
			}
			else
			{
				Debug.LogError("No target assigned!");
			}
		}

		
	}
}


