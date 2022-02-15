using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Math.Abs(player.transform.position.magnitude - transform.position.magnitude);
        if (distToPlayer < 0.20f)
        {
            Debug.Log(distToPlayer);

            _navMeshAgent.ResetPath();
            _animator.SetTrigger("Jump Attack");
        }
        else
        {
            _navMeshAgent.SetDestination(player.transform.position);
        }
    }
}