using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected D_Charge stateData;

    protected bool isPlayerMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    public ChargeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_Charge stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }



    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        entity.SetVelocity(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(Time.time>= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }

    }
    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }
}
