using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FishingIsland.Controllers
{
    public class FishBoxController : MonoBehaviour
    {
        public static FishBoxController Instance;

        public TextMeshProUGUI boxFishText;
        private int _totalFishCount = 0;

        public bool HasFishBox => _totalFishCount > 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void Initialize()
		{

		}

        public IEnumerator CollectFish(int fishCount)
        {
            for (int i = 0; i < fishCount; i++)
            {
                yield return new WaitForSeconds(0.3f);
                _totalFishCount++;
                UpdateFishCountText();
            }
        }


        public IEnumerator TransferFish()
        {
            while (_totalFishCount > 0)
            {
                yield return new WaitForSeconds(0.3f);
                _totalFishCount--;
                UpdateFishCountText();
            }
        }

        private void UpdateFishCountText()
        {
            boxFishText.text = $" {_totalFishCount}";
        }
    }
}

