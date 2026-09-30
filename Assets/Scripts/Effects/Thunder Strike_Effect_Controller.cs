using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Effect_Controller : MonoBehaviour
{
    protected PlayerStats playerStats;
    protected bool isattack;
     protected virtual  void Start()
    {
        playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null&&!isattack)
        {
            EnemyStats enemyTarget= collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
            isattack = true;
        }
    }

}
