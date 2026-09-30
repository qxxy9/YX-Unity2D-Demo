using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool canPenerate;
    private CharacterStats stats;
    private float speed;
    [SerializeField] private bool isMagic;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpArrow(bool _canPenerate,CharacterStats _stats,float _speed,float _arrowDuration)
    {
        canPenerate = _canPenerate;
        stats = _stats;
        speed = _speed;
        if(transform.rotation.y<0)
            rb.velocity=new Vector2(-1*_speed,0);
        else
            rb.velocity=new Vector2(_speed,0);
        Invoke("SelfDestroy", _arrowDuration);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stats is PlayerStats)
        {
            if (collision.GetComponent<Player>() != null)
                return;

            if (collision.GetComponent<Enemy>() != null)
            {
                CheckDamage(collision);
            }


            if (!canPenerate)
            {

                rb.velocity = Vector2.zero;

                Invoke("SelfDestroy", .2f);
            }
        }
        
        else
        {
            if (collision.GetComponent<Enemy>() != null)
                return;

            if (collision.GetComponent<Player>() != null)
            {
                CheckDamage(collision);
                Invoke("SelfDestroy", .2f);
            }
        }
        
    }

    private void CheckDamage(Collider2D collision)
    {
        if(!isMagic)
            stats.DoDamage(collision.GetComponent<CharacterStats>());
        else
            stats.DoMagicDamage(collision.GetComponent<CharacterStats>());
    }

    public void ArrowDamage()
    {

    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
