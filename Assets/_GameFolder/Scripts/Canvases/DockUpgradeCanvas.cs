using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FishingIsland.Canvases
{
    public class DockUpgradeCanvas : MonoBehaviour
    {
        public Button dockCloseButton;
        public void Initialize()
		{
            dockCloseButton.onClick.AddListener(OnCloseButtonClick);
		}
       


        public void OnCloseButtonClick()
		{
            gameObject.SetActive(false);
		}
    }
}

