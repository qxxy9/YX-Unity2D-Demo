using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArrowBattle : EnemyState
{
    private Enemy_SkeletonArrow enemy;
    private Transform player;
    private int moveDir;
    public SkeletonArrowBattle(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SkeletonArrow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        player = PlayerManager.instance.player.transform;
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
                if (Canattack())
                {
                    stateMachine.ChangeState(enemy.attackState);
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
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idle);
                return;
            }
                
        }


        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public bool Canattack()
    {
        if(enemy.attackCooldown+enemy.lastTimerAttacked<Time.time)
            return true;

        return false;
    }

}
