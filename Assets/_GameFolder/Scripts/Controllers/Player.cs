using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
	public enum PlayerState
	{
		OnLand,
		OnBoat,
	}
	public class Player : MonoBehaviour
	{
		public float movementSpeed = 5f;
		public Transform target;
		public PlayerState PlayerState { get; private set; }

		private void Awake()
		{
			PlayerState = PlayerState.OnLand;
		}
		private void Start()
		{

		}

		private void Update()
		{
			switch (PlayerState)
			{
				case PlayerState.OnLand:
					MoveTowardsTarget();

					break;
				case PlayerState.OnBoat:

					break;
				default:
					break;
			}

		}

		public void SetTargetForCharacter(Transform newTarget)
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


