using System.Collections.Generic;
using UnityEngine;

public class RandomSpellGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectibleSpells;

    public GameObject Generate(Vector3 posToGenerate)
    {
        if (collectibleSpells.Count == 0)
        {
            Debug.Log("No spells left to generate!");
            return null;
        }
        else
        {
            int randomIndex = Random.Range(0, collectibleSpells.Count);
            GameObject collectibleSpell = collectibleSpells[randomIndex];
            collectibleSpells.RemoveAt(randomIndex);
            GameObject createdCollectible = Instantiate(collectibleSpell, posToGenerate, Quaternion.identity);
            return createdCollectible;
        }
    }
}