using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomGameObjGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool;
    [SerializeField] private bool doesDeplete;

    public GameObject Generate(Vector3 posToGenerate)
    {
        if (objectPool.Count == 0)
        {
            Debug.Log("No collectibles left to generate!");
            return null;
        }

        int randomIndex = Random.Range(0, objectPool.Count);
        GameObject collectibleItem = objectPool[randomIndex];
        GameObject createdCollectible = Instantiate(collectibleItem, posToGenerate, Quaternion.identity);
        if (doesDeplete)
            objectPool.RemoveAt(randomIndex);
        return createdCollectible;
    }

    public GameObject Generate(Vector3 posToGenerate, GameObject toNotSpawn)
    {
        if (objectPool.Count == 0)
        {
            Debug.Log("No collectibles left to generate!");
            return null;
        }

        List<GameObject> newObjectPool = new List<GameObject>(objectPool);
        if (toNotSpawn != null)
        {
            newObjectPool = newObjectPool
                .Where(it =>
                    it.GetComponent<OnPickUp>().Collectible != toNotSpawn.GetComponent<OnPickUp>().Collectible)
                .ToList();
        }

        int randomIndex = Random.Range(0, newObjectPool.Count);
        GameObject collectibleItem = newObjectPool[randomIndex];
        GameObject createdCollectible = Instantiate(collectibleItem, posToGenerate, Quaternion.identity);
        if (doesDeplete)
            objectPool.RemoveAt(randomIndex);
        return createdCollectible;
    }

    public int ObjectsLeft()
    {
        return objectPool.Count;
    }
}