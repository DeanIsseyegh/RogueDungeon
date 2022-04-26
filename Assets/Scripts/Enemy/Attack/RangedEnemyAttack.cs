using UnityEngine;

public class RangedEnemyAttack : EnemyAttack
{
    [SerializeField] private float attackDistance;
    [SerializeField] private Spell spell;
    
    private Animator _animator;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }

    public override void DoAttack()
    {
        spell.Cast(_animator, _playerInventory);
    }

    public override bool IsAttacking()
    {
        var currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isSpellCastAnim = currentAnimatorStateInfo.IsName(spell.data.animationName);
        return spell.IsCastingSpell || isSpellCastAnim;
    }

    public override float AttackDistance()
    {
        return attackDistance;
    }
}
