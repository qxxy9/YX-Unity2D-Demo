using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType=SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;


    [Header("Skill info")]
    [SerializeField] private GameObject swordPerfab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField]private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    [SerializeField] private CanSkill regularSword;
    [SerializeField] private CanSkill bounceSword;
    [SerializeField] private CanSkill PierceSword;
    [SerializeField] private CanSkill spinSword;
    [SerializeField] private CanSkill swordFroze;
    [SerializeField] private CanSkill swordDeBuff;

    protected override void Start()
    {
        base.Start();
        GenerateDots();

        SetupGravity();
        GetSkillChange();
    }

    public override List<CanSkill> GetSkillChange()
    {
        canSkills.Clear();
        canSkills.Add(regularSword);
        canSkills.Add(bounceSword);
        canSkills.Add(PierceSword);
        canSkills.Add(spinSword);
        canSkills.Add(swordFroze);
        canSkills.Add(swordDeBuff);
        return canSkills;
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType==SwordType.Spin)
            swordGravity = spinGravity;


    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse1)&&regularSword.CanUseThisSkill() && TimePuaseManager.instance.CanReceiveNormalButton())
            finalDir=new Vector2(AimDirection().normalized.x*launchForce.x,AimDirection().normalized.y*launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1)&& regularSword.CanUseThisSkill() && TimePuaseManager.instance.CanReceiveNormalButton())
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position=DotsPosition(i*spaceBetweenDots);
            }
        }


    }

    public void CreatSword()
    {
        if(regularSword.CanUseThisSkill())
            swordType = SwordType.Regular;
        else
            return;
        if (bounceSword.CanUseThisSkill())
            swordType = SwordType.Bounce;
        else if (PierceSword.CanUseThisSkill())
            swordType = SwordType.Pierce;
        else if (spinSword.CanUseThisSkill())
            swordType = SwordType.Spin;
        
        SetupGravity();

        GameObject newSword = Instantiate(swordPerfab, player.transform.position, transform.rotation);
        Sword_Skill_controller newSwordScript=newSword.GetComponent<Sword_Skill_controller>();

        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SetUpBounce(true, bounceAmount,bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);
                break;
        }

        float frozeSword=0;
        if(swordFroze.CanUseThisSkill())
            frozeSword = freezeTimeDuration;
        newSwordScript.SetupSword(finalDir,swordGravity,player,frozeSword,returnSpeed,swordDeBuff.CanUseThisSkill());
        player.AssignNewSword(newSword);
        DotsActive(false);
    }


    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition=player.transform.position;
        Vector2 mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction=mousePosition- playerPosition;
        
        return direction;
    }

    public void  DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }


    private void GenerateDots()
    {
        dots=new GameObject[numberOfDots];

        for(int i = 0; i < numberOfDots; i++)
        {
            dots[i]= Instantiate(dotPrefab,player.transform.position,Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position=(Vector2)player.transform.position+new Vector2(
            AimDirection().normalized.x*launchForce.x,
            AimDirection().normalized.y*launchForce.y)*t+.5f*(Physics2D.gravity*swordGravity)*(t*t);

        return position;
    }
    #endregion
}
