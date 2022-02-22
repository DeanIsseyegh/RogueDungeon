using UnityEngine;
using UnityEngine.AI;

public class EnemyStateCtx
{
    public GameObject Enemy { get; }
    public NavMeshAgent MeshAgent { get; }
    public Animator Animator { get; }
    public GameObject Player { get; }
    public EnemyAttack EnemyAttack { get; }

    public EnemyStateCtx(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator, GameObject player,
        EnemyAttack enemyEnemyAttack)
    {
        Enemy = enemy;
        MeshAgent = navMeshAgent;
        Animator = animator;
        Player = player;
        EnemyAttack = enemyEnemyAttack;
    }
}