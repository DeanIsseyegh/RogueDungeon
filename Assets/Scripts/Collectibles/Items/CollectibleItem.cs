using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items")]
public class CollectibleItem : Collectible
{
    public float playerSpellSpeedModifier = 1;
    public float playerSpellSizeModifier = 0;
    
    public ItemInfo info;

    private void OnEnable()
    {
        info.GenerateStats(this);
    }

    public override void ApplyEffects(UIManager uiManager)
    {
        uiManager.AddItemIcon(icon);
    }

    public override void ApplyEffects(GameObject spellPrefab)
    {
        MoveForward moveForward = spellPrefab.GetComponent<MoveForward>();
        moveForward.ModifySpeed((speed) => speed * playerSpellSpeedModifier);

        Vector3 scaleChange = new Vector3(playerSpellSizeModifier, playerSpellSizeModifier, playerSpellSizeModifier);
        spellPrefab.transform.localScale += scaleChange;
    }
    
    public override CollectibleInfo Info()
    {
        return info;
    }
}
