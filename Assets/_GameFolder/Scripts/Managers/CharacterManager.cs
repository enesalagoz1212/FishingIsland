using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Managers
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }

		public GameObject maleCharacterPrefab;
		public GameObject femaleCharacterPrefab;
		public Transform character;
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

		public void InstantiateCharacter(bool isMale)
		{
			GameObject characterPrefab = isMale ? maleCharacterPrefab : femaleCharacterPrefab;
			Instantiate(characterPrefab, new Vector3(-32.5f,0f,0f),Quaternion.identity,character);
		}
	}
}

