using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private static readonly int Running = Animator.StringToHash("running");
    private static readonly int BasicSpell = Animator.StringToHash("BasicSpell");
    public bool CanMove { private get; set; }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMoveAnimation();
        }
    }

    private void HandleMoveAnimation()
    {
        Vector3 velocity = _navMeshAgent.velocity;
        if (Math.Abs(velocity.x) + Math.Abs(velocity.z) > 0.2)
        {
            _animator.ResetTrigger(BasicSpell);
            _animator.SetBool(Running, true);
        }
        else
        {
            _animator.SetBool(Running, false);
        }
    }

    public void StartBasicSpellAnimation()
    {
        _animator.SetTrigger(BasicSpell);
    }
}