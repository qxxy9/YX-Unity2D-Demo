using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunderstrike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePerfab;
    public override void ExcuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike=Instantiate(thunderStrikePerfab,_enemyPosition.position,Quaternion.identity);
        Destroy(newThunderStrike,.2f);
    }
}
