using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonCall : Enemy
{
    public SkeletonCall_Idle idle;
    public SkeletonCall_Move move;
    public SkeletonCall_Battle battle;
    public SkeletonCall_Attack attack;
    public SkeletonCall_Call call;
    public SkeletonCall_Dead dead;
    [SerializeField] private List<GameObject> callEnemy;
    public bool isCalled;
    protected override void Awake()
    {
        base.Awake();
        idle=new SkeletonCall_Idle(this,stateMachine,"Idle",this);
        move=new SkeletonCall_Move(this,stateMachine,"Move",this);
        battle = new SkeletonCall_Battle(this, stateMachine, "Move", this);
        attack = new SkeletonCall_Attack(this, stateMachine, "Attack", this);
        call = new SkeletonCall_Call(this, stateMachine, "Call", this);
        dead=new SkeletonCall_Dead(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initiation(idle);
    }

    protected override void Update()
    {
        base.Update();
        
    }

    public void CallEnemy()
    {
        for (int a = 0; a < 3; a++)
        {
            int i = Random.Range(0, callEnemy.Count);
            GameObject newEnemy=Instantiate(callEnemy[i], new Vector2(transform.position.x + facingDir * (a+1), transform.position.y), stats.GetQuaternion());
            Destroy(newEnemy, 8f);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dead);
    }
}
