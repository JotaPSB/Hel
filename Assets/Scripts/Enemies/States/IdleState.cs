using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinArgoRange;

    protected float idleTime;
    public IdleState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_IdleState stateDate) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateDate;
    }



    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime +idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
       
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinArgoRange = entity.CheckPlayerInMinAgroRange();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
