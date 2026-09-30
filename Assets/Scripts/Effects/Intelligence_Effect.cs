using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Intelligence Effect", menuName = "Data/Item effect/Intelligence effect")]
public class Intelligence_Effect : ItemEffect
{
    [Range(.2f, .5f)]
    [SerializeField] private float intelligencePersent;
    [SerializeField]private float timeBuffDuration;
    public override void ExcuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int addAmount = Mathf.RoundToInt(playerStats.intelligence.GetValue() * intelligencePersent);

        playerStats.AddtimeBuff(addAmount, "intelligence", timeBuffDuration);
        



    }



}
