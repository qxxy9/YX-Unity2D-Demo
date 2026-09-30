using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SkeletonCall_Ground : EnemyState
{
    private Enemy_SkeletonCall enemy;
    protected Transform player;
    public SkeletonCall_Ground(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_SkeletonCall _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player=PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            stateMachine.ChangeState(enemy.battle);
        }
        if (enemy.stats.currentHealth < (enemy.stats.maxHp.GetValue() * .5f)&&!enemy.isCalled)
        {
            stateMachine.ChangeState(enemy.call);

        }
    }
}
