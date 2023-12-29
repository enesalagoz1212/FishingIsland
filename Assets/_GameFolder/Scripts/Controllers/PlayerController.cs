using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		private BoatController _boatController;
		public void Initialize(BoatController boatController)
		{
			_boatController=boatController;
		}
		void Start()
		{

		}

		void Update()
		{

		}
		public void FollowBoat(Vector3 boatPosition)
		{
			transform.position = new Vector3(boatPosition.x, transform.position.y, boatPosition.z);
		}

	}
}

