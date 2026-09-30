using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePerfab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField]private float shrinkSpeed;

    Blackhole_Skill_Contoller currentBlackhole;

    public CanSkill CanBlackholeSkill;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();

    }

    public override void UseSkill()
    {
        base.UseSkill();
        SkillManager.instance.SetSkillCoolDOwn(CanBlackholeSkill.GetSkillName(),cooldown);
        if (!CanBlackholeSkill.CanUseThisSkill())
            return;
        GameObject newBlackHole=Instantiate(blackHolePerfab,player.transform.position,Quaternion.identity);

        currentBlackhole=newBlackHole.GetComponent<Blackhole_Skill_Contoller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown,blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;
        if (currentBlackhole.playerCanExitState == true)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }


    public override List<CanSkill> GetSkillChange()
    {
        canSkills.Clear();
        canSkills.Add(CanBlackholeSkill);
        return canSkills;
    }

}
