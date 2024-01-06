using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingIsland.UpgradeScriptableObjects
{
    public class ShackUpgrade : ScriptableObject
    {
        public int level;
        public float cost;
        public float speedIncrease;
        public int capacityIncrease;
    }
}

