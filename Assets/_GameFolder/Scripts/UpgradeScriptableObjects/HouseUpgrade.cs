using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
    [CreateAssetMenu(fileName = "New Dock Upgrade", menuName = "Dock Upgrade")]
    public class HouseUpgrade : ScriptableObject
    {
        public int level;
        public float cost;
        public float speedIncrease;
        public int capacityIncrease;
    }
}

