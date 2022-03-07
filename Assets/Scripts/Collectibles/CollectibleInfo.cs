using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public abstract class CollectibleInfo : ScriptableObject
{

    public string name;
    public string description;
    [MultiLineProperty(5)]
    public string stats;

}
