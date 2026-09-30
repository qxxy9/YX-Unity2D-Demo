using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePerfab;
    [SerializeField] private float newVelocity;
    public override void ExcuteEffect(Transform _enemyPosition)
    {

        Player player = PlayerManager.instance.player;

        bool thirdAttack=player.primaryAttack.comboCounter==2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePerfab, _enemyPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocity*player.facingDir,0);

            Destroy(newIceAndFire,10f);
        }
    }
}
