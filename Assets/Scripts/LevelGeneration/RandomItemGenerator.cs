using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectibleItems;

    public void Generate(Vector3 posToGenerate)
    {
        int randomIndex = Random.Range(0, collectibleItems.Count);
        GameObject collectibleSpell = collectibleItems[randomIndex];
        Instantiate(collectibleSpell, posToGenerate, Quaternion.identity);
    }
}
