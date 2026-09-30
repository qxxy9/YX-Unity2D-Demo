using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAnimationTrriger : MonoBehaviour
{
    private bool isHide;
    [SerializeField] private GameObject _ob;
    private EnemyBoss enemy => GetComponentInParent<EnemyBoss>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(_target);
            }
        }
    }

    private void HideController()
    {
        if (!isHide)
        {
            isHide = true;
            enemy.fx.Hide(isHide);
            enemy.EvasionSet();
            
        }
        else
        {
            isHide = false;
            enemy.fx.Hide(isHide);
            enemy.EvasionOff();
        }

    }

    private void SetOb()
    {
        _ob.SetActive(true);
    }

    private void OffOb()
    {
        _ob.SetActive(false);

    }

}
