using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Canvases;

namespace FishingIsland.Managers
{
	public class UpgradeManager : MonoBehaviour
	{
		public static UpgradeManager Instance { get; private set; }

		[SerializeField] private HouseUpgradeCanvas houseUpgradeCanvas;
		[SerializeField] private DockUpgradeCanvas dockUpgradeCanvas;
		[SerializeField] private ShackUpgradeCanvas shackUpgradeCanvas;

	
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

		public void Initialize(GameManager gameManager )
		{

			houseUpgradeCanvas.Initialize();
			dockUpgradeCanvas.Initialize();
			shackUpgradeCanvas.Initialize();

		}
	}

}
