using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float visDistance = 10f;
    private EnemyState _currentState;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        var enemyAttacks = GetComponents<EnemyAttack>();
        var health = GetComponent<Health>();
        var despawner = GetComponent<Despawner>();
        var enemyStateCtx = new EnemyStateCtx(gameObject, navMeshAgent, animator, player, health, despawner, enemyAttacks, visDistance);
        _currentState = new Idle(enemyStateCtx);
    }

    private void Update()
    {
        _currentState = _currentState.Process();
    }
}