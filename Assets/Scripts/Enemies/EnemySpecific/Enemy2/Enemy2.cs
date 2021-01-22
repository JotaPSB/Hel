using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState moveState { get; private set; }
    public E2_IdleState idleState { get; private set; }
    public E2_PlayerDetectedState playerDetectedState { get; private set; }
    public E2_LookForPlayerState lookForPlayerState { get; private set; }
    public E2_DeadState deadState { get; private set; }
    public E2_DodgeState dodgeState { get; private set; }
    public E2_RangeAttackState rangeAttackState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    public D_RangeAttackState rangeAttackStateData;

    [SerializeField]
    private Transform rangeAttackPosition;

   
    public override void Start()
    {
        base.Start();

        moveState = new E2_MoveState(stateMachine, this, "move", moveStateData, this);
        idleState = new E2_IdleState(stateMachine, this, "idle", idleStateData, this);
        playerDetectedState = new E2_PlayerDetectedState(stateMachine, this, "playerDetected", playerDetectedStateData, this);
        lookForPlayerState = new E2_LookForPlayerState(stateMachine, this, "lookForPlayer", lookForPlayerData, this);
        deadState = new E2_DeadState(stateMachine, this, "dead", deadStateData, this);
        dodgeState = new E2_DodgeState(stateMachine, this, "dodge", dodgeStateData, this);
        rangeAttackState = new E2_RangeAttackState(stateMachine, this, "rangeAttack", rangeAttackPosition, rangeAttackStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangeAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
