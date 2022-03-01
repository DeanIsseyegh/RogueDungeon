using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectibleItems;

    public void Generate(Vector3 posToGenerate)
    {
        if (collectibleItems.Count == 0)
        {
            Debug.Log("No items left to generate!");
        }
        else
        {
            int randomIndex = Random.Range(0, collectibleItems.Count);
            GameObject collectibleItem = collectibleItems[randomIndex];
            Instantiate(collectibleItem, posToGenerate, Quaternion.identity);
            collectibleItems.Remove(collectibleItem);
        }
    }
}