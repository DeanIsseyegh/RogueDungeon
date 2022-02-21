using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationHandler : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private static readonly int Running = Animator.StringToHash("running");
    [SerializeField] private List<string> _animationTriggersToResetOnMove;
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
        if (gameObject.name.Equals("EnemyCult"))
        {
            Debug.Log("enemy Cult Handle Move");
        }
        Vector3 velocity = _navMeshAgent.velocity;
        if (Math.Abs(velocity.x) + Math.Abs(velocity.z) > 0.2)
        {
            _animationTriggersToResetOnMove.ForEach(_animator.ResetTrigger);
            _animator.SetBool(Running, true);
        }
        else
        {
            _animator.SetBool(Running, false);
        }
    }

    public void SetTriggerAnimation(String triggerAnimation)
    {
        _animator.SetTrigger(triggerAnimation);
    }
}