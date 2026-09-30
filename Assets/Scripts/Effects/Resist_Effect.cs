using UnityEngine;


[CreateAssetMenu(fileName = "Resist Effect", menuName = "Data/Item effect/Resist effect")]
public class Resist_Effect : ItemEffect
{
    [Range(.2f, .5f)]
    [SerializeField] private float resistPercent;
    [SerializeField] private float timeBuffDuration;
    public override void ExcuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int addAmount = Mathf.RoundToInt(playerStats.armor.GetValue()*resistPercent);
        playerStats.AddtimeBuff(addAmount, "armor", timeBuffDuration);
    }
}
