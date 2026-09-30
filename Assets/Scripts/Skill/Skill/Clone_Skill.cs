using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePerfab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private CanSkill creatCloneOnDashStart;//冲刺开始时生成克隆
    [SerializeField]private CanSkill creatCloneOnDashOver;//冲刺结束时生成克隆
    [SerializeField] private CanSkill canCreatCloneOnCounterAttack; //攻击生成克隆

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;  //克隆攻击时概率生成克隆
    private float cloneChance;
    [Header("Crystal instead of clone")]
    [SerializeField]private CanSkill cloneInsteadOfCrystal;  //水晶替代克隆

    public void CreatClone(Transform _clonePosition ,Vector3 _offset)
    {
        if(cloneInsteadOfCrystal.CanUseThisSkill())
        {
            SkillManager.instance.crystal.CreatCrystal();
           
            return;
        }
        GameObject newClone=Instantiate(clonePerfab);
        newClone.GetComponent<Clone_Skll_Controller>().SetupClone
            (_clonePosition,cloneDuration,canAttack,_offset, FindCloseseEnemy(_clonePosition),canDuplicateClone,cloneChance);
    }

    public void CreatCloneOnDashStart()
    {
        if (creatCloneOnDashStart.CanUseThisSkill())
        {
            CreatClone(player.transform,Vector3.zero);
        }
    }

    public void CreatCloneOnDashOver()
    {
        if (creatCloneOnDashOver.CanUseThisSkill())
        {
            CreatClone(player.transform, Vector3.zero);
        }
    }
    public void CreatCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreatCloneOnCounterAttack.CanUseThisSkill())
            StartCoroutine(CreatCloneWithDelay(_enemyTransform,new Vector3(2*player.facingDir,0)));
    }
    private IEnumerator CreatCloneWithDelay(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.3f);
        CreatClone(_transform,_offset);
    }

    public override List<CanSkill> GetSkillChange()
    {
        canSkills.Clear();
        canSkills.Add(cloneInsteadOfCrystal);
        canSkills.Add(creatCloneOnDashStart);
        canSkills.Add(creatCloneOnDashOver);
        canSkills.Add(canCreatCloneOnCounterAttack);
        

        return canSkills;
    }
}
