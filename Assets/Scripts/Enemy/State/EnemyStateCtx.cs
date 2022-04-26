using UnityEngine;
using UnityEngine.AI;

public class EnemyStateCtx
{
    public GameObject Enemy { get; }
    public NavMeshAgent MeshAgent { get; }
    public Animator Animator { get; }
    public GameObject Player { get; }
    public EnemyAttack[] EnemyAttacks { get; }
    public float VisDistance { get; }

    public EnemyStateCtx(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator, GameObject player,
        EnemyAttack[] enemyEnemyAttacks, float visDistance)
    {
        Enemy = enemy;
        MeshAgent = navMeshAgent;
        Animator = animator;
        Player = player;
        EnemyAttacks = enemyEnemyAttacks;
        VisDistance = visDistance;
    }
}