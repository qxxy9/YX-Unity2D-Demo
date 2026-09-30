using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Player player => PlayerManager.instance.player;
    private Animator anim=>GetComponent<Animator>();

    private float crystalExistTimer;

    private bool isExplode;
    private bool canExplode;
    private float attackCheckRadius;
    private bool canGrow;
    private bool canMove;
    private float moveSpeed;
    private float growSpeed;
    private Transform closeseEnemy;
    private CircleCollider2D cd=>GetComponent<CircleCollider2D>();
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal
        (float _crystalDuratiom,bool _canExplode,float _attackChecckRadius,bool _canMove,float _moveSpeed,float _growSpeed,Transform _closeseEnemy)
    {
        crystalExistTimer=_crystalDuratiom;
        canExplode=_canExplode;
        attackCheckRadius=_attackChecckRadius;
        canMove=_canMove;
        moveSpeed=_moveSpeed;
        growSpeed=_growSpeed;
        closeseEnemy=_closeseEnemy;
    }

    public void Update()
    {
        crystalExistTimer-=Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            
            CrystalCompleted();
            

        }
        if (canMove)
        {
            if (closeseEnemy == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, closeseEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closeseEnemy.position) < 1)
            {
                CrystalCompleted();
                
            }
        }

        if(canGrow)
            transform.localScale=Vector2.Lerp(transform.localScale,new Vector2(3,3),growSpeed*Time.deltaTime);

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (!isExplode)
                {
                    player.stats.DoMagicDamage(hit.GetComponent<EnemyStats>());

                    ItemData_Equipment equipedAmulet=Inventory.Instance.GetEquipment(EquipmentType.Amulet);

                    if (equipedAmulet != null)
                        equipedAmulet.ExcuteItemEffect(hit.transform);

                    //isExplode= true;  

                }
            }

        }
        Invoke("SelfDestroy", .4f);
    }
    public void CrystalCompleted()
    {
        if (canExplode)
        {
            anim.SetTrigger("Explode");
            canGrow = true;
            canMove = false;
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void ChooseRandomEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 50,whatIsEnemy);

        if(colliders.Length > 0)
            closeseEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }


}
