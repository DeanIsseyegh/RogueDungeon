using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellData : ScriptableObject
{
    [BoxGroup("General")]
    public GameObject spellPrefab;
    [BoxGroup("General")]
    public Vector3 spellSpawnOffset = new Vector3(0.2f, 1.1f, 0.2f);
    [BoxGroup("General")]
    public string animationName = "BasicSpell";
    
    [BoxGroup("Basic Stats")]
    public float manaCost = 10;
    [BoxGroup("Basic Stats")]
    public float spellLifeTime = 2f;
    [BoxGroup("Basic Stats")]
    public float spellStartUp = 0.4f;
    [BoxGroup("Basic Stats")]
    public float spellCooldown = 1;

    [InlineEditor()]
    public List<SpellTrait> spellTraits;

}
