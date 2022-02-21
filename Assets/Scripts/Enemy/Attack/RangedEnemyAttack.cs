using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAttack : MonoBehaviour, EnemyAttack
{
    [SerializeField] private Spell spell;
    
    private NavMeshAgent _navMeshAgent;
    private AnimationHandler _animationHandler;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animationHandler = GetComponent<AnimationHandler>();
        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        spell = Instantiate(spell);
    }

    public void DoAttack()
    {
        spell.Cast(_navMeshAgent, _animationHandler, _playerInventory);
    }

    public bool IsAttacking()
    {
        return spell.IsCastingSpell;
    }
}
