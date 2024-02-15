using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
	public enum Direction
	{
		North,
		East,
		South,
		West
	}

	public class CarController : MonoBehaviour
	{
		public Direction Direction { get; private set; }
		private Vector3 lastPosition;
		private void Start()
		{
			Direction = Direction.East;
			 lastPosition = transform.position;
		}
	
		private void SetRotation()
		{
			switch (Direction)
			{
				case Direction.North:
					transform.rotation = Quaternion.Euler(0f, 90f, 0f);
					break;
				case Direction.East:
					transform.rotation = Quaternion.Euler(0f, 180f, 0f);
					break;
				case Direction.South:
					transform.rotation = Quaternion.Euler(0f, -90f, 0f);
					break;
				case Direction.West:
					transform.rotation = Quaternion.Euler(0f, 0f, 0f);
					break;
				default:
					break;
			}
		}

		private void Update()
		{
			UpdateDirectionFromPosition();
		}



		private void UpdateDirectionFromPosition()
		{
			Vector3 currentPosition = transform.position;
			Vector3 direction = currentPosition - lastPosition;

			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
			{
				if (direction.x > 0)
				{
					Direction = Direction.North;
				}
				else
				{
					Direction = Direction.South;
				}
			}
			else
			{
				if (direction.z > 0)
				{
					Direction = Direction.West;
				}
				else
				{
					Direction = Direction.East;
				}
			}

			SetRotation();
			lastPosition = currentPosition;
		}
	}
}

