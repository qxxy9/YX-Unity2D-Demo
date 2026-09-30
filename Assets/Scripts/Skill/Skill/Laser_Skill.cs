using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Skill : Skill
{
    [SerializeField] private GameObject laserPerfab;
    [SerializeField] private bool isSingle;
    
    [SerializeField] private float laserDuration;
    [SerializeField] private float laserPrepare;
    [SerializeField] private float laserSize;
    private CharacterStats stat;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (isSingle)
        {
            GameObject newLaser = Instantiate(laserPerfab, player.transform.position, Quaternion.identity);
            Laser_Controller laser_Controller = newLaser.GetComponent<Laser_Controller>();
            laser_Controller.SetUpLaser(laserDuration, laserPrepare,laserSize,stat);
        }
        else 
        {
            for (int i = -2; i < 3; i++)
            {
                GameObject newLaser = Instantiate(laserPerfab, new Vector2(player.transform.position.x+i*4,player.transform.position.y), Quaternion.identity);
                Laser_Controller laser_Controller = newLaser.GetComponent<Laser_Controller>();
                laser_Controller.SetUpLaser(laserDuration*2, laserPrepare*1.5f, laserSize*2, stat);
            }
            
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SinglePattern()
    {
        isSingle = true;
    }

    public void NotSinglePattern()
    {
        isSingle = false;
    }

    public void GetStat(CharacterStats _stat)
    {
        stat = _stat;
    }


}
