using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    
    public int comboCounter { get; private set; }

    private float lastTimeAttacked;
    private float comboWindow=1.2f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if (comboCounter >2||Time.time>=lastTimeAttacked+comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter",comboCounter);
        //player.anim.speed = 1.5f;  攻击速度改变
        #region Choose attack direction
        float attackDir=player.facingDir;

        float xt= Input.GetAxisRaw("Horizontal");

        if(xt != 0)
            attackDir = xt;

        //if (xInput!=0)
        //attackDir = xInput;

        #endregion
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
       // player.anim.speed = 1f;  攻速回正；
        comboCounter++;
        lastTimeAttacked=Time.time;

    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
