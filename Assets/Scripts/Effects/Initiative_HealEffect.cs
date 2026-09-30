using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Initiative Heal Effect", menuName = "Data/Item effect/Initiative Heal Effect")]
public class Initiative_HealEffect : ItemEffect
{
    public override void RemoveEffect()
    {
        PlayerManager.instance.player.OffHeal();
    }

    public override void WearEffect()
    {
        PlayerManager.instance.player.SetHeal();
    }
}
