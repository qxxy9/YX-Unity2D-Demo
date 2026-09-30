using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    private UI_DarkScreen darkScreen;
    private void Start()
    {
        darkScreen = GetComponentInChildren<UI_DarkScreen>();
        darkScreen.FadeOut();
        SaveManager.instance.enterMenu();
        if (!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
        }
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(yieldFadeIN());
    }

    public void ContinueGame()
    {
        StartCoroutine(yieldFadeIN());
        
    }

    private IEnumerator yieldFadeIN()
    {
        darkScreen.FadeIn();
        yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        darkScreen.FadeIn();
        SaveManager.instance.ExitGame();
    }

    
}
