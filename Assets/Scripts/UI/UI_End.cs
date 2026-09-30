using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_End : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        this.gameObject.SetActive(false);
    }

    public void SetUI()
    {
        this.gameObject.SetActive(true);
        anim.SetBool("InMenu", false);
    }

    public void BackMainMenu()
    {
        
        TimePuaseManager.instance.BackMainMenu();
        //StartCoroutine(BackMenu());
    }

}
