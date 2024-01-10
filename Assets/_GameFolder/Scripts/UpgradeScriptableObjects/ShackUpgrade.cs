using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
    [CreateAssetMenu(fileName = "New Shack Upgrade", menuName = "Shack Upgrade")]
    public class ShackUpgrade : ScriptableObject
    {
        public int level;
        public float cost;
        public float speedIncrease;
        public int capacityIncrease;
    }
}

