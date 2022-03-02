using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomGameObjGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool;

    public GameObject Generate(Vector3 posToGenerate)
    {
        if (objectPool.Count == 0)
        {
            Debug.Log("No collectibles left to generate!");
            return null;
        }
        else
        {
            int randomIndex = Random.Range(0, objectPool.Count);
            GameObject collectibleItem = objectPool[randomIndex];
            objectPool.RemoveAt(randomIndex);
            GameObject createdCollectible = Instantiate(collectibleItem, posToGenerate, Quaternion.identity);
            return createdCollectible;
        }
    }
}
