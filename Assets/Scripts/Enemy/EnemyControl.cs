using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 2;
    [SerializeField] private float attackDistance = 2;

    private GameObject _player;
    private EnemyAttack _enemyAttack;
    private NavMeshAgent _navMeshAgent;
    private float _timeSinceLastAttackFinished = 0;
    private AnimationHandler _animationHandler;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();
        _animationHandler = GetComponent<AnimationHandler>();
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        bool isAttacking = _enemyAttack.IsAttacking();
        RotateTowardsPlayer();

        if (isAttacking)
        {
            _animationHandler.CanMove = false;
            _timeSinceLastAttackFinished = 0;
            _navMeshAgent.ResetPath();
        }
        else
        {
            _timeSinceLastAttackFinished += Time.deltaTime;
            bool isAttackCooldownDone = _timeSinceLastAttackFinished > attackCooldown;
            _animationHandler.CanMove = true;
            float distToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            if (distToPlayer < attackDistance && isAttackCooldownDone)
            {
                _navMeshAgent.ResetPath();
                _enemyAttack.DoAttack();
                _timeSinceLastAttackFinished = 0;
            }
            else
            {
                _navMeshAgent.SetDestination(_player.transform.position);
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        var direction = (_player.transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime);
    }
}