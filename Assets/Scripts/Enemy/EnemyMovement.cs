using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float attackCooldown = 2;
    [SerializeField] private float attackDistance = 2;
    private EnemyAttack _enemyAttack;
    private NavMeshAgent _navMeshAgent;
    private float _timeSinceLastAttackFinished = 0;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();
    }

    void Update()
    {
        bool isAttacking = _enemyAttack.IsAttacking();
        RotateTowardsPlayer();

        if (isAttacking)
        {
            _timeSinceLastAttackFinished = 0;
            if (_enemyAttack.IsAttacking())
            {
                _navMeshAgent.ResetPath();
            }
        }
        else
        {
            _timeSinceLastAttackFinished += Time.deltaTime;
            bool isAttackCooldownDone = _timeSinceLastAttackFinished > attackCooldown;
            
            float distToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distToPlayer < attackDistance && isAttackCooldownDone)
            {
                _navMeshAgent.ResetPath();
                _enemyAttack.DoAttack();
                _timeSinceLastAttackFinished = 0;
            }
            else
            {
                _navMeshAgent.SetDestination(player.transform.position);
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        var direction = (player.transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime);
    }
}