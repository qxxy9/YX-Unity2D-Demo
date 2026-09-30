using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    

    [SerializeField] private Button TurnDownButton;
    
    private bool isAction;

    [SerializeField] private List<string> canNotSkill;
    [SerializeField] private List<string> frontSkill;
    [SerializeField]private string skillName;
    private void Awake()
    {
        TurnDownButton = GetComponent<Button>();
        
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        TurnDownButton.onClick.RemoveListener(SetSkill);
        TurnDownButton.onClick.AddListener(SetSkill);
        if (!isAction)
        {
            SkillManager.instance.skillCanUse += ColorChange;
            isAction = true;
        }
        
    }

    private void OnDisable()
    {
        TurnDownButton.onClick.RemoveListener(SetSkill);
    }

    private void SetSkill()
    {
        SkillManager.instance.SetSkillTreeTrue(canNotSkill, frontSkill, skillName);

    }
           

    private void ColorChange(string _skill,bool _canUse)
    {
        
        if (skillName == _skill)
        {
            if(_canUse)
            {
                
                GetComponent<Image>().color = Color.white;
            }
            else
            {
                GetComponent<Image>().color = Color.gray;
                
            }
                
        }
        
    }

    private void OnDestroy()
    {
        SkillManager.instance.skillCanUse -= ColorChange;
    }
}
