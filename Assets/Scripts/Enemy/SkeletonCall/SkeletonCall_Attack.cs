using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCall_Attack : EnemyState
{
    private Enemy_SkeletonCall enemy;

    public SkeletonCall_Attack(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_SkeletonCall _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.lastTimerAttacked=Time.time;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            
            SkillManager.instance.fire.GetsStat(enemy.stats);
            SkillManager.instance.fire.UseSkill();
            stateMachine.ChangeState(enemy.idle);
        }
    }
}
