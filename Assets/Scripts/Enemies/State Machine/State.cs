using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected float startTime;

    protected string animboolName;

    public State(FiniteStateMachine stateMachine ,Entity entity, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animboolName = animBoolName;
        this.entity = entity;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.anim.SetBool(animboolName, true);
        DoChecks();
    }

    public virtual void Exit()
    {

        entity.anim.SetBool(animboolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
