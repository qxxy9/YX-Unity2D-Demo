using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration=.2f;
    // public float lastcountertime;
    //public float countercooldown;
    public bool isblackHole=true;
    private bool isRemote;
    public Vector2 initionPlace;    
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;


    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }

    

    public SkillManager skill {  get; private set; }
 
    public GameObject sword {  get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayeridleState idleState { get; private set; }

    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerWallSlideState wallSlide { get; private set; }

    public PlayerWallJumpState wallJump{ get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }

    public PlayerCounterAttackState counterAttack { get; private set; }

    public PlayerAimSwordState aimSword {  get; private set; }

    public PlayerCatchSwordState catchSword {  get; private set; }

    public PlayerBlackholeState blackHole { get; private set; }

    public PlayerDeadState deadState { get; private set; }

    public PlayerRemoteAttackState remoteAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayeridleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        remoteAttackState = new PlayerRemoteAttackState(this, stateMachine, "RemoteAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

    }

    protected override void Start()
    {
     base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);


        defaultDashSpeed = dashSpeed;
        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
        if (Input.GetKeyDown(KeyCode.Z)&&TimePuaseManager.instance.CanReceiveNormalButton())
            skill.crystal.CanUseSkill();
     
        if(Input.GetKeyDown(KeyCode.X)&&canHeal && TimePuaseManager.instance.CanReceiveNormalButton())
            skill.heal.CanUseSkill();

    }

   //非协程

    public override void SlowEntity(float _slowPercent, float _slowDuration)
    {
        moveSpeed = moveSpeed *(1- _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        anim.speed = 1 - _slowPercent;
        Invoke("ReturnToNormalSpeed", _slowDuration);
    }
   
    protected override void ReturnToNormalSpeed()
    {
        base.ReturnToNormalSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    //协程


 // protected override void SlowEntity(float _slowAmount, float _slowDuration)
 // {
 //     StartCoroutine(SlowEntityCoroutine(_slowAmount, _slowDuration));
 // }
 //
 // private IEnumerator SlowEntityCoroutine(float _slowAmount, float _slowDuration)
 // {
 //     moveSpeed = moveSpeed * (1 - _slowAmount);
 //     jumpForce = jumpForce * (1 - _slowAmount);
 //     dashSpeed = dashSpeed * (1 - _slowAmount);
 //     anim.speed = 1 - _slowAmount;
 //
 //
 //     yield return new WaitForSeconds(_slowDuration);
 //     ReturnToNormalSpeed();
 // }
 //
 // private  void ReturnToNormalSpeed()
 // {
 //     anim.speed = 1;
 //     moveSpeed = defaultMoveSpeed;
 //     jumpForce = defaultJumpForce;
 //     dashSpeed = defaultDashSpeed;
 // }
 //未完善

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
        SaveManager.instance.End();
        UI_Manager.Instance.Dead();
        
    }

    public void BackPlace()
    {
        transform.position = initionPlace;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword= _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    public void ExitBlackHoleAbility()
    {
        stateMachine.ChangeState(airState);
    }
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    public void AnimationTrigger()=>stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {

        
        if (IsWallDetected())
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill()&&isblackHole && TimePuaseManager.instance.CanReceiveNormalButton())
        {
            
            dashDir = Input.GetAxisRaw("Horizontal");
            stateMachine.ChangeState(dashState);
            if (dashDir == 0)
                dashDir = facingDir;

        }
    }

    
    public void SetRemote()
    {
        isRemote = true;
    }

    public void OffRemote()
    {
        isRemote= false;
    }
    
    public bool IsRemote()
    {
        return isRemote;
    }

    public void GetTrapped()
    {
        rb.AddForce(new Vector2(-5*facingDir,4), ForceMode2D.Impulse);
        stats.TakeDamage((int)(stats.GetMaxHP()*.1f));
    }

}

