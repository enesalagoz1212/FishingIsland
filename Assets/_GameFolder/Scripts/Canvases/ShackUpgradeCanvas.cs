using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FishingIsland.Canvases
{
    public class ShackUpgradeCanvas : MonoBehaviour
    {
        public Button shackCloseButton;

		public void Initialize()
		{
			shackCloseButton.onClick.AddListener(OnCloseButtonClick);
		}

		public void OnCloseButtonClick()
		{
			gameObject.SetActive(false);
		}
     
    }
}

