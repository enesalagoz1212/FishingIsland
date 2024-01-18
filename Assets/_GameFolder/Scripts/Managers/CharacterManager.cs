using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Managers
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }

		public void Initialize()
		{

		}

		private void Awake()
		{
			if ( Instance!=null && Instance!=this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}
	}
}

