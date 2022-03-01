using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectibleItems;

    public GameObject Generate(Vector3 posToGenerate)
    {
        if (collectibleItems.Count == 0)
        {
            Debug.Log("No items left to generate!");
            return null;
        }
        else
        {
            int randomIndex = Random.Range(0, collectibleItems.Count);
            GameObject collectibleItem = collectibleItems[randomIndex];
            collectibleItems.RemoveAt(randomIndex);
            GameObject createdCollectible = Instantiate(collectibleItem, posToGenerate, Quaternion.identity);
            return createdCollectible;
        }
    }
}