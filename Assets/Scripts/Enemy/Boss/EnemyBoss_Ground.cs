using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Ground : EnemyState
{
    private EnemyBoss enemy;
    protected Transform player;
    public EnemyBoss_Ground(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemyBoss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy= _enemy;
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
    }

    public virtual bool CheckPlayer()
    {
        if (player.position.x < (enemy.StartPosition.x - enemy.maxChaseDistance )
            || player.position.x >( enemy.StartPosition.x + enemy.maxChaseDistance))
        {
            return false; ;
        }
        else
            return true;
    }

}
