using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;

    [SerializeField]protected List<CanSkill> canSkills = new List<CanSkill>();

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer-=Time.deltaTime;
        
    }

    //冷却判定
    public virtual bool CanUseSkill()
    {
        if(cooldownTimer <= 0)
        {
            UseSkill();
            
            cooldownTimer = cooldown;
            return true; ;

        }
        return false;
    }

    //技能释放
    public virtual void UseSkill()
    {

    }

    //寻找最近敌人
    protected virtual Transform FindCloseseEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 10);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;

                    closestEnemy = hit.transform;
                }

            }
        }
        return closestEnemy;
    }

    
    public virtual List<CanSkill> GetSkillChange()
    {
        if (canSkills.Count == 0)
            return null;
        return canSkills;
    }

    public virtual float GetCoolDown()
    {
        return cooldown;
    }

    public virtual float GetCoolUp()
    {
        return cooldownTimer;
    }
}
