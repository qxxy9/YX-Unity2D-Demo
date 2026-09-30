using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class Entity : MonoBehaviour,IsSaveManager
{

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;//攻击是否命中判定范围
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public System.Action onFlipped;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    protected bool canHeal;
    [SerializeField]protected string characterID=>gameObject.name;
    private bool isLoad;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }

    public CharacterStats stats { get; private set; }

    public CapsuleCollider2D cd { get; private set; }
    #endregion

    #region baseLife
    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {

        sr = GetComponentInChildren<SpriteRenderer>();
        fx = GetComponentInChildren<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        cd = GetComponent<CapsuleCollider2D>();
        CheckRotation();

        if (!facingRight)
        {
            onFlipped();
        }
    }

    

    protected virtual void Update()
    {

    }
    #endregion

    //非协程

    public virtual void SlowEntity(float _slowPercent, float _slowDuration)
     {
         
     }
    
     protected virtual void ReturnToNormalSpeed()
     {
         anim.speed = 1;
     }

     public virtual void CheckFacingDir()
    {

    }

    public void EvasionOff()
    {
        stats.OffEvasion();
        
    }
    public virtual void EvasionSet()
    {
        stats.GetEvasion();
        Invoke("EvasionOff", .3f);
    }


    #region damageEffect
    public virtual void DamageImpact()
    {
        
        StartCoroutine("HitKnockback");
        
    }

    public virtual void Die()
    {
        
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * facingDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    #endregion


    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);//是否还在地面上
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);//是否撞墙

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        onFlipped?.Invoke();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();

    }

    private void CheckRotation()
    {
        float eulerY = transform.eulerAngles.y;
        facingRight = !(eulerY > 90 && eulerY < 270);
        facingDir = facingRight ? 1 : -1;
    }

    #endregion

    #region public skill

    public void SetHeal()
    {
        canHeal = true;
        SkillManager.instance.UseStartSkill("heal");
    }

    public void OffHeal()
    {
        canHeal=false;
    }

    public bool IsHeal() => canHeal;



    #endregion

    #region save and load

    public void LoadData(GameData _data)
    {
        if (_data == null)
        {
            return;
        }
            
        if (_data.targetData.ContainsKey(characterID))
        {
            //facingDir = _data.targetData[characterID].facingDir;
            transform.position = _data.targetData[characterID].savePosition;
            transform.rotation = _data.targetData[characterID].rotate;
            stats.ReceiveSaveData(_data.targetData[characterID].characterStatSaveData);
            CheckRotation();
        }
        
    }

    public void SaveData(ref GameData _data)
    {
        TargetSaveData data = new TargetSaveData(transform.position, facingDir,transform.rotation,stats.GetSaveData());
        _data.targetData.Add(characterID, data);
    }
    #endregion
}
