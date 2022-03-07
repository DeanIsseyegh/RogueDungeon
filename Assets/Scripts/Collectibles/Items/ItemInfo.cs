using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Collectible Info", menuName = "Item Collectible Info")]
public class ItemInfo : CollectibleInfo
{

    public void GenerateStats(CollectibleItem item)
    {
        base.stats = "";
    }
}
