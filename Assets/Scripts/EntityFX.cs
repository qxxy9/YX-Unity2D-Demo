using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float FlashDuration;
     private Material originalMat;

    [Header("Ailment color")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] shockColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color hideColor;
    private void Start()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        
    }

    public void Hide(bool _isHide)
    {
        if (_isHide)
        {
            sr.color = hideColor;
        }
        else
            sr.color = Color.white;
    }
    //完全隐身
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(FlashDuration);
        sr.color = currentColor;
        sr.material = originalMat;
    }
    private void RedColorBlink()
    {
        if(sr.color!=Color.white)
            sr.color = Color.white;
        else sr.color = Color.red;
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color=Color.white;
    }

    public void IgnitedFXFor(float _seconds)
    {
        InvokeRepeating("IgnitedColorFX",0,.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgnitedColorFX()
    {
        if(sr.color!=igniteColor[0])
            sr.color = igniteColor[0];
        else sr.color = igniteColor[1];
    }
    private void ChillColorFX()
    {
        sr.color = chillColor;
    }

    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else sr.color = shockColor[1];
    }
}
