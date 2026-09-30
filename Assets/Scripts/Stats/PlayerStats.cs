using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player ;
    public override void Start()
    {
        base.Start();
        player = GetComponentInParent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
        
    }

    public override void Die()
    {
        
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>().GenerateDrop();
    }
}
