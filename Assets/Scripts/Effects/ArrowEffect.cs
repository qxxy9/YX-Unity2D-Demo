using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrow Effect", menuName = "Data/Item effect/Arrow effect")]
public class ArrowEffect : ItemEffect
{
    public override void RemoveEffect()
    {
        PlayerManager.instance.player.OffRemote();
    }

    public override void WearEffect()
    {
        
        PlayerManager.instance.player.SetRemote();
    }
}
