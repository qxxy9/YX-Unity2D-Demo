using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArrowAttack : EnemyState
{
    private Enemy_SkeletonArrow enemy;
    public SkeletonArrowAttack(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SkeletonArrow enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimerAttacked=Time.time;
        SkillManager.instance.arrow.GetsStat(enemy.stats);
        SkillManager.instance.arrow.UseSkill();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battle);
        }
    }
}
