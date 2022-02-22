using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAttack : MonoBehaviour, EnemyAttack
{
    [SerializeField] private Spell spell;
    [SerializeField] private float attackDistance;
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        spell = Instantiate(spell);
    }

    public void DoAttack()
    {
        spell.Cast(_navMeshAgent, _animator, _playerInventory);
    }

    public bool IsAttacking()
    {
        var currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isSpellCastAnim = currentAnimatorStateInfo.IsName(spell.animationName);
        return spell.IsCastingSpell || isSpellCastAnim;
    }

    public float AttackDistance()
    {
        return attackDistance;
    }
}
