using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [SerializeField]private List<CanvasGroup> currentSelectUI=new List<CanvasGroup>();
    [SerializeField] private List<CanvasGroup> currentCraftSelectUI = new List<CanvasGroup>();
    [SerializeField] private CanvasGroup normalSelectUI;
    [SerializeField] private CanvasGroup iconSelectUI;
    [SerializeField]private Slider slider;
    public bool isInUI;

    private characterAttribute[] textMeshList;

    [SerializeField] private Transform textMeshListParent;
    [SerializeField]private List<UI_SKillCoolDOwn> skillCoolDOwn_UIs;
    [SerializeField] private UI_BossHealth bossHeallth;
    [SerializeField] private UI_End deadUI;
    [SerializeField] private UI_End winUI;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance); 
        }
    }

    private void Start()
    {
        iconSelectUI.alpha = 0;
        iconSelectUI.interactable = false;
        iconSelectUI.blocksRaycasts = false;

        Invoke("BloodUIChange", .1f);
        textMeshList = textMeshListParent.GetComponentsInChildren<characterAttribute>();
        PlayerManager.instance.player.stats.onHealthChanged += BloodUIChange;
    }

    private void Update()
    {
        if (isInUI)
            return;

        if (Input.GetKeyDown(KeyCode.Tab)&&!isInUI&&TimePuaseManager.instance.CanOpenBackpack())
        {
            
            TurnSeleceUI(iconSelectUI);
            TurnSeleceUI(normalSelectUI);
            UpdateTextMeshList();
            isInUI = true;
            TimePuaseManager.instance.backpackChangePause();
        }
    }

    private void OnDestroy()
    {
        PlayerManager.instance.player.stats.onHealthChanged -= BloodUIChange;
    }
    private void BloodUIChange()
    {
        slider.maxValue = PlayerManager.instance.player.stats.GetMaxHP();
        slider.value = PlayerManager.instance.player.stats.currentHealth;
    }
    public void TurnSeleceUI(CanvasGroup _gameObject)
    {
        TurnOffAllSeleceUI(currentSelectUI);
        SetUITrue(_gameObject);

        
    }

    public void TurnCraftSeleceUI(CanvasGroup _gameObject)
    {
        TurnOffAllSeleceUI(currentCraftSelectUI);
        SetUITrue(_gameObject);
    }

    private void TurnOffAllSeleceUI(List<CanvasGroup>  canvasGroups)
    {
        foreach(var ui in canvasGroups)
        {
            SetUIFalse(ui);
        }
    }

    public void SetUIFalse(CanvasGroup _cg )
    {
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    public  void SetUITrue(CanvasGroup _gameObject)
    {
        _gameObject.alpha = 1;
        _gameObject.interactable = true;
        _gameObject.blocksRaycasts = true;
    }

    public void UpdateTextMeshList()
    {
        if (textMeshList == null || textMeshList.Length == 0)
            return;
        
        for (int i = 0; i < textMeshList.Length; i++)
        {
            textMeshList[i].UpdatestatUI();
        }
    }

    public void SetBossHealthUI(CharacterStats _stats,bool _isAttack)
    {
        if (_isAttack)
        {
            bossHeallth.SetUI(_stats);
        }
        else
        {
            bossHeallth.OffUI();
        }
    }

    public void Dead()
    {
        deadUI.SetUI();
    }

    public void Win()
    {
        winUI.SetUI();
    }

}
