using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Idle : EnemyBoss_Ground
{
    private EnemyBoss enemy;
    private int attackPattern;
    private float waitTime;
    public EnemyBoss_Idle(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBoss _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer=enemy.idleTime;

        attackPattern = enemy.CheckAttackPatern2();
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();
        waitTime -= Time.deltaTime;
        if (CheckPlayer())
        {
            enemy.EnterAttackState();
            waitTime=enemy.battleTime;
            if (stateTimer < 0)
            {
                if (enemy.actionPattern)
                {
                    if (attackPattern == 0)
                    {
                        stateMachine.ChangeState(enemy.move);//高速移动对路径造成伤害
                    }
                    else if (attackPattern == 1)
                    {
                        stateMachine.ChangeState(enemy.fmove);//闪烁至近身造成伤害
                    }
                    else
                    {
                        stateMachine.ChangeState(enemy.sattack);//飞天发生激光然后坠落造成伤害
                    }
                }
                else
                {
                    stateMachine.ChangeState(enemy.attack);
                }
            }
        }
        else
        {
            
            if (waitTime < 0)
            {
                enemy.ExitAttackState();
            }
        }

    }



}
