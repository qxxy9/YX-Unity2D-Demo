using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Skill : Skill
{
    public GameObject FirePerfab;
    public CanSkill canPenetarate;
    public float FireSpeed;
    public float FireDuration;
    private CharacterStats stats;

    public override void UseSkill()
    {
        CharacterStats _stat = stats;
        GameObject newFire = Instantiate(FirePerfab, _stat.GetPosition(), stats.GetQuaternion());
        Arrow_Controller _arrow_Controller = newFire.GetComponent<Arrow_Controller>();
        _arrow_Controller.SetUpArrow(canPenetarate.CanUseThisSkill(), _stat, FireSpeed, FireDuration);
    }

    public void GetsStat(CharacterStats _stats)
    {
        stats = _stats;
    }

    public override List<CanSkill> GetSkillChange()
    {
        canSkills.Clear();
        canSkills.Add(canPenetarate);
        return canSkills;
    }
}
