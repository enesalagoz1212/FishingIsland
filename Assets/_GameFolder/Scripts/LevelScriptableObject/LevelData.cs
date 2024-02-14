using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.LevelScriptableObject
{
	[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data")]
	public class LevelData : ScriptableObject
	{
		public List<GameObject> levelObjects;

	}
}
