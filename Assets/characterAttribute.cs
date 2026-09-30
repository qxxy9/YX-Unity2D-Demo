using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class characterAttribute : MonoBehaviour
{
    [SerializeField] private StatType _statType;
    [SerializeField]private TextMeshProUGUI _textMeshProUGUI;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    public void UpdatestatUI()
    {

        if(playerStats == null)
        {
            playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        }

        _textMeshProUGUI.text=""+ playerStats.GetNowStats(_statType).GetValue();

        

    }
}
