using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GameRunning,        // 正常游戏，所有输入生效
    InventoryOpen,      // 背包打开：游戏暂停；只允许ESC、鼠标点击
    PauseMenuOpen       // ESC暂停菜单打开：游戏暂停；只允许鼠标点击，屏蔽键盘
}

public class TimePuaseManager : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";
    [SerializeField] private UI_DarkScreen darkScreen;
    public static TimePuaseManager instance;
    private GameState gameState;
    public bool isPaused { get; private set; }
    private bool isOpenBackPack;
    [SerializeField]private GameObject EscUI;
    private bool canEsc = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
        gameState= GameState.GameRunning;
    }

    private void Start()
    {
        darkScreen.FadeOut();
        gameState = GameState.GameRunning;
        EscUI.SetActive(false);
    }

    // 切换暂停/继续
    public void backpackChangePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            gameState = GameState.InventoryOpen;
            Time.timeScale = 0;
            isOpenBackPack = true;
        }
        else
        {
            Time.timeScale = 1;
            gameState = GameState.GameRunning;
            isOpenBackPack = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&canEsc)
        {
            EscUI.SetActive(true);
            gameState=GameState.PauseMenuOpen;
            Time.timeScale = 0;
        }
    }

    //是否在正常运行
    public bool CanReceiveNormalButton()
    {
        if (gameState == GameState.GameRunning)
        {
            return true;
        }
        return false;
    }
    //是否允许打开背包
    public bool CanOpenBackpack()
    {
        if (gameState == GameState.PauseMenuOpen)
        {
            return false;
        }
        return true;
    }

    //继续游戏
    public void continueGame()
    {
        if (isOpenBackPack)
        {
            gameState = GameState.InventoryOpen;
        }
        else
        {
            gameState = GameState.GameRunning;
            Time.timeScale = 1;
        }
        EscUI.SetActive(false);    

    }

    
    //退出游戏
    public void ExitGame()
    {
        Time.timeScale = 1;
    }

    //协程实现淡入黑幕并切换场景返回主菜单
    public void BackMainMenu()
    {
        Time.timeScale = 1;
        canEsc = false;
        EscUI.SetActive(false);
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        SaveManager.instance.gameToMenu();
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(sceneName);
    }
}
