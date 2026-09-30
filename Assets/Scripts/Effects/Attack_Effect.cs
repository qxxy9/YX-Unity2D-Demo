using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack Effect", menuName = "Data/Item effect/Attack effect")]
public class Attack_Effect : ItemEffect
{
    [Range(.2f, .5f)]
    [SerializeField] private float attackPersent;
    [SerializeField] private float timeBuffDuration;

    public override void ExcuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int addAmount = Mathf.RoundToInt(playerStats.damage.GetValue() * attackPersent);

        playerStats.AddtimeBuff(addAmount, "damage", timeBuffDuration);
    }

}
