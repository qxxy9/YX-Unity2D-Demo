using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArrowAnimationTrriger : MonoBehaviour
{
    [SerializeField]private GameObject _ob;
    private Enemy_SkeletonArrow enemy=>GetComponentInParent<Enemy_SkeletonArrow>();
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
