using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        //player.lastcountertime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.R)&&SkillManager.instance.CanUseSkill("Blaackhole") && TimePuaseManager.instance.CanReceiveNormalButton()) 
            stateMachine.ChangeState(player.blackHole);
        if(Input.GetKeyDown(KeyCode.Mouse1)&&HasNoSword()&&SkillManager.instance.CanUseSkill("regularSword") && TimePuaseManager.instance.CanReceiveNormalButton())
            stateMachine.ChangeState(player.aimSword);
        if(Input.GetKeyDown(KeyCode.Q) && TimePuaseManager.instance.CanReceiveNormalButton())//&&Time.time>player.lastcountertime+player.countercooldown
            stateMachine.ChangeState(player.counterAttack);
        if (Input.GetKey(KeyCode.Mouse0) && TimePuaseManager.instance.CanReceiveNormalButton())
        {
            if (player.IsRemote())
            {
                stateMachine.ChangeState(player.remoteAttackState);
            }
            else
            {
                stateMachine.ChangeState(player.primaryAttack);
            }

        }
            
        if (player.IsGroundDetected() == false)
        {
            player.initionPlace=new Vector2(player.transform.position.x-1*player.facingDir, player.transform.position.y);
            stateMachine.ChangeState(player.airState);
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected() && TimePuaseManager.instance.CanReceiveNormalButton())
        {
            stateMachine.ChangeState(player.jumpState);
            player.initionPlace=player.transform.position;
        }
            
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_controller>().ReturnSword();
        return false;
    }
}
