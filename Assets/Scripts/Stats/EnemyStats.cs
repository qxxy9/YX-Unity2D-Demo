using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level;

    [Range(0f, .4f)]
    [SerializeField] private float percentageModifier;
    public override void Start()
    {
        ApplyLevelModifier();

        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifier()
    {
        Modify(damage);
        Modify(maxHp);
        Modify(armor);
        Modify(magicResistance);
        Modify(strength);
        
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifior = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifior));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    public override void Die()
    {
        base.Die();
        
        enemy.Die();
        Inventory.Instance.AddSkillSoul(level);
        myDropSystem.GenerateDrop();

    }
}
