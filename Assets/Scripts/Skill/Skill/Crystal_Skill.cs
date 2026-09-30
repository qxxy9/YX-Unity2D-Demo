using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{

    [SerializeField] private GameObject crystalPerfab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [SerializeField] private CanSkill canCrystalSkill;  //晶体技能是否解锁
    [SerializeField] private CanSkill cloneInstead;  //晶体被使用时是否换成克隆一个分身

    [SerializeField] private CanSkill canExplode;  //晶体是否可以爆炸
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private float growSpeed;

    [Header("Move crystal")]
    [SerializeField] private CanSkill canMove; //晶体可移动
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private CanSkill canUseMultiStacks;  //能使用多个晶体
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWondow;
    [SerializeField]private List<GameObject> crystalleft=new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (!canCrystalSkill.CanUseThisSkill())
            return;

        if (CanUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreatCrystal();

        }
        else
        {
            if (canMove.CanUseThisSkill())
                return;

            Vector2 playerPos=player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInstead.CanUseThisSkill())
            {
                SkillManager.instance.clone.CreatClone(currentCrystal.transform,Vector3.zero);
            }

            currentCrystal.GetComponent<Crystal_Skill_Controller>().CrystalCompleted();
        }
    }

    public void CreatCrystal()
    {
        currentCrystal = Instantiate(crystalPerfab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal
            (crystalDuration, canExplode.CanUseThisSkill(), attackCheckRadius, canMove.CanUseThisSkill(), moveSpeed, growSpeed, FindCloseseEnemy(currentCrystal.transform));
        
    }

    public void CurrentCrystalChooseEnemy() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks.CanUseThisSkill())
        {
            if(crystalleft.Count > 0)
            {
                if (crystalleft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWondow);
                cooldown = 0;
                GameObject crystalSpawn = crystalleft[crystalleft.Count-1];
                GameObject newCrystal=Instantiate(crystalSpawn,player.transform.position, Quaternion.identity);

                crystalleft.Remove(crystalSpawn);
             
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal
                    (crystalDuration, canExplode.CanUseThisSkill(), attackCheckRadius, canMove.CanUseThisSkill(), moveSpeed, growSpeed, FindCloseseEnemy(newCrystal.transform));

                if(crystalleft.Count <=0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
            }

            return true;
        }
        return false;
    }
    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalleft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalleft.Add(crystalPerfab);
        }
    }
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;

        RefilCrystal();
    }


    public override List<CanSkill> GetSkillChange()
    {
        canSkills .Clear();
        canSkills.Add(canCrystalSkill);
        canSkills.Add(cloneInstead);
        canSkills.Add(canExplode);
        canSkills.Add(canMove);
        canSkills.Add(canUseMultiStacks);
        return canSkills;
    }

}
