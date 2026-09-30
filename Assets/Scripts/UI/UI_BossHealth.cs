using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    private CharacterStats stats;
    private Slider slider;
    public void Start()
    {
        slider = GetComponentInChildren<Slider>();
        this.gameObject.SetActive(false);
    }

    public void SetUI(CharacterStats _stats)
    {
        this.gameObject.SetActive(true);
        this.stats = _stats;
        slider.maxValue=stats.GetMaxHP();
        slider.value=stats.currentHealth;
        stats.onHealthChanged += ChangeUI;
    }

    public void OffUI()
    {
        this.gameObject.SetActive(false);
        stats.onHealthChanged -= ChangeUI;
        this.stats = null;
    }
    public void ChangeUI()
    {
        slider.value=stats.currentHealth;
    }
}
