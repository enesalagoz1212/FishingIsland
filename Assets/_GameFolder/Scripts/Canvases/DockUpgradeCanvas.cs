using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishingIsland.Managers;
using TMPro;

namespace FishingIsland.Canvases
{
    public class DockUpgradeCanvas : MonoBehaviour
    {
        private DockUpgradeManager _dockUpgradeManager;

        public Button dockCloseButton;
        public Button boatButton;
        public Button timerButton;
        public Button capacityButton;

        public TextMeshProUGUI boatLevelText;
        public void Initialize()
		{
        

            dockCloseButton.onClick.AddListener(OnCloseButtonClick);
            boatButton.onClick.AddListener(OnBoatButtonClick);
            timerButton.onClick.AddListener(OnTimerButtonClick);
            capacityButton.onClick.AddListener(OnCapacityButtonClick);

            DockUpgradeManager.OnBoatLevelUpdated += UpdateBoatLevelText;
        }
       


        public void OnCloseButtonClick()
		{
            gameObject.SetActive(false);
		}

        public void OnBoatButtonClick()
		{
            DockUpgradeManager.Instance.UpgradeBoatLevel();
            Debug.Log("OnBoatButtonClick");
		}

        public void OnTimerButtonClick()
		{
            DockUpgradeManager.Instance.UpgradeTimerLevel();
            Debug.Log("OnTimerButtonClick");
		}

        public void OnCapacityButtonClick()
		{
            DockUpgradeManager.Instance.UpgradeCapacityLevel();
            Debug.Log("OnCapacityButtonClick");
		}

        private void UpdateBoatLevelText(int newBoatLevel)
		{
            boatLevelText.text = $"Boat Level: {newBoatLevel}";
        }
    }
}

