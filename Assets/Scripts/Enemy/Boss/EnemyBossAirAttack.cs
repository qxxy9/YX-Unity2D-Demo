using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAirAttack : EnemyState
{
    private EnemyBoss enemy;
    private int attackAmount;
    private Transform player;
    private int stage;
    private float gravity;
    public EnemyBossAirAttack(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemyBoss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player=PlayerManager.instance.player.transform;
        stage = 1;
        stateTimer = 1;
        attackAmount = enemy.attackAmount;
        SkillManager.instance.laser.SinglePattern();
        gravity=enemy.rb.gravityScale;
        enemy.rb.gravityScale = 0;
        enemy.SetVelocity(enemy.StartPosition.x - enemy.transform.position.x, 6);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("fallAttack", false);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.transform.position.x<enemy.StartPosition.x+.1f&&
            enemy.transform.position.x>enemy.StartPosition.x - .1f)
        {
            enemy.SetZeroVelocity();
        }
        if (stage == 1 && stateTimer < 0)
        {
            stage++;
            stateTimer = 1;
            
            
        }
        if (attackAmount==0)
        {
            enemy.SetVelocity((player.position.x - enemy.transform.position.x)*2.5f, (player.position.y - enemy.transform.position.y)*2.5f);
            enemy.anim.SetBool("fallAttack", true);
        }
        if (stateTimer<0&&stage==2)
        {
            if (Time.time > enemy.attackCooldown + enemy.lastTimerAttacked)
            {

                SkillManager.instance.laser.GetStat(enemy.stats);
                SkillManager.instance.laser.UseSkill();
                enemy.lastTimerAttacked = Time.time;
                attackAmount--;
            }
        }

        if (triggerCalled)
        {
            enemy.rb.gravityScale = gravity;
            stateMachine.ChangeState(enemy.idle);
        }
    }
}
