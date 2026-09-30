using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;
    private Animator anim;
    private bool triggerd;

    
    public void Setup(int _damage,CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    void Start()
    {
        anim=GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        if (!targetStats)
        {
            Destroy(gameObject);
            return;
        }
        

        if (triggerd)
            return;
        transform.position = Vector3.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right=transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale= new Vector3(3,3,1);

            triggerd = true;
            anim.SetTrigger("Hit");
            
            Invoke("DamageAndSelfDestroy", .2f);
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject,.4f);

    }
}
