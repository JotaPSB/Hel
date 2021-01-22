using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_IdleState : IdleState
{
    private Enemy3 enemy;
    public B_IdleState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_IdleState stateDate, Enemy3 enemy) : base(stateMachine, entity, animBoolName, stateDate)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMinArgoRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
