using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood_Sucking", menuName = "Data/Item effect/Blood_Sucking")]
public class Blood_Sucking : ItemEffect
{
    [Range(0f, .2f)]
    [SerializeField] private float healPercent;
    [SerializeField] private bool effectDamage;
    int healAmount;
    public override void ExcuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(effectDamage)
            healAmount = Mathf.RoundToInt(playerStats.doTotalDamage * healPercent);
        else 
            healAmount = Mathf.RoundToInt(playerStats.doTotalMagicDamage * healPercent);
        playerStats.IncreaseHealthBy(healAmount);
    }
}
