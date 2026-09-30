using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnDown : MonoBehaviour
{
    [SerializeField]private List<CanvasGroup> ManagerPageUI = new List<CanvasGroup>();

    [SerializeField]private Button TurnDownButton;

    private void Awake()
    {
        TurnDownButton = GetComponent<Button>();
    }

   

    private void OnEnable()
    {
        TurnDownButton.onClick.RemoveListener(TurnDownManageUI);
        TurnDownButton.onClick.AddListener(TurnDownManageUI);
    }

    private void OnDisable()
    {
        TurnDownButton.onClick.RemoveListener(TurnDownManageUI);
    }


    private void TurnDownManageUI()
    {
        foreach(var _ManagerPageUI in ManagerPageUI)
        {
            _ManagerPageUI.alpha = 0;
            _ManagerPageUI.interactable = false;
            _ManagerPageUI.blocksRaycasts = false;
        }
        
        UI_Manager.Instance.isInUI = false;
        TimePuaseManager.instance.backpackChangePause();
    }
}
