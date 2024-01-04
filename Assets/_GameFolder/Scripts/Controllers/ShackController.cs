using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FishingIsland.Controllers
{
    public class ShackController : MonoBehaviour
    {
        public static ShackController Instance;
        public TextMeshProUGUI shackFishCountText;
        private int _shackFishCount = 0;

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

        public IEnumerator CollectFish(int fishCount)
        {
            for (int i = 0; i < fishCount; i++)
            {
                yield return new WaitForSeconds(0.3f);
                _shackFishCount++;
                UpdateFishCountText();
            }
        }

        private void UpdateFishCountText()
        {
            shackFishCountText.text = $" {_shackFishCount}";
        }
    }

}