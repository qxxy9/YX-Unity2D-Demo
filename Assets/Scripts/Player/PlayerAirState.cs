using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 3;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            player.BackPlace();
        }
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
        if(xInput!=0)
            player.SetVelocity(player.moveSpeed*.8f*xInput,rb.velocity.y);
    }
}
