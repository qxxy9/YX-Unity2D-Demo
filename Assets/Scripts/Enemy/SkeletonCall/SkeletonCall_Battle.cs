using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCall_Battle : EnemyState
{
    private Enemy_SkeletonCall enemy;
    private Transform player;
    private int moveDir;
    public SkeletonCall_Battle(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_SkeletonCall _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attack);

                }

                else
                {
                    enemy.SetVelocity(moveDir * .5f, rb.velocity.y);
                    return;
                }

            }
        }

        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idle);
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time > enemy.lastTimerAttacked + enemy.attackCooldown)
        {
            return true;
        }
        return false;
    }

}
