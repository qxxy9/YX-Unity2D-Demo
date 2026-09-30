using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    
    public override void UseSkill()
    {
        base.UseSkill();
        SkillManager.instance.SetSkillCoolDOwn("dash", cooldown);
    }

    protected override void Start()
    {
        base.Start();
        SkillManager.instance.UseStartSkill("dash");
    }
}
