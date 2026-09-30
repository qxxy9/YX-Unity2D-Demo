using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCall_Dead : EnemyState
{
    private Enemy_SkeletonCall enemy;

    public SkeletonCall_Dead(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_SkeletonCall _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);

        enemy.anim.speed = 0;

        enemy.cd.enabled = false;
        stateTimer = .2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {

            rb.velocity = new Vector2(0, 10);
        }
    }
}
