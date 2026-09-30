using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DarkScreen : MonoBehaviour
{
    private Animator anim;

    
    private void Awake()
    {
        anim=GetComponent<Animator>(); 
    }

    private void Start()
    {
        SaveManager.instance.gameToMenu += FadeIn;
    }

    private void OnDestroy()
    {
        SaveManager.instance.gameToMenu -= FadeIn;
    }
    public void FadeOut()
    {
        anim.SetBool("InMenu", true);
    }
    public void FadeIn()
    {
        anim.SetBool("InMenu", false);
    }

}
