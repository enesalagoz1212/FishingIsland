using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;

namespace FishingIsland.Canvases
{
    public class DockUpgradeCanvas : MonoBehaviour
    {
        public Button dockCloseButton;
        public Button boatButton;
        public Button timerButton;
        public Button capacityButton;
        public void Initialize()
		{
            dockCloseButton.onClick.AddListener(OnCloseButtonClick);
            boatButton.onClick.AddListener(OnBoatButtonClick);
            timerButton.onClick.AddListener(OnTimerButtonClick);
            capacityButton.onClick.AddListener(OnCapacityButtonClick);
		}
       


        public void OnCloseButtonClick()
		{
            gameObject.SetActive(false);
		}

        public void OnBoatButtonClick()
		{
            DockUpgradeManager.Instance.UpgradeBoatLevel();
		}

        public void OnTimerButtonClick()
		{
            DockUpgradeManager.Instance.UpgradeTimerLevel();
		}

        public void OnCapacityButtonClick()
		{
            DockUpgradeManager.Instance.UpgradeCapacityLevel();
		}
    }
}

