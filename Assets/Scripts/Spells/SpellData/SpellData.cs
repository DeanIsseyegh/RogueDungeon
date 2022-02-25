using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellData : ScriptableObject
{
    public GameObject spellPrefab;
    public float damage = 10;
    public Vector3 spellSpawnOffset = new Vector3(0.2f, 1.1f, 0.2f);
    public float spellLifeTime = 2f;
    public float spellStartUp = 0.4f;
    public string animationName = "BasicSpell";
    public float spellCooldown = 1;

}
