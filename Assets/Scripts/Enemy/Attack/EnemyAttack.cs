using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public abstract void DoAttack();

    public abstract bool IsAttacking();
    
    public abstract float AttackDistance();
}
