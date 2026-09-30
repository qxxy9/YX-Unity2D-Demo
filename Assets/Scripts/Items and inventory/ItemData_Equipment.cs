using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]


public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public ItemEffect[] effects;


    [Header("Major stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offencive Stats")]
    public int damage;       //伤害
    public int critChance;  //暴击率
    public int critPower;   //暴击伤害


    [Header("Defencive Stats")]
    public int maxHp;    //初始最大生命值
    public int armor;     //护甲，一比一减少伤害
    public int evasion;   //闪避
    public int magicResistance;  //初始魔抗，属性不受智力影响

    [Header("Magic Stats")]
    public int fireDamage;    //火属性伤
    public int iceDamage;     //冰属性伤
    public int lightningdamage;   //雷属性伤


    [Header("Craft requirement")]
    public List<InventoryItem> craftMaterial;


    public void ExcuteItemEffect(Transform _enemyPosition)
    {
        if (effects == null)
        {
            
            return;
        }
        foreach (var item in effects)
        {
            item.ExcuteEffect(_enemyPosition);
        }
    }


    public void WearItemEffect()
    {
        if (effects == null)
        {
            return;
        }
        foreach (var item in effects)
        {
            item.WearEffect();
        }
    }


    public void RemoveItemEffect()
    {
        if (effects == null)
        {
            return;
        }
        foreach (var item in effects)
        {
            item.RemoveEffect();
        }
    }


    public void AddModifiers()
    {
        PlayerStats playerstats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerstats.strength.AddModifier(strength);
        playerstats.agility.AddModifier(agility);
        playerstats.intelligence.AddModifier(intelligence);
        playerstats.vitality.AddModifier(vitality);

        playerstats.damage.AddModifier(damage);
        playerstats.critChance.AddModifier(critChance);
        playerstats.critPower.AddModifier(critPower);

        playerstats.maxHp.AddModifier(maxHp);
        playerstats.armor.AddModifier(armor);
        playerstats.evasion.AddModifier(evasion);
        playerstats.magicResistance.AddModifier(magicResistance);

        playerstats.fireDamage.AddModifier(fireDamage);
        playerstats.iceDamage.AddModifier(iceDamage);
        playerstats.lightningdamage.AddModifier(lightningdamage);
        
    }

    public void RemoveModifiers()
    {
        PlayerStats playerstats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerstats.strength.RemoveModifier(strength);
        playerstats.agility.RemoveModifier(agility);
        playerstats.intelligence.RemoveModifier(intelligence);
        playerstats.vitality.RemoveModifier(vitality);

        playerstats.damage.RemoveModifier(damage);
        playerstats.critChance.RemoveModifier(critChance);
        playerstats.critPower.RemoveModifier(critPower);

        playerstats.maxHp.RemoveModifier(maxHp);
        playerstats.armor.RemoveModifier(armor);
        playerstats.evasion.RemoveModifier(evasion);
        playerstats.magicResistance.RemoveModifier(magicResistance);

        playerstats.fireDamage.RemoveModifier(fireDamage);
        playerstats.iceDamage.RemoveModifier(iceDamage);
        playerstats.lightningdamage.RemoveModifier(lightningdamage);

    }




}


