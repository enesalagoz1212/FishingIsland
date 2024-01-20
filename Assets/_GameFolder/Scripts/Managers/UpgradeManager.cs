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

		public void Initialize(GameManager gameManager)
		{
			houseUpgradeCanvas.Initialize();
			dockUpgradeCanvas.Initialize();
			shackUpgradeCanvas.Initialize();
		}

		public void ActivateDockUpgradeCanvas()
		{
			dockUpgradeCanvas.gameObject.SetActive(true);
			houseUpgradeCanvas.gameObject.SetActive(false);
			shackUpgradeCanvas.gameObject.SetActive(false);
		}

		public void ActivateShackUpgradeCanvas()
		{
			dockUpgradeCanvas.gameObject.SetActive(false);
			houseUpgradeCanvas.gameObject.SetActive(false);
			shackUpgradeCanvas.gameObject.SetActive(true);
		}

		public void ActivateHouseUpgradeCanvas()
		{
			dockUpgradeCanvas.gameObject.SetActive(false);
			houseUpgradeCanvas.gameObject.SetActive(true);
			shackUpgradeCanvas.gameObject.SetActive(false);
		}
	}
}
