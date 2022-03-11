using UnityEngine;

public class PhysicalEnemyAttack : EnemyAttack
{
    [SerializeField] private float attackDistance;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private EnemyWeapon _enemyWeapon;
    private AnimatorStateInfo _currentAnimatorStateInfo;
    private Animator _animator;
    private bool _isAttackStarting;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyWeapon = GetComponentInChildren<EnemyWeapon>();
    }

    private void Update()
    {
        _currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_isAttackStarting && _currentAnimatorStateInfo.IsName("Attack")) 
            _isAttackStarting = false;
        if (_enemyWeapon != null) 
            _enemyWeapon.IsActive = IsAttacking();
    }
    
    public override void DoAttack()
    {
        _animator.SetTrigger(Attack);
        _isAttackStarting = true;
    }
    
    public override bool IsAttacking()
    {
        return _isAttackStarting || _currentAnimatorStateInfo.IsName("Attack");
    }
    
    public override float AttackDistance()
    {
        return attackDistance;
    }
}
