using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCall_Call : EnemyState
{
    private Enemy_SkeletonCall enemy;
    public SkeletonCall_Call(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SkeletonCall enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.isCalled = true;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        if (triggerCalled)
        {
            enemy.CallEnemy();
            stateMachine.ChangeState(enemy.idle);
        }
    }
}
