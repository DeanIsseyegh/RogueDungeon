using UnityEngine;

public class EnemyState
{
    public STATE Name;
    protected EVENT Stage;
    protected EnemyState NextState;
    protected EnemyStateCtx Ctx;

    private float rotateSpeed = 10f;

    public enum STATE
    {
        IDLE,
        PATROL,
        PURSUE,
        ATTACK
    }

    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    }

    public EnemyState(EnemyStateCtx ctx)
    {
        Ctx = ctx;
    }

    public virtual void Enter()
    {
        Stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        Stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        Stage = EVENT.EXIT;
    }

    public EnemyState Process()
    {
        if (Stage == EVENT.ENTER) Enter();
        if (Stage == EVENT.UPDATE) Update();
        if (Stage == EVENT.EXIT)
        {
            Exit();
            return NextState;
        }

        return this;
    }

    protected bool CanSeePlayer()
    {
        float distance = Vector3.Distance(Ctx.Player.transform.position, Ctx.Enemy.transform.position);
        return distance < Ctx.VisDistance;
    }

    protected void RotateTowardsPlayer()
    {
        var direction = (Ctx.Player.transform.position - Ctx.Enemy.transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        Ctx.Enemy.transform.rotation =
            Quaternion.Lerp(Ctx.Enemy.transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }
}