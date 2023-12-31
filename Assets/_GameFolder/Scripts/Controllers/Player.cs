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

		}



		public void ChangeState(PlayerState playerState)
		{
			PlayerState = playerState;
			Debug.Log($"PlayerState: {playerState}");
		
		}

		
		
	}
}


