using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomGameObjGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool;
    [SerializeField] private bool doesDeplete;

    public GameObject Generate(Vector3 posToGenerate)
    {
        if (objectPool.Count == 0)
        {
            Debug.Log("No objects left to generate!");
            return null;
        }

        int randomIndex = Random.Range(0, objectPool.Count);
        GameObject objToCreate = objectPool[randomIndex];
        GameObject createdObj = CreateObj(posToGenerate, objToCreate, objToCreate.transform.rotation);
        if (doesDeplete)
            objectPool.RemoveAt(randomIndex);
        return createdObj;
    }

    public GameObject Generate(Vector3 posToGenerate, List<Collectible> collectiblesToNotSpawn,
        GameObject collectibleToNotSpawn)
    {
        if (objectPool.Count == 0)
        {
            Debug.Log("No objects left to generate!");
            return null;
        }

        List<GameObject> newObjectPool = new List<GameObject>(objectPool);
        if (collectibleToNotSpawn != null)
        {
            newObjectPool = newObjectPool
                .Where(it =>
                    it.GetComponent<OnPickUp>().Collectible !=
                    collectibleToNotSpawn.GetComponent<OnPickUp>().Collectible)
                .ToList();
        }

        newObjectPool = newObjectPool
            .Where(it => !collectiblesToNotSpawn.Contains(it.GetComponent<OnPickUp>().Collectible))
            .ToList();

        int randomIndex = Random.Range(0, newObjectPool.Count);
        GameObject objToCreate = newObjectPool[randomIndex];
        GameObject createdObj = CreateObj(posToGenerate, objToCreate, objToCreate.transform.rotation);
        if (doesDeplete)
            objectPool.RemoveAt(randomIndex);
        return createdObj;
    }

    protected virtual GameObject CreateObj(Vector3 posToGenerate, GameObject objToCreate, Quaternion quaternion)
    {
        return Instantiate(objToCreate, posToGenerate, quaternion);
    }

    public int ObjectsLeft()
    {
        return objectPool.Count;
    }
}