using System.Collections.Generic;
using Level.Generation.WallDecorations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Wall Decoration", menuName = "Wall Decorations")]
public class WallDecoration : ScriptableObject
{
    [FormerlySerializedAs("decoName")] public WallDecoName decoWallDecoName;
    public Vector3 offset;
    public GameObject prefab;
    
    [Title("Wave Function Collapse")]
    public int rngWeighting;
    public List<WallDecoName> allowedNeighbours;
}
