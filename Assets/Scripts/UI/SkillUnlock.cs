using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUnlock : MonoBehaviour
{
    [SerializeField]private Button unlockButton;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    CanSkill canSkillData;

    private void Start()
    {
        unlockButton.gameObject.SetActive(false);
        textMeshProUGUI.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        unlockButton.onClick?.RemoveListener(SetSkillUnlocked);
        unlockButton.onClick.AddListener(SetSkillUnlocked);
        SkillManager.instance.SetUITrouduce += SetUITrouduce;
    }

    private void OnDisable()
    {
        
        textMeshProUGUI.text = "";
        unlockButton.onClick?.RemoveListener(SetSkillUnlocked);
    }


    public void SetUITrouduce(CanSkill _canSkill,bool _canLearn)
    {
        unlockButton.gameObject.SetActive(false);
        textMeshProUGUI.gameObject.SetActive(true);
        textMeshProUGUI.text = _canSkill.GetIntroduce();
        if (!_canSkill.IsUnlocked()&&_canLearn)
        {
            canSkillData=_canSkill;
            unlockButton.gameObject.SetActive(true);
        }

    }

    public void SetSkillUnlocked()
    {
        if(SkillManager.instance.MakeSkillUnlocked(canSkillData))
            unlockButton.gameObject.SetActive(false);;
        
    }

    
}
