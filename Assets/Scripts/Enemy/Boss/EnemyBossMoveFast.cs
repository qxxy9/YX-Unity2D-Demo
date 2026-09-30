using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBossMoveFast : EnemyState
{
    private EnemyBoss enemy;
    private Transform player;
    private int moveDirct;
    private int stage;
    private bool isAttack;
    public EnemyBossMoveFast(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemyBoss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.position.x < enemy.transform.position.x)
        {
            moveDirct = -1;
        }
        else
        {
            moveDirct = 1;
        }

        stage = 1;
        stateTimer = 2f;
    }

    public override void Exit()
    {
        base.Exit();
        isAttack = false;
        enemy.anim.SetBool("citeAttack", false);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0&&!isAttack)
        {
            enemy.transform.position = new Vector3(player.position.x + moveDirct * 2, enemy.transform.position.y, enemy.transform.position.z);
            enemy.Flip();
            isAttack = true;
            enemy.anim.SetBool("citeAttack",true);
        }
        if (triggerCalled&&stage==1)
        {
            enemy.stats.GetEvasion();
            triggerCalled = false;
            stage=2;
        }
        if (triggerCalled && stage == 2)
        {
            stateMachine.ChangeState(enemy.idle);
        }
    }

}
