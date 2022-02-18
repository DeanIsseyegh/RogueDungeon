using UnityEngine.AI;

public class SpeedySpellsItem : Item
{
    public override void ApplyEffects(PlayerHealth playerHealth) { }

    public override void ApplyEffects(PlayerSpellManager playerSpellManager)
    {
    }

    public override void ApplyEffects(NavMeshAgent playerNavMeshAgent) { }

    public override void ApplyEffects(Spell spell)
    {
        spell.ModifySpeed((speed) => speed * 2);
    }

}
