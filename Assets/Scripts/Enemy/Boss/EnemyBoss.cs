using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    #region 状态
    public EnemyBoss_Idle idle {  get; private set; }
    public EnemyBoss_Move move { get; private set; }
    public EnemyBossPrimaryAttack attack { get; private set; }
    public EnemyBossMoveFast fmove { get; private set; }
    public EnemyBossAirAttack sattack { get; private set; }
    #endregion
    public bool actionPattern;
    [SerializeField] public Vector3 StartPosition;
    [SerializeField] public float maxChaseDistance;
    [SerializeField] public int attackAmount;
    public bool isAttackState;
    public int lastAttackPatern1;
    public int lastAttackPatern2;
    protected override void Awake()
    {
        base.Awake();
        idle=new EnemyBoss_Idle(this,stateMachine,"idle",this);
        move = new EnemyBoss_Move(this, stateMachine, "move", this);
        attack = new EnemyBossPrimaryAttack(this, stateMachine, "attack", this);
        fmove = new EnemyBossMoveFast(this, stateMachine, "fmove" , this);
        sattack = new EnemyBossAirAttack(this, stateMachine, "air", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initiation(idle);
    }

    protected override void Update()
    {
        base.Update();
        if(stats!=null)
        if (stats.currentHealth <= stats.GetMaxHP() * .5f)
        {
            actionPattern = true;
        }
    }

    public override void EvasionSet()
    {
        stats.GetEvasion();
    }

    public bool CheckRadiuce()
    {
        return
            this.StartPosition.x - this.maxChaseDistance < this.transform.position.x
            && this.StartPosition.x + this.maxChaseDistance > this.transform.position.x;
    }
    
    public void EnterAttackState()
    {
        if (!isAttackState)
        {
            isAttackState = true;
            UI_Manager.Instance.SetBossHealthUI(stats,isAttackState);
        }
            
    }

    public void ExitAttackState()
    {
        if (isAttackState)
        {
            isAttackState =false;
            stats.InitionHealth();
            actionPattern = false;
            UI_Manager.Instance.SetBossHealthUI(stats, isAttackState);
        }
        
    }

    public int CheckAttackPatern2()
    {
        int a;
        do
        {
             a = Random.Range(0, 3);
        } while (a == lastAttackPatern2);
        lastAttackPatern2 = a;
        return a;
    }

    public int CheckAttackPatern1()
    {
        int a;
        do
        {
            a = Random.Range(0, 2);
        } while (a == lastAttackPatern1);
        lastAttackPatern1 = a;
        return a;
    }

    public override void Die()
    {
        base.Die();
        SaveManager.instance.End();
        UI_Manager.Instance.Win();
    }
}
