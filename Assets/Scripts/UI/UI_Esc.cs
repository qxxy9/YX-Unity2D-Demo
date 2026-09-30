using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Esc : MonoBehaviour
{
    public void continueGame()
    {
        TimePuaseManager.instance.continueGame();
    }

    public void BackMainMenu()
    {
        TimePuaseManager.instance.BackMainMenu();
        //StartCoroutine(BackMenu());
    }

  // private IEnumerator BackMenu()
  // {
  //     TimePuaseManager.instance.BackMainMenu();
  //     SaveManager.instance.gameToMenu();
  //     yield return new WaitForSeconds(1.8f);
  //     SceneManager.LoadScene(sceneName);
  // }

    public void ExitGame()
    {
        
        TimePuaseManager.instance.ExitGame();
        SaveManager.instance.ExitGame();
    }

}
