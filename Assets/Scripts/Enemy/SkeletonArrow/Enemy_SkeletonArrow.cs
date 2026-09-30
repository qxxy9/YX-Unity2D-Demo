using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonArrow : Enemy
{
    public SkeletonArrowIdle idle;
    public SkeletonArrowMove move;
    public SkeletonArrowAttack attackState;
    public SkeletonArrowBattle battle;
    public SkeletonArrow_Dead dead;

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dead);
    }

    protected override void Awake()
    {
        base.Awake();
        idle=new SkeletonArrowIdle(this, stateMachine, "Idle", this);
        move=new SkeletonArrowMove(this, stateMachine,"Move",this);
        attackState=new SkeletonArrowAttack(this,stateMachine,"Attack",this);
        battle = new SkeletonArrowBattle(this, stateMachine, "Move", this);
        dead= new SkeletonArrow_Dead(this, stateMachine, "Idle", this);

    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initiation(idle);
    }

    protected override void Update()
    {
        base.Update();
    }
}
