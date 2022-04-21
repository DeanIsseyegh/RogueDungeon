using System.Collections.Generic;
using Level.Generation.FloorDecorations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Floor Decoration", menuName = "Floor Decorations")]
public class FloorDecoration : ScriptableObject
{

    public FloorDecoName decoName;
    public GameObject prefab;
    
    [Title("Wave Function Collapse")]
    public int rngWeighting;

    public List<FloorDecoName> allowedNeighbours;
    public float rotationRange = 360;

}
