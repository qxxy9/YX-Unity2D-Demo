using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skll_Controller : MonoBehaviour
{
    private Player player => PlayerManager.instance.player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorlosingSpeed;
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float cloneChance;
    private void Awake()
    {
        sr=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();

    }

    private void Update()
    {
        cloneTimer-= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color=new Color(1,1,1,sr.color.a-(Time.deltaTime*colorlosingSpeed));
            if(sr.color.a<=0 )
                Destroy(gameObject);
        }
    }
    public void SetupClone
        (Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _closestEnemy,bool _canDuplicateClone,float _cloneChance)
    {
        if (_canAttack) 
            anim.SetInteger("AttackNumber",Random.Range(1,3));
        transform.position = _newTransform.position+_offset;
        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        cloneChance = _cloneChance;

        FaceClosestTarge();
    }

    private void AnimationTrigger()
    {
        cloneTimer = .1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                
                player.stats.DoDamage(hit.GetComponent<EnemyStats>());

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < cloneChance)
                    {
                        SkillManager.instance.clone.CreatClone(hit.transform, new Vector3(1.5f*facingDir, 0));
                    }
                }
            }
        }
    }
    private void FaceClosestTarge()
    {
       
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
                facingDir = -1;
            }
        }
         

    }
}
