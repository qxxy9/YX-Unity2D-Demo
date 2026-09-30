using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCall_Idle : SkeletonCall_Ground
{
    private Enemy_SkeletonCall enemy;

    public SkeletonCall_Idle(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SkeletonCall _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.move);
        }
    }

}
