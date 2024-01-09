using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Managers
{
	public class LevelManager : MonoBehaviour
	{
		public static LevelManager Instance { get; private set; }

		public List<Transform> boatSpawnPoints = new List<Transform>();

		public List<Transform> fishWorkerSellPath = new List<Transform>();
		public List<Transform> fishWorkerReturnPath = new List<Transform>();

		public List<Transform> dockWorkerGoesFishing = new List<Transform>();

		public List<Transform> BoatSpawnPoints
		{
			get { return boatSpawnPoints; }
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		public void Initialize()
		{

		}

		public Transform GetRandomBoatSpawnPoint()
		{
			if (boatSpawnPoints.Count == 0)
			{
				Debug.LogError("No boat spawn points available ");
				return null;
			}

			int randomIndex = Random.Range(0, boatSpawnPoints.Count);
			return boatSpawnPoints[randomIndex];
		}
	}
}

