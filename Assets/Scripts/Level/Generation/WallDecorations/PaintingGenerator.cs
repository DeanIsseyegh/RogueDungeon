using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> possiblePaintings;
    [SerializeField] private List<GameObject> possibleFrames;

    public GameObject Generate(Transform parent)
    {
        int randomFrameIndex = Random.Range(0, possibleFrames.Count);
        var pickedFrame = possibleFrames[randomFrameIndex];
        int randomPaintingIndex = Random.Range(0, possiblePaintings.Count);
        var pickedPainting = possiblePaintings[randomPaintingIndex];

        GameObject newObj = new GameObject($"Painting-{randomFrameIndex}-{randomPaintingIndex}");
        GameObject paintingParent = Instantiate(newObj, parent);
        paintingParent.transform.Rotate(new Vector3(0, 90, 0));
        Destroy(newObj);

        Instantiate(pickedFrame, paintingParent.transform);
        Instantiate(pickedPainting, paintingParent.transform);

        return paintingParent;
    }
}
