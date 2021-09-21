using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PartOfSkillView : MonoBehaviour
{
    [SerializeField] SkillBase mySkill;
    [SerializeField] int skillId;
    [SerializeField] string skillName;
    [SerializeField] TextMeshProUGUI skillNameText;

    [SerializeField] TextMeshProUGUI tagText;
    [SerializeField] string[] tagsArray = new string[3];
    bool selectState = false;

    [SerializeField] Image boxImage;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Image elementImage;
    [SerializeField] Color defaultColor;
    [SerializeField] Color ChoosedColor;

    [SerializeField] Vector3 defaultPos;


    public void Setup(SkillBase skill,Sprite elementSprite)
    {
        mySkill = skill;
        this.skillName = mySkill.GetName();
        this.elementImage.sprite = elementSprite;
        boxImage.sprite = defaultSprite;
    }

    public void UpdateTexts()
    {
        skillNameText.text = this.skillName;
        defaultPos = this.transform.localPosition;

    }

    public void ChangeSelectState(bool onOff)
    {
        selectState = onOff;
        if (onOff == true)
        {
            boxImage.sprite = activeSprite;
            this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
        else
        {
            boxImage.sprite = defaultSprite;
            this.transform.localScale = Vector3.one;
        }
    }

    public SkillBase GetSkill()
    {
        return mySkill;
    }

    public string GetHelpText()
    {
        return mySkill.GetHelptext();
    }
}
