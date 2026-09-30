using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Move : EnemyBoss_Ground
{
    private EnemyBoss enemy;
    
    
    private int moveDirct;
    public EnemyBoss_Move(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBoss _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        this.enemy = _enemy;
        player = PlayerManager.instance.player.transform;
    }

    public override void Enter()
    {
        base.Enter();
        
        if (player.position.x < enemy.transform.position.x)
        {
            moveDirct=-1;
        }
        else
        {
            moveDirct = 1;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        
            if (enemy.CheckRadiuce())
            {
                enemy.SetVelocity(enemy.moveSpeed * moveDirct*2, rb.velocity.y);
            }
            else if(!enemy.CheckRadiuce()||enemy.transform.position.x-player.position.x==2.5f*moveDirct) 
            {
                enemy.SetZeroVelocity();
                
                enemy.FlipController(-moveDirct);
                stateMachine.ChangeState(enemy.idle);
            }
        
        
    }

}
