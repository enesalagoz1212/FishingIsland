using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FishingIsland.Controllers
{
    public class DockController : MonoBehaviour
    {
		public GameObject dockUpgradeCanvas;
		private void OnMouseDown()
		{
			Debug.Log("DockController");
			dockUpgradeCanvas.SetActive(true);
		}
	}
}
