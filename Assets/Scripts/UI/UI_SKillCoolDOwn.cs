using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SKillCoolDOwn : MonoBehaviour
{
    [SerializeField] private List<Slider> slider = new List<Slider>();

    private Dictionary<Slider, string> skillCoolUI = new Dictionary<Slider, string>();//已有技能UI
    private Dictionary<Slider, float> skillCoolRemain = new Dictionary<Slider, float>();
    private Dictionary<Slider, float> skillCoolMax = new Dictionary<Slider, float>();

    private void Awake()
    {
        Slider[] sliderArray = GetComponentsInChildren<Slider>();
        slider = new List<Slider>(sliderArray);
        foreach (var ui in slider)
        {
            ui.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        SkillManager.instance.skillUse += ExposeSkillCoolDown;
        SkillManager.instance.skillCoolDowwn += SetMaxValue;
    }

    private void Update()
    {
        // 先复制一份key列表遍历，避免原字典被修改导致报错
        List<Slider> allActiveSliders = new List<Slider>(skillCoolRemain.Keys);
        List<Slider> toRemove = new List<Slider>();

        foreach (Slider s in allActiveSliders)
        {
            if (!skillCoolRemain.ContainsKey(s)) continue; // 兜底，防止已经被删掉

            float remain = skillCoolRemain[s];
            if (remain > 0)
            {
                remain -= Time.deltaTime;
                skillCoolRemain[s] = remain;
                s.value = remain;
            }
            else
            {
                toRemove.Add(s);
            }
        }

        // 统一删除（不在foreach里删字典）
        foreach (var s in toRemove)
        {
            skillCoolRemain.Remove(s);
            skillCoolMax.Remove(s);
        }
    }

    private void OnDisable()
    {
        if (SkillManager.instance != null)
        {
            SkillManager.instance.skillCoolDowwn -= SetMaxValue;
        }
    }

    private void OnDestroy()
    {
        if (SkillManager.instance != null)
        {
            SkillManager.instance.skillUse -= ExposeSkillCoolDown;
        }
    }

    private void ExposeSkillCoolDown(string _skillName, bool isTrue)
    {
        foreach (var ld in slider)
        {
            if (!skillCoolUI.ContainsKey(ld))
            {
                skillCoolUI.Add(ld, _skillName);
                ld.gameObject.SetActive(true);
                ld.value = 0;
                return;
            }
        }
    }

    public void SetMaxValue(string id, float _coolDown)
    {
        foreach (var ld in slider)
        {
            if (skillCoolUI.ContainsKey(ld) && skillCoolUI[ld] == id)
            {
                ld.maxValue = _coolDown;
                skillCoolMax[ld] = _coolDown;
                skillCoolRemain[ld] = _coolDown;
                ld.value = _coolDown;
            }
        }
    }

}
