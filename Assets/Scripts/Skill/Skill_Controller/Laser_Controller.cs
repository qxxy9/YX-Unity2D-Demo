using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Controller : MonoBehaviour
{
    private Animator anim;
    private float duration;
    private float prepare;
    private float size;
    [SerializeField]private float damageGap;
    private float damageTime;
    private CharacterStats stat;
    private Collider2D cd;
    void Start()
    {
        cd = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        prepare-=Time.deltaTime;
        duration -= Time.deltaTime;
        damageTime -= Time.deltaTime;
        if (prepare < 0)
        {
            anim.SetBool("attack", true);
        }
        if (duration<=0)
        {
            anim.SetBool("attack", false);
            cd.enabled = false;
            Destroy(gameObject,.2f);
        }
    }

    public void SetUpLaser(float _duration,float _prepare,float _size,CharacterStats _stat)
    {
        duration = _duration;
        prepare = _prepare;
        size = _size;
        stat = _stat;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (prepare < 0&&damageTime<0)
        {
            if (collision.GetComponent<Player>() != null)
            {
                stat.DoDamage(collision.GetComponent<CharacterStats>());
                damageTime = damageGap;
            }
        }
    }

}
