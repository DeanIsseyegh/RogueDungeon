using System.Collections.Generic;
using UnityEngine;

public class GeneratedWalls
{
    public List<GameObject> FirstRowWalls { get; }
    public List<GameObject> LastRowWalls { get; }
    public List<GameObject> RightColumnWalls { get; }
    public List<GameObject> LeftColumnWalls { get; }

    public GeneratedWalls(List<GameObject> firstRowWalls, List<GameObject> lastRowWalls, List<GameObject> rightColumnWalls, List<GameObject> leftColumnWalls)
    {
        FirstRowWalls = firstRowWalls;
        LastRowWalls = lastRowWalls;
        RightColumnWalls = rightColumnWalls;
        LeftColumnWalls = leftColumnWalls;
    }
}