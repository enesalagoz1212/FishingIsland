using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FishingIsland.Controllers
{
	public class BaseCharacterController : MonoBehaviour
	{
		protected string characterName;
		protected float moveSpeed;
		protected int capacity;

		public virtual void Initialize(string name, float speed, int initialCapacity)
		{
			characterName = name;
			moveSpeed = speed;
			capacity = initialCapacity;
		}

	
		public void OnMouseDown()
		{
			WorkerMouseDown();
		}

		public virtual void WorkerMouseDown()
		{
			
		}

	    public virtual void Start()
		{

		}
	}
}

