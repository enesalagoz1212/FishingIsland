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

        public Button dockCloseButton;
        public Button boatButton;
        public Button timerButton;
        public Button capacityButton;

        public TextMeshProUGUI boatLevelText;
        public TextMeshProUGUI timerLevelText;
        public TextMeshProUGUI capacityLevelText;


        public void Initialize()
		{
            dockCloseButton.onClick.AddListener(OnCloseButtonClick);
            boatButton.onClick.AddListener(OnBoatButtonClick);
            timerButton.onClick.AddListener(OnTimerButtonClick);
            capacityButton.onClick.AddListener(OnCapacityButtonClick);

			DockUpgradeManager.OnBoatLevelUpdated += UpdateBoatLevelText;
			DockUpgradeManager.OnTimerLevelUpdated += UpdateTimerLevelText;
			DockUpgradeManager.OnCapacityLevelUpdated += UpdateCapacityLevelText;

		
			UpdateBoatLevelText(DockUpgradeManager.Instance.GetBoatLevel());
            UpdateTimerLevelText(DockUpgradeManager.Instance.GetTimerLevel());
            UpdateCapacityLevelText(DockUpgradeManager.Instance.GetCapacityLevel());
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
            boatLevelText.text = $" {newBoatLevel}";
        }

        private void UpdateTimerLevelText(int newTimerLevel)
        {
            timerLevelText.text = $" {newTimerLevel}";
        }

        private void UpdateCapacityLevelText(int newCapacityLevel)
        {
            capacityLevelText.text = $" {newCapacityLevel}";
        }

      
    }
}

