
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHp,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningdamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;


    UI_Manager uiManager;

    [Header("Major Stats")]
    public Stat strength;   //攻击力，一点提示百分之一爆伤
    public Stat agility;    //提升闪避以及暴击率，一比一
    public Stat intelligence;  //提升魔法伤害，一比一；提升魔法抗性，一比三，只在结算界面影响不影响魔抗面板
    public Stat vitality;     //提升最大生命值，一比五

    [Header("Offencive Stats")]
    public Stat damage;       //初始伤害
    public Stat critChance;  //暴击率
    public Stat critPower;   //暴击伤害


    [Header("Defencive Stats")]
    public Stat maxHp;    //初始最大生命值
    public Stat armor;     //护甲，一比一减少伤害
    public Stat evasion;   //闪避
    public Stat magicResistance;  //初始魔抗，属性不受智力影响

    [Header("Magic Stats")]
    public Stat fireDamage;    //火伤
    public Stat iceDamage;     //冰伤
    public Stat lightningdamage;   //雷伤

    public bool isIgnited;  //烧伤
    public bool isChilled;  //降低护甲
    public bool isShocked;  //降低命中率

    private float ignitedTimer;
    private float chilledTimer;  
    private float shockedTimer;
    private float ignitDamageCooldown = .3f;//灼伤伤害间隔
    private float ignitDamageTimer;
    private int ignitDamage ;  //被灼烧伤害，由敌方属性决定
    private int shockDamage;//被电击伤害
    [SerializeField] private GameObject shockStrikePerfab;
    [SerializeField] private float alimentsDuration = 2.5f;  //元素持续时间

    public int mod;//限时buff量
    public Stat stat;

    public List<BuffData> timeBuff;
    public List<BuffData> removedBuff;

    public bool isDead { get;private set; }
    public int currentHealth;
    public System.Action onHealthChanged;
    public int doTotalDamage;
    public int doTotalMagicDamage;

    private bool isRandomDamage;
    private bool isOneKill;
    private bool isLoad=false;


    public virtual void Start()
    {
        critPower.SetDefaultValue(150);
        if (!isLoad)
        {
            InitionHealth();
        }


        fx =GetComponent<EntityFX>();

        uiManager = UI_Manager.Instance;
    }

    

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        ignitDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;
        ApplyIgniteDamage();

        removedBuff.Clear();
        foreach (BuffData data in timeBuff)
        {
            data.timeLoss();
            if (!data.CheckTime())
            {
                removedBuff.Add(data);
                
            }
        }
        if (removedBuff.Count > 0)
        {
            foreach (BuffData data in removedBuff)
            {
                RemoveTimeBuff(data.mod, data.buffId);
                timeBuff.Remove(data);
            }
        }

    }

   public int GetMaxHP()
    {
        return maxHp.GetValue()+vitality.GetValue()*5;
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {

        if (TargetCanAvoidAttack(_targetStats))
            return;
        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        totalDamage = EquipmentEffectDamage(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        doTotalDamage = totalDamage;
    }

    public virtual void TakeDamage(int _damage)
    {
        
        currentHealth -= _damage;
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        onHealthChanged?.Invoke();
        if (currentHealth <= 0&&!isDead)
        {
           Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amont)
    {
        
        currentHealth += _amont;
        if(currentHealth>GetMaxHP())
            currentHealth=GetMaxHP();
        if(onHealthChanged != null)
            onHealthChanged();
    }

    public virtual void DotDamage(int _damage)
    {
        currentHealth -= _damage;
        onHealthChanged?.Invoke();
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void InitionHealth()
    {
        currentHealth = GetMaxHP();
    }

    #region 闪避效果
    public virtual void GetEvasion()
    {
        evasion.AddModifier(99);
    }
    public void OffEvasion()
    {
        evasion.RemoveModifier(99);
    }
    #endregion
    public virtual void Die()
    {
        isDead = true;
    }

    #region 装备及药剂效果
   private int EquipmentEffectDamage(CharacterStats _targetStats, int totalDamage)
    {
        if (isRandomDamage)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * Random.Range(.1f, 3f));

        }
        if (isOneKill && Random.Range(0, 1000) < 9)
        {
            totalDamage = _targetStats.currentHealth;
        }

        totalDamage = Mathf.Clamp(totalDamage, 1, int.MaxValue);
        return totalDamage;
    }

    //限时加成
    public void AddCheckTimeBuff(int _mod, Stat _stat, float _duration)
    {
        _stat.AddModifier(_mod);
        
    }

    public void RemoveTimeBuff(int _mod, string _id)
    {

        switch (_id)
        {
            case "strength":
                strength.RemoveModifier(_mod);
                break;
            case "agility":
                agility.RemoveModifier(_mod);
                break;
            case "intelligence":
                intelligence.RemoveModifier(_mod);
                break;
            case "vitality":
                vitality.RemoveModifier(_mod);
                break;

            case "damage":
                damage.RemoveModifier(_mod);
                break;
            case "critChance":
                critChance.RemoveModifier(_mod);
                break;
            case "critPower":
                critPower.RemoveModifier(_mod);
                break;

            case "maxHp":
                maxHp.RemoveModifier(_mod);
                break;
            case "armor":
                armor.RemoveModifier(_mod);
                break;
            case "evasion":
                evasion.RemoveModifier(_mod);
                break;
            case "magicResistance":
                magicResistance.RemoveModifier(_mod);
                break;

            case "fireDamage":
                fireDamage.RemoveModifier(_mod);
                break;
            case "iceDamage":
                iceDamage.RemoveModifier(_mod);
                break;
            case "lightningdamage":
                lightningdamage.RemoveModifier(_mod);
                break;
            default:
                return;
        }
        
    }

    public void AddtimeBuff(int _mod, string _id, float _duration)
    {
        BuffData _data;

        switch (_id)
        {
            case "strength":
                AddCheckTimeBuff(_mod, strength,_duration);
                break;
            case "agility":
                AddCheckTimeBuff(_mod, agility, _duration);
                break;
            case "intelligence":
                AddCheckTimeBuff(_mod, intelligence, _duration);
                break;
            case "vitality":
                AddCheckTimeBuff(_mod, vitality, _duration);
                break;

            case "damage":
                AddCheckTimeBuff(_mod, damage, _duration);
                break;
            case "critChance":
                AddCheckTimeBuff(_mod, critChance, _duration);
                break;
            case "critPower":
                AddCheckTimeBuff(_mod, critPower, _duration); 
                break;

            case "maxHp":
                AddCheckTimeBuff(_mod, maxHp, _duration); 
                break;
            case "armor":
                AddCheckTimeBuff(_mod, armor, _duration); 
                break;
            case "evasion":
                AddCheckTimeBuff(_mod, evasion, _duration); 
                break;
            case "magicResistance":
                AddCheckTimeBuff(_mod, magicResistance, _duration); 
                break;

            case "fireDamage":
                AddCheckTimeBuff(_mod, fireDamage, _duration); 
                break;
            case "iceDamage":
                AddCheckTimeBuff(_mod, iceDamage, _duration); 
                break;
            case "lightningdamage":
                AddCheckTimeBuff(_mod, lightningdamage, _duration); 
                break;
            default:
                return;
        }
        _data = new BuffData(_id, _duration, _mod);
        timeBuff.Add(_data);
    }


    public virtual Vector2 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetQuaternion()
    {
        return transform.rotation;
    }

    //随机伤害开关
    public void SetRandomDamage()
    {
        isRandomDamage = true;
    }
    public void OffRandomDamage()
    {
        isRandomDamage= false;
    }


    //一击必杀开关

    public void SetOneKill()
    {
        isOneKill = true;
    }

    public void OffOneKill()
    {
        isOneKill= false;
    }


    #endregion

    //魔法伤害和元素状态
    #region Magic Damage And Aliments
    public virtual void DoMagicDamage(CharacterStats _targetsatas)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningdamage.GetValue();



        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetsatas, totalMagicDamage);
        EquipmentEffectDamage(_targetsatas, totalMagicDamage);
        doTotalMagicDamage = totalMagicDamage;
        _targetsatas.TakeDamage(totalMagicDamage);
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyAliments(_targetsatas, _fireDamage, _iceDamage, _lightningDamage);
    }

    private static void AttemptToApplyAliments(CharacterStats _targetsatas, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool _canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool _canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool _canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;
        while (!_canApplyChill && !_canApplyIgnite && !_canApplyShock)
        {
            if (Random.value < .33f && _fireDamage > 0)
            {
                _canApplyIgnite = true;

                if (_canApplyIgnite)
                    _targetsatas.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

                _targetsatas.ApplyAliments(_canApplyIgnite, _canApplyChill, _canApplyShock);

                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                _canApplyChill = true;

                if (_canApplyShock)
                    _targetsatas.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * .8f));

                _targetsatas.ApplyAliments(_canApplyIgnite, _canApplyChill, _canApplyShock);
                return;
            }
            if (Random.value < 1f && _lightningDamage > 0)
            {
                _canApplyShock = true;
                _targetsatas.ApplyAliments(_canApplyIgnite, _canApplyChill, _canApplyShock);

                return;
            }
        }

        if (_canApplyIgnite)
            _targetsatas.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        if (_canApplyShock)
            _targetsatas.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * .8f));

        _targetsatas.ApplyAliments(_canApplyIgnite, _canApplyChill, _canApplyShock);
    }

    public void SetupIgniteDamage(int _damage)
    {
        ignitDamage = _damage;
    }

    public void SetupShockDamage(int _damage)
    {
        shockDamage = _damage;
    }

    private int CheckTargetResistance(CharacterStats _targetsatas, int totalMagicDamage)
    {
        totalMagicDamage -= _targetsatas.magicResistance.GetValue() + (_targetsatas.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 1, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAliments(bool _ignite, bool _chill, bool _shock)
    {
        bool _canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool _canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool _canApplyShock = !isIgnited && !isChilled;



        if (_ignite && _canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = alimentsDuration;

            fx.IgnitedFXFor(alimentsDuration);
        }

        if (_chill && _canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = alimentsDuration;
            float slowPercent = 0.3f;
            GetComponent<Entity>().SlowEntity(slowPercent, chilledTimer);
            fx.ChillFXFor(alimentsDuration);
        }
        if (_shock && _canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                HitNearTargetWithShockStrike();
            }


        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;
        isShocked = _shock;
        shockedTimer = alimentsDuration;
        fx.ShockFXFor(alimentsDuration);
    }

    private void ApplyIgniteDamage()
    {
        if (ignitDamageTimer < 0 && isIgnited)
        {
            DotDamage(ignitDamage);

            ignitDamageTimer = ignitDamageCooldown;
        }
    }

    private void HitNearTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 15);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && transform.position != hit.transform.position)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;

                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;
        }


        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePerfab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    #endregion


    //物理伤害计算
    #region Stat calculation


    private  bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (isShocked)
            totalEvasion += 10;
        if (Random.Range(0, 100) < totalEvasion)
        {

            return true;
        }
        return false;
    }


    private  int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() *.7f);
        }
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 1, int.MaxValue);
        return totalDamage;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }
    public int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue())*0.01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }
    #endregion
  

    //UI相关
    public Stat GetNowStats(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critPower:
                return critPower;
            case StatType.maxHp:
                return maxHp;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicResistance:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningdamage:
                return lightningdamage;
        }
        return null;
    }


    #region 数据存储

    public CharacterStatSaveData GetSaveData()
    {
        CharacterStatSaveData data = new CharacterStatSaveData();
        data.currentHP=this.currentHealth;
        data.isIgnited = this.isIgnited;
        data.isShocked = this.isShocked;
        data.isChilled = this.isChilled;
        data.ignitedTimer = this.ignitedTimer;
        data.shockedTimer = this.shockedTimer;
        data.chilledTimer = this.chilledTimer;
        data.ignitDamageTimer = this.ignitDamageTimer;
        data.ignitDamage = this.ignitDamage;
        data.shockDamage = this.shockDamage;
        data.buffData=this.timeBuff;
        
        return data;
    }


    public void ReceiveSaveData(CharacterStatSaveData _data)
    {
        
        this.currentHealth = _data.currentHP;
        this.isIgnited=_data.isIgnited;
        this.isShocked=_data.isShocked;
        this.isChilled=_data.isChilled;
        this.ignitedTimer = _data.ignitedTimer;
        this.shockedTimer = _data.shockedTimer;
        this.chilledTimer = _data.chilledTimer;
        this.ignitDamageTimer = _data.ignitDamageTimer;
        this.ignitDamage = _data.ignitDamage;
        this.shockDamage = _data.shockDamage;
        if (_data.buffData.Count > 0)
        {
            foreach (var _timeData in _data.buffData)
            {
                AddtimeBuff(_timeData.mod, _timeData.buffId, _timeData.duration);
            }
        }

        isLoad = true;
        
    }
    #endregion
}
