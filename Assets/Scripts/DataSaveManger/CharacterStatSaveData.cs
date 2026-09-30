using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatSaveData 
{
    public int currentHP;
    public bool isIgnited;  //烧伤
    public bool isChilled;  //降低护甲
    public bool isShocked;  //降低命中率
    public float ignitedTimer;
    public float ignitDamageTimer;
    public float chilledTimer;
    public float shockedTimer;
    public float ignitDamageCooldown = .3f;//灼伤伤害间隔
    public int ignitDamage;  //被灼烧伤害，由敌方属性决定
    public int shockDamage;   //被电击伤害
    public List<BuffData> buffData;
}
