using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = " One Kill Effect", menuName = "Data/Item effect/One Kill Effect")]
public class OneKill : ItemEffect
{
    public override void WearEffect()
    {
        PlayerManager.instance.player.GetComponent<CharacterStats>().SetOneKill();
    }
    override public void RemoveEffect()
    {
        PlayerManager.instance.player.GetComponent<CharacterStats>().OffOneKill();
    }
}
