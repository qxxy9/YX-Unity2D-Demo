using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour,IsSaveManager
{
    public static SkillManager instance;

    #region 技能字典
    [SerializeField]private List<CanSkill> canSkills = new List<CanSkill>();
    public Dictionary<string, CanSkill> canSkillDictionary = new Dictionary<string, CanSkill>();
    
    #endregion

    #region 技能相关委托
    public Action<CanSkill,bool>SetUITrouduce;//设置技能介绍UI的委托
    public Action<string,bool> skillCanUse;//技能可以使用的委托
    public Action<string,bool> skillUse;//技能解锁
    public Action<string, float> skillCoolDowwn;//技能冷却
    #endregion

    #region 技能
    private List<CanSkill> TemporaryCanSkill=new List<CanSkill>();
    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }

    public Blackhole_Skill blackhole { get; private set; }

    public Crystal_Skill crystal { get; private set; }

    public Arrow_Skill arrow { get; private set; }

    public  HealSkill heal { get; private set; }

    public Fire_Skill fire { get; private set; }

    public Laser_Skill laser { get; private set; }
    #endregion

    #region 生命周期
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        TemporaryCanSkill = new List<CanSkill>();

        clone = GetComponent<Clone_Skill>();
        AddCanSkill(clone);

        sword = GetComponent<Sword_Skill>();
        AddCanSkill(sword);

        blackhole = GetComponent<Blackhole_Skill>();
        AddCanSkill(blackhole);

        crystal = GetComponent<Crystal_Skill>();
        AddCanSkill(crystal);

        arrow = GetComponent<Arrow_Skill>();
        AddCanSkill(arrow);

    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        fire = GetComponent<Fire_Skill>();
        heal =GetComponent<HealSkill>();
        laser=GetComponent<Laser_Skill>();
    }
    #endregion

    //添加技能到技能管理器中
    public void AddCanSkill(Skill _skill)
    {
        TemporaryCanSkill.Clear();
        TemporaryCanSkill =_skill.GetSkillChange();

        if (TemporaryCanSkill.Count == 0)
            return;
        foreach (CanSkill _canSkill in TemporaryCanSkill)
        {

            canSkills.Add(_canSkill);
            canSkillDictionary.Add(_canSkill.GetSkillName(), _canSkill);
            
        }
    }

    //检查技能是否可以使用
    public bool CanUseSkill(string _skillName)
    {
        if (canSkillDictionary.TryGetValue(_skillName, out CanSkill _canSkill))
        {
            return _canSkill.CanUseThisSkill();
        }
        return false;
    }

    //切换技能并且打开技能介绍UI
    public bool SetSkillTreeTrue(List<string> _CanNotSelect,List<string> _frontSkill,string _select)
    {
        bool canLearn=true;

        if (canSkillDictionary.TryGetValue(_select, out CanSkill _selectSkill))
        {
            //前置技能判断是否解锁
            if(_frontSkill.Count != 0)
                foreach (var _FrontSkill in _frontSkill)
                {
                    if (canSkillDictionary.TryGetValue(_FrontSkill, out CanSkill _canSkill))
                    {
                        if(!_canSkill.IsUnlocked())
                            canLearn=false;
                    }
                }
            //skillTroduceUI.SetUITrouduce(_selectSkill,canLearn);
            //判断是否能解锁
            SetUITrouduce?.Invoke(_selectSkill, canLearn);
            //判断技能是否已经解锁
            if (!_selectSkill.IsUnlocked()) 
            {
                
                return false;
            }

            //前置技能采用
            foreach (var _FrontSkill in _frontSkill)
            {
                if (canSkillDictionary.TryGetValue(_FrontSkill, out CanSkill _canSkill))
                {
                    UseSkill(_canSkill);
                    skillCanUse?.Invoke(_canSkill.GetSkillName(), true);
                }
            }

            //取消相对技能采用
            if (_CanNotSelect.Count != 0 ) 
                foreach (var _canNotSkill in _CanNotSelect)
            {
                if(canSkillDictionary.TryGetValue(_canNotSkill, out CanSkill _canSkill))
                    {
                        OffUseSkill(_canSkill);
                        skillCanUse?.Invoke(_canSkill.GetSkillName(), false);


                    }
                }
            
            //技能采用
            UseSkill(_selectSkill);
            skillCanUse?.Invoke(_selectSkill.GetSkillName(),true);
            return true;
        }

        return false;
    }

    //第一次通过技能点数解锁技能
    public bool MakeSkillUnlocked(CanSkill _canSkill)
    {
        if (_canSkill.GetNeedAmount() <= Inventory.Instance.skillSoulAmount)
        {
            unlockSkill(_canSkill);
            Inventory.Instance.SpendSkillSoul(_canSkill.GetNeedAmount());
            return true;
        }
        else
            return false;
    }
    //技能冷却传递
    public void SetSkillCoolDOwn(string _id ,float _coolDown)
    {
        skillCoolDowwn?.Invoke(_id,_coolDown);
    }

    //设定初始技能冷却UI
    public void UseStartSkill(string _id)
    {
        skillUse?.Invoke(_id, true);
    }
    //采用当前技能
    private  void UseSkill(CanSkill _canSkill)
    {
        skillUse?.Invoke(_canSkill.GetSkillName(),true);
        _canSkill.SetCanUseSkill();
    }
    //关闭采用技能
    private  void OffUseSkill(CanSkill _canSkill)
    {
        skillUse?.Invoke(_canSkill.GetSkillName(), false);
        _canSkill.SetCannotUseSkill();
    }
    //直接解锁技能
    public  void unlockSkill(CanSkill _selectSkill)
    {
        _selectSkill.SetIsUnlocked();
    }

    #region 数据存储
    public void LoadData(GameData _data)
    {
        if (_data == null)
            return;
        
        if(_data.skillUnlocked!=null)
            foreach (var _skill in _data.skillUnlocked)
        {
            if (canSkillDictionary.TryGetValue(_skill, out CanSkill _selectSkill))
                {
                    unlockSkill(_selectSkill);
                }
            }
        if (_data.skillUnlocked!=null)
            foreach (var _skill in _data.skillUsed)
        {
            if (canSkillDictionary.TryGetValue(_skill, out CanSkill _selectSkill))
            {
                _selectSkill.SetCanUseSkill();
                skillCanUse?.Invoke(_selectSkill.GetSkillName(), true);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        List<string> unlocked=new List<string>();
        List<string> used=new List<string>();
        foreach (var _skill in canSkills)
        {
            if (_skill.IsUnlocked())
            {
               unlocked.Add(_skill.GetSkillName());
            }
            if (_skill.CanUseThisSkill())
            {
                used.Add(_skill.GetSkillName());
            }
        }

        _data.skillUnlocked=unlocked;
        _data.skillUsed=used;
    }
    #endregion
}
