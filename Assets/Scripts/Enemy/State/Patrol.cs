using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : EnemyState
{
    private int _currentIndex = 0;
    private float _singlePatrolLength;
    private List<Vector3> _patrolTargets;
    private static readonly int Running = Animator.StringToHash("running");

    public Patrol(EnemyStateCtx ctx) : base(ctx)
    {
        Name = STATE.PATROL;
        _singlePatrolLength = Random.Range(8f, 16f);

        var randomX = Random.Range(-_singlePatrolLength, _singlePatrolLength);
        var randomZ = Random.Range(-_singlePatrolLength, _singlePatrolLength);
        var enemyPos = ctx.Enemy.transform.position;
        var patrolPos1 = new Vector3(enemyPos.x + randomX, enemyPos.y,
            enemyPos.z + randomZ);
        var patrolPos2 = enemyPos;
        _patrolTargets = new List<Vector3>() {patrolPos1, patrolPos2};
    }

    public override void Enter()
    {
        Ctx.Animator.SetBool(Running, true);
        _currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        if (Ctx.MeshAgent.remainingDistance < 1)
        {
            if (_currentIndex >= _patrolTargets.Count - 1)
            {
                _currentIndex = 0;
            }
            else
            {
                _currentIndex++;
            }

            Ctx.MeshAgent.SetDestination(_patrolTargets[_currentIndex]);
        }

        if (CanSeePlayer())
        {
            NextState = new PursueState(Ctx);
            Stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        Ctx.Animator.SetBool(Running, false);
        base.Exit();
    }
}