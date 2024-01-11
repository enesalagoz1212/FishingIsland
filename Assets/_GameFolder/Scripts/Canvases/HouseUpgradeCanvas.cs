using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FishingIsland.Canvases
{
	public class HouseUpgradeCanvas : MonoBehaviour
	{
		public Button houseCloseButton;
		public void Initialize()
		{
			houseCloseButton.onClick.AddListener(OnCloseButtonClick);
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
		}

	}
}

