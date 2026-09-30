using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private GameData gameData;
    private List<IsSaveManager> saveManagers;
    private FileDataHandler dataHandler;
    private bool isMenu;
    private bool isEnd=false;
    public Action gameToMenu;
    public Action menuToGame;

    #region 生命周期
    private void Awake()
    {
     // if (instance != null)
     //     Destroy(instance);
     // else
     //     instance = this;
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
    }

    public void Start()
    {
        saveManagers = FindAllSaveManagers();
        gameToMenu += SaveGame;
        LoadGame();
    }
    private void OnDestroy()
    {
        gameToMenu -= SaveGame;
    }
    #endregion

    #region 游戏存档读档相关
    public void NewGame()
    {
        DeleteSaveData();
        gameData = new GameData();

    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (var manager in saveManagers)
        {
            manager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (isEnd)
        {
            DeleteSaveData();
            return;
        }
        gameData=new GameData();
        foreach (IsSaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("退出游戏（编辑器内不会关闭）");
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }


    private void OnApplicationQuit()
    {
        if(!isMenu)
            SaveGame();
    }

    private List<IsSaveManager> FindAllSaveManagers()
    {
        IEnumerable<IsSaveManager> saveMangers=FindObjectsOfType<MonoBehaviour>().OfType<IsSaveManager>();
        return new List<IsSaveManager>(saveMangers);
    }
    #endregion

    [ContextMenu("Delete save file")]

    public void End()
    {
        isEnd= true;
    }

    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    public bool HasSaveData()
    {
        
        if (dataHandler.Load() != null)
        {
            
            return true;
        }
        return false;
    }
    #region 游戏场景相关
    public void enterMenu()
    {
        isMenu = true;
    }
    #endregion
}
