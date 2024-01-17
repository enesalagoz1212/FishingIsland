using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishingIsland.Canvases;
using FishingIsland.Controllers;

namespace FishingIsland.Managers
{
	public class UpgradeManager : MonoBehaviour
	{
		public static UpgradeManager Instance { get; private set; }

		private DockController _dockController;
		private ShackController _shackController;

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

		public void Initialize(GameManager gameManager,DockController dockController,ShackController shackController )
		{
			_dockController = dockController;
			_shackController = shackController;

			houseUpgradeCanvas.Initialize();
			dockUpgradeCanvas.Initialize(dockController);
			shackUpgradeCanvas.Initialize(shackController);

		}
	}

}
