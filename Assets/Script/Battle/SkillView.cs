using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillView : MonoBehaviour
{
    [SerializeField] PartOfSkillView partBasePrefab;
    [SerializeField] IconManager iconManager;
    [SerializeField] GameObject parentObject;
    List<PartOfSkillView> skillList = new List<PartOfSkillView>();
    [SerializeField] TextMeshProUGUI skillHelpText;

    [SerializeField] SoundManager soundManager;

    int selectingNumber = 0;
    SkillBase chooseSkill;

    public delegate void SetUseSkillOfBattleManager(SkillBase skill);
    public SetUseSkillOfBattleManager SetUseSkill;

    public Action BackToPhaseForBattleManager;

    public enum InputDirectionTag
    {
        Up,
        Down,
        Right,
        Left,
    }
    public void Setup(List<SkillBase> skillList)
    {
        selectingNumber = 0;
        if (this.skillList.Count >= 1)
        {
            foreach (var item in this.skillList)
            {
                Destroy(item.gameObject);
            }
        }

        this.skillList = new List<PartOfSkillView>();
        for (int i = 0; i < skillList.Count; i++)
        {
            PartOfSkillView partOfSkillView = Instantiate(partBasePrefab);
            Sprite elementImage = iconManager.GetIconImage(skillList[i].GetElement());

            SelectionObject selectionComponent = partOfSkillView.GetComponent<SelectionObject>();

            partOfSkillView.Setup(skillList[i], elementImage);
            partOfSkillView.transform.SetParent(parentObject.transform);
            if (i == 0)
            {
                partOfSkillView.ChangeSelectState(true);
            }
            partOfSkillView.UpdateTexts();
            this.skillList.Add(partOfSkillView);

        }
        UpdateHelp();
    }

    public void SetInput()
    {
        InputManager.ResetInputSettings();
        InputManager.InputUp += () => InputDirection(InputDirectionTag.Up);
        InputManager.InputUp += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputDown += () => InputDirection(InputDirectionTag.Down);
        InputManager.InputDown += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputEnter += () => InputEnter();
        InputManager.InputEnter += () => soundManager.PlaySe(SoundManager.SeList.Decided);
        InputManager.InputCancel += () => BackToBeforePhase();
        InputManager.InputCancel += () => soundManager.PlaySe(SoundManager.SeList.Cancel);

    }

    public void Display(bool onOff)
    {
        this.gameObject.SetActive(onOff);
    }

    /// <summary>
    /// スキル説明の更新
    /// </summary>
    public void UpdateHelp()
    {
        this.skillHelpText.text = skillList[selectingNumber].GetHelpText();
    }

    void InputEnter()
    {
        chooseSkill = skillList[selectingNumber].GetSkill();
        SetUseSkill(chooseSkill);

        //スキルビューを閉じる処理
        Display(false);


    }

    public void InputDirection(InputDirectionTag tag)
    {
        Debug.Log($"selected : {selectingNumber}");
        switch (tag)
        {
            case InputDirectionTag.Up:


                if (selectingNumber > 0)
                {
                    selectingNumber--;
                }
                else
                {
                    selectingNumber = skillList.Count - 1;
                }

                break;
            case InputDirectionTag.Down:

                if (selectingNumber < skillList.Count - 1)
                {
                    selectingNumber++;
                }
                else
                {
                    selectingNumber = 0;
                }

                break;
            case InputDirectionTag.Right:
                break;
            case InputDirectionTag.Left:
                break;
            default:
                break;
        }
        foreach (var item in skillList)
        {
            item.ChangeSelectState(false);
        }

        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].ChangeSelectState(false);
        }

        skillList[selectingNumber].ChangeSelectState(true);
        UpdateHelp();
    }

    public void BackToBeforePhase()
    {
        InputManager.ResetInputSettings();
        Display(false);
        BackToPhaseForBattleManager.Invoke();

    }
}
