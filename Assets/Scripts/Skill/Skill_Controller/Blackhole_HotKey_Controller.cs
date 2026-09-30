using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myNewHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private Blackhole_Skill_Contoller blackHole;

    public void SetupHotKey(KeyCode _myHotKey,Transform _myEnemy,Blackhole_Skill_Contoller _myBlackHole)
    {
        sr=GetComponent<SpriteRenderer>();
        myText=GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myNewHotKey = _myHotKey;
        myText.text=myNewHotKey.ToString();
    }
    public void Update()
    {
        if (Input.GetKeyDown(myNewHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
