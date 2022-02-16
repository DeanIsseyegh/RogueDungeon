using UnityEngine;

public class PhysicalEnemyAttack : MonoBehaviour, EnemyAttack
{
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private EnemyWeapon _enemyWeapon;
    private AnimatorStateInfo _currentAnimatorStateInfo;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyWeapon = GetComponentInChildren<EnemyWeapon>();
    }

    private void Update()
    {
        _currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_enemyWeapon != null) _enemyWeapon.IsActive = IsAttacking();
    }
    
    public void DoAttack()
    {
        _animator.SetTrigger(Attack1);
    }
    
    public bool IsAttacking()
    {
        return _currentAnimatorStateInfo.IsName("Attack");
    }
}
