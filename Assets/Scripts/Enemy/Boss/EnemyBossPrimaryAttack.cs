using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossPrimaryAttack : EnemyState
{
    private EnemyBoss enemy;
    private int attackAmount;

    private int attackpatern;
    public EnemyBossPrimaryAttack(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemyBoss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        attackpatern = enemy.CheckAttackPatern1();

        if (attackpatern == 0)
        {
            attackAmount = enemy.attackAmount;
            SkillManager.instance.laser.SinglePattern();
        }

        else
        {
            attackAmount = Random.Range(2, 4);
            SkillManager.instance.laser.NotSinglePattern();
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (attackAmount==0)
        {
            stateMachine.ChangeState(enemy.idle);
        }
        if (Time.time > enemy.attackCooldown + enemy.lastTimerAttacked)
        {
            
            SkillManager.instance.laser.GetStat(enemy.stats);
            SkillManager.instance.laser.UseSkill();
            enemy.lastTimerAttacked = Time.time;
            attackAmount--;
        }
    }

}
