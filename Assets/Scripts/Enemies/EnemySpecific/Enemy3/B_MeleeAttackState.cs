﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_MeleeAttackState : MeleeAttackState
{
    private Enemy3 enemy;
    public B_MeleeAttackState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Enemy3 enemy) : base(stateMachine, entity, animBoolName, attackPosition, stateData)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinish)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
