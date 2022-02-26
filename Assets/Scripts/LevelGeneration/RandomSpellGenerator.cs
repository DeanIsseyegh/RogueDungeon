using System.Collections.Generic;
using UnityEngine;

public class RandomSpellGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectibleSpells;

    public void Generate(Vector3 posToGenerate)
    {
        int randomIndex = Random.Range(0, collectibleSpells.Count);
        GameObject collectibleSpell = collectibleSpells[randomIndex];
        Instantiate(collectibleSpell, posToGenerate, Quaternion.identity);
        collectibleSpells.Remove(collectibleSpell);
    }
}
