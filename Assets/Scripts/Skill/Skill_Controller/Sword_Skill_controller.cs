using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Sword_Skill_controller : MonoBehaviour
{

    private float returnSpeed;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotated=true;
    private bool isReturning;

    private float freezeTimeDuration;

    [Header("Pierce info")]
     private float pierceAmount;

    [Header("Bouse info")]
    private float bouncespeed;
    private bool isBouncing ;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitcooldown;
    private float spinDirection;
    private bool canDeBuff;
    private void Awake()
    {
        
        anim=GetComponentInChildren<Animator>();
        rb=GetComponent<Rigidbody2D>();
        cd=GetComponent<CircleCollider2D>();

    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir,float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed,bool _canDeBuff)
    {
        player= _player;
        freezeTimeDuration= _freezeTimeDuration;
        rb.velocity= _dir;
        rb.gravityScale= _gravityScale;
        returnSpeed= _returnSpeed;
        canDeBuff= _canDeBuff;
        if(pierceAmount<=0)
            anim.SetBool("Rotation",true);
        Invoke("DestroyMe", 7);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
    }

    public void SetUpBounce(bool _isbouncing,int _amountofBounce, float _bounceSpeed)
    {
        isBouncing= _isbouncing;
        bounceAmount= _amountofBounce;
        bouncespeed = _bounceSpeed;

        enemyTarget =new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount= _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration,float _hitCoolDown)
    {
        hitcooldown= _hitCoolDown;
        isSpinning= _isSpinning;
        maxTravelDistance= _maxTravelDistance;
        spinDuration= _spinDuration;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic= false;
        transform.parent= null;
        isReturning = true;
        
    }

    private void Update()
    {
        if (canRotated)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        Bouncelogic();
        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitcooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());

                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void Bouncelogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bouncespeed * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                
                targetIndex++;
                bounceAmount--;
                if (bounceAmount < 0)
                {
                    isBouncing = false;

                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }


        
        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        if (canDeBuff)
        {
            CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
            enemyStats.AddtimeBuff(-1, "armor", 5);
            canDeBuff = false;
        }
            

        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);

        ItemData_Equipment equipedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);

        if (equipedAmulet != null)
            equipedAmulet.ExcuteItemEffect(enemy.transform);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount>0&&collision.GetComponent<Enemy>() != null) 
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotated = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing&&enemyTarget.Count>0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
