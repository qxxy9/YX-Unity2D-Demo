using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Contoller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPerfab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackHoleTimer;

    private bool canGrow=true;
    private bool canShrink;
    private bool canCreatHotKeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisapper=true;

    private int amountOfAttack=4;
    private float cloneAttackCooldown=.3f;
    private float cloneAttackTimer;


    public List<Transform> targets=new List<Transform>();
    private List<GameObject> createdHotKey= new List<GameObject>();

    public bool playerCanExitState {  get; private set; }

    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttack,float _cloneAttackCooldown,float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;
    }


    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;
        if (blackHoleTimer < 0)
        {
            DestroyHotKey();
            blackHoleTimer = Mathf.Infinity;
            if(targets.Count > 0) 
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }
        if (Input.GetKeyDown(KeyCode.R)&&TimePuaseManager.instance.CanReceiveNormalButton())
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;
        DestroyHotKey();
        cloneAttackReleased = true;
        canCreatHotKeys = false;
        if (playerCanDisapper)
        {

            playerCanDisapper=false;
            PlayerManager.instance.player.fx.MakeTransprent(true);

        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased&&amountOfAttack>0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            //if (SkillManager.instance.clone.cloneInsteadOfCrystal)
           // {
            //    SkillManager.instance.crystal.CreatCrystal();
            //    SkillManager.instance.crystal.CurrentCrystalChooseEnemy();
         //   }
           // else
         //   {
               SkillManager.instance.clone.CreatClone(targets[randomIndex], new Vector3(xOffset, 0));

          //  }
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackHoleAbility",.5f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        playerCanExitState = true;
        //PlayerManager.instance.player.ExitBlackHoleAbility();
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKey()
    {
        if (createdHotKey.Count <= 0)
            return;
        for(int i = 0;i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreatHotKey(collision);

        }
       
    }

      private void OnTriggerExit2D(Collider2D collision)
      {
       if (collision.GetComponent<Enemy>() != null)
       {
          collision.GetComponent<Enemy>().FreezeTime(false);
        }
      }

  //  private void OnTriggerExit2D(Collider2D collision)=>collision.GetComponent<Enemy>()?.FreezeTime(false);
   

    private void CreatHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;
        if (!canCreatHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotKeyPerfab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform)=>targets.Add(_enemyTransform);
}
