using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillSoul : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshPro;
    private bool HaveSet;
    private void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
        if (m_TextMeshPro != null && !HaveSet)
        {
            Inventory.Instance.SkillSoulAmountChange += SetUI;
            HaveSet = true;
        }
    }
    private void OnEnable()
    {
        if (m_TextMeshPro != null&&!HaveSet)
        {
            Inventory.Instance.SkillSoulAmountChange += SetUI;
            HaveSet = true;
        }
        
        
    }

    private void OnDisable()
    {
        Inventory.Instance.SkillSoulAmountChange -= SetUI;
        HaveSet=false;
    }
    public void SetUI(float _amount)
    {
        
        m_TextMeshPro.text=_amount.ToString();
    }

}
