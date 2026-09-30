using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Skill : Skill
{
    public GameObject arrowPerfab;
    public CanSkill canPenetarate;
    public float arrowSpeed;
    public float arrowDuration;
    private CharacterStats stats;

    public override void UseSkill()
    {
        CharacterStats _stat= stats;
        GameObject newArrow=Instantiate(arrowPerfab,_stat.GetPosition(),stats.GetQuaternion());
        Arrow_Controller _arrow_Controller=newArrow.GetComponent<Arrow_Controller>();
        _arrow_Controller.SetUpArrow(canPenetarate.CanUseThisSkill(),_stat,arrowSpeed,arrowDuration);
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
