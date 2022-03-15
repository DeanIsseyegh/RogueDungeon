using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public abstract class CollectibleInfo : ScriptableObject
{

    public string collectibleName;
    public string description;
    [MultiLineProperty(5)]
    public string stats;

}
