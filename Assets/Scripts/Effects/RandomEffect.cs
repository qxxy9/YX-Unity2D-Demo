using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomFate", menuName = "Data/Item effect/RandomFate")]

public class RandomEffect : ItemEffect
{
    public bool isRandom;
    private float randomPercent;

    public override void WearEffect()
    {
        PlayerManager.instance.player.GetComponent<CharacterStats>().SetRandomDamage();
    }

    public override void RemoveEffect()
    {
        PlayerManager.instance.player.GetComponent<CharacterStats>().OffRandomDamage();
    }

}
