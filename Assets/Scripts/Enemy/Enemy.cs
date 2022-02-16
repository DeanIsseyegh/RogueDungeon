using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private EnemyAttack _enemyAttack;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private float _attackCooldown = 2;
    private float _timeSinceLastAttack = 0;
    private static readonly int Attack = Animator.StringToHash("Attack");

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();
    }

    void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = currentAnimatorStateInfo.IsName("Attack");
        _enemyAttack.IsActive = isAttacking;
        if (isAttacking)
        {
            RotateTowardsPlayer();
            _timeSinceLastAttack = 0;
            float normalizedTime = currentAnimatorStateInfo.normalizedTime;
            if (normalizedTime >= 1)
            {
                isAttacking = false;
            }
            else
            {
                _navMeshAgent.ResetPath();
            }
        }

        float distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (!isAttacking)
        {
            _timeSinceLastAttack += Time.deltaTime;
            bool isAttackCooldownDone = _timeSinceLastAttack > _attackCooldown;
            if (distToPlayer < 2 && isAttackCooldownDone)
            {
                _navMeshAgent.ResetPath();
                _animator.SetTrigger(Attack);
                _timeSinceLastAttack = 0;
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