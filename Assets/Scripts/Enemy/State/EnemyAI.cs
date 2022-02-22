using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private EnemyState _currentState;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        var enemyAttack = GetComponent<EnemyAttack>();
        var enemyStateCtx = new EnemyStateCtx(gameObject, navMeshAgent, animator, player, enemyAttack);
        _currentState = new Idle(enemyStateCtx);
    }

    private void Update()
    {
        _currentState = _currentState.Process();
    }
}