using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CanSkill 
{
    private bool isUnlocked;
    [SerializeField]private bool canUseSkill;
    [SerializeField]private string skillName;
    [SerializeField]private float skillAmount;

    [SerializeField] private string skillTroduce;



    public bool CanUseThisSkill()
    {
     
        if(canUseSkill)
             return true;
        else
            return false;
    }

    public float GetNeedAmount()
    {
        return skillAmount;
    }

    public string GetSkillName()
    {
        return skillName;
    }


    public void SetCanUseSkill()
    {
        if(isUnlocked)
            canUseSkill = true;
    }


    public void SetCannotUseSkill()
    {
        canUseSkill = false;
    }


    public bool SetIsUnlocked( )
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            return true;
        }
           return false;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public string GetIntroduce()
    {
        return skillTroduce;
    }

}
