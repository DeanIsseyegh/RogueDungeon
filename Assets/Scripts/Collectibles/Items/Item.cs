using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New Item", menuName = "Items")]
public class Item : Collectible
{
    public float playerSpellSpeedModifier = 1;

    public override void ApplyEffects(GameObject spellPrefab)
    {
        MoveForward moveForward = spellPrefab.GetComponent<MoveForward>();
        moveForward.ModifySpeed((speed) => speed * playerSpellSpeedModifier);
    }
}
