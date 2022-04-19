using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Decoration", menuName = "Wall Decorations")]
public class WallDecoration : ScriptableObject
{
    public Vector3 offset;
    public GameObject prefab;
    public int rngWeighting;
}
