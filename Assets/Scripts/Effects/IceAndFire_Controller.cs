using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : ThunderStrike_Effect_Controller
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null )
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
            
        }
    }

}
