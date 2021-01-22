using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity
{

    public B_MoveState moveState { get; private set; }
    public B_PlayerDetected playerDetectedState { get; private set; }
    public B_MeleeAttackState meleeAttackState { get; private set; }
    public B_IdleState idleState { get; private set; }
    public B_ChargeState chargeState { get; private set; }
    public B_LookForPlayerState lookForPlayerState { get; private set; }
    public B_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedData;
    [SerializeField]
    private D_Charge chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private D_DeadState deadStateData;

    public override void Start()
    {
        base.Start();

        moveState = new B_MoveState(stateMachine, this, "move", moveStateData, this);
        idleState = new B_IdleState(stateMachine, this, "idle", idleStateData, this);
        playerDetectedState = new B_PlayerDetected(stateMachine, this, "playerDetected", playerDetectedData, this);
        chargeState = new B_ChargeState(stateMachine, this, "charge", chargeStateData, this);
        lookForPlayerState = new B_LookForPlayerState(stateMachine, this, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new B_MeleeAttackState(stateMachine, this, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        
        deadState = new B_DeadState(stateMachine, this, "dead", deadStateData, this);
        stateMachine.Initialize(moveState);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }


    }

}
