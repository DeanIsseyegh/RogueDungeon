using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Powerup", menuName = "Powerups")]
public class CollectiblePowerup : Collectible
{
    public float extraHp = 0;
    public float extraMana = 0;

    public override void ApplyEffects(PlayerHealth playerHealth)
    {
        playerHealth.AddMaxHp(extraHp);
    }

    public override void ApplyEffects(PlayerMana playerMana)
    {
        playerMana.AddMaxMana(extraMana);
    }

    public override CollectibleInfo Info()
    {
        return null;
    }
}
