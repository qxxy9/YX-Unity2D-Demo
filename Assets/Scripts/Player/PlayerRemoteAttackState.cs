using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRemoteAttackState : PlayerState
{

    public PlayerRemoteAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        player.rb.velocity=Vector2.zero;
        base.Enter();
        player.anim.speed = 0.6f;
        
        #region Choose attack direction
        float attackDir = player.facingDir;

        float xt = Input.GetAxisRaw("Horizontal");

        if (xt != 0)
            attackDir = xt;

        //if (xInput!=0)
        //attackDir = xInput;
        #endregion

        SkillManager.instance.arrow.GetsStat(player.GetComponent<CharacterStats>());
        SkillManager.instance.arrow.UseSkill();

    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
        player.anim.speed = 1f;
    }

    public override void Update()
    {
        base.Update();
        player.rb.velocity = new Vector2(player.facingDir*-1.2f,0);
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
