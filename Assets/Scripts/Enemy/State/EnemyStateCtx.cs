using UnityEngine;
using UnityEngine.AI;

public class EnemyStateCtx
{
    public GameObject Enemy { get; }
    public NavMeshAgent MeshAgent { get; }
    public Animator Animator { get; }
    public GameObject Player { get; }
    public Health Health { get; }
    public Despawner Despawner { get; }
    public EnemyAttack[] EnemyAttacks { get; }
    public float VisDistance { get; }

    public EnemyStateCtx(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator, GameObject player,
        Health health,
        Despawner despawner,
        EnemyAttack[] enemyEnemyAttacks, float visDistance)
    {
        Enemy = enemy;
        MeshAgent = navMeshAgent;
        Animator = animator;
        Player = player;
        Health = health;
        Despawner = despawner;
        EnemyAttacks = enemyEnemyAttacks;
        VisDistance = visDistance;
    }
}