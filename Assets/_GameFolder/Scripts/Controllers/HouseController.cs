using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.Controllers
{
    public class HouseController : MonoBehaviour
    {

        public GameObject houseUpgradeCanvas;
        void Start()
        {

        }


        void Update()
        {

        }

		private void OnMouseDown()
		{
            Debug.Log("HouseController");
            houseUpgradeCanvas.SetActive(true);
		}
	}
}

