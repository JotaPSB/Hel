using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_DeadState : DeadState
{
    Enemy3 enemy;
    public B_DeadState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_DeadState stateData, Enemy3 enemy) : base(stateMachine, entity, animBoolName, stateData)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
