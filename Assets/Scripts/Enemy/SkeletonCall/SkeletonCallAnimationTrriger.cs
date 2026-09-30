using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCallAnimationTrriger : MonoBehaviour
{
    [SerializeField] private GameObject _ob;
    private Enemy_SkeletonCall enemy => GetComponentInParent<Enemy_SkeletonCall>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
        OffOb();
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
