using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCollectibleGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectibles;

    public GameObject Generate(Vector3 posToGenerate)
    {
        if (collectibles.Count == 0)
        {
            Debug.Log("No collectibles left to generate!");
            return null;
        }
        else
        {
            int randomIndex = Random.Range(0, collectibles.Count);
            GameObject collectibleItem = collectibles[randomIndex];
            collectibles.RemoveAt(randomIndex);
            GameObject createdCollectible = Instantiate(collectibleItem, posToGenerate, Quaternion.identity);
            return createdCollectible;
        }
    }
}
