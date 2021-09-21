using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommandBoxManager : MonoBehaviour
{
    public delegate void SetUseSkillForBattleManager(SkillBase skill);
    SetUseSkillForBattleManager AfterDecidingTheCommand;



    public delegate void PhaseSelectSkillforBattleManager();
    public PhaseSelectSkillforBattleManager PhaseSkill;
    public Action BackToBeforePhaseForBattleManager;

    public enum Mode
    {
        Command,
        Skill,
        Item
    }
    Mode mode = Mode.Command;
    public enum SelectingInTheBox
    {
        Neutral,
        Up,
        Down,
        Left,
        Right,
        UpperRight,
        UpperLeft,
        LowerRight,
        LowerLeft
    }
    public SelectingInTheBox selectingInTheBox;

    float timeCount = 0;
    public bool isActive { get; set; } = false;

    [SerializeField] SkillBase defaultSkill;
    [SerializeField] SkillBase guardSkill;
    [SerializeField] SkillBase chargeSkill;

    [SerializeField] GameObject commandObject;
    [SerializeField] CommandBox commandBoxCenter;
    [SerializeField] CommandBox commandBoxTop;
    [SerializeField] CommandBox commandBoxBottom;
    [SerializeField] CommandBox commandBoxRight;
    [SerializeField] CommandBox commandBoxLeft;

    [SerializeField] GameObject commandParentObject;
    [SerializeField] Vector3 commandDefaultPos;
    [SerializeField] Vector3 commandHidePos;

    [SerializeField] SoundManager soundManager;

    [SerializeField] float commandMoveSpeed = 0.3f;

    public void Setup(SetUseSkillForBattleManager setUseSkill)
    {
        AfterDecidingTheCommand = setUseSkill;
        mode = Mode.Command;
        DisplayCommand(true);
        SetInput();
    }
    void SetInput()
    {
        InputManager.ResetInputSettings();
        switch (mode)
        {
            case Mode.Command:
                InputManager.InputEnter += InputEnter;
                InputManager.InputEnter += () => soundManager.PlaySe(SoundManager.SeList.Decided);
                InputManager.InputCancel += InputCancel;
                InputManager.InputCancel += () => soundManager.PlaySe(SoundManager.SeList.Decided);
                InputManager.InputSubButton1 += InputSub1;
                InputManager.InputSubButton1 += () => soundManager.PlaySe(SoundManager.SeList.Decided);
                InputManager.InputSubButton2 += InputSub2;
                InputManager.InputSubButton2 += () => soundManager.PlaySe(SoundManager.SeList.Decided);


                break;
            case Mode.Skill:
                InputManager.InputEnter += InputEnter;
                break;
            case Mode.Item:
                InputManager.InputEnter += InputEnter;
                break;
            default:
                break;
        }
        InputManager.setupCompleted = true;

    }


    /// <summary>
    /// コマンド決定
    /// </summary>
    void InputEnter()
    {
        if (mode == Mode.Command)
        {
            AfterDecidingTheCommand(defaultSkill);
        }
        DisplayCommand(false);
    }
    void InputCancel()
    {
        if (mode == Mode.Command)
        {
            //ガード選択
            AfterDecidingTheCommand(guardSkill);
        }
        DisplayCommand(false);
    }

    void InputSub1()
    {
        if (mode == Mode.Command)
        {
            commandBoxTop.OnPush();
            mode = Mode.Skill;
            PhaseSkill();

        }
    }
    void InputSub2()
    {
        if (mode == Mode.Command)
        {
            //ガード選択
            AfterDecidingTheCommand(chargeSkill);
        }
        DisplayCommand(false);
    }

    void ChooseCommandBox(SelectingInTheBox selectingInTheBox)
    {
        ResetCommandColor();
        switch (selectingInTheBox)
        {
            case SelectingInTheBox.Neutral:
                commandBoxCenter.ChangeImage(CommandBox.ImageColor.Orange);
                break;
            case SelectingInTheBox.Up:
                commandBoxTop.ChangeImage(CommandBox.ImageColor.Orange);
                break;
            case SelectingInTheBox.Down:
                commandBoxBottom.ChangeImage(CommandBox.ImageColor.Orange);
                break;
            case SelectingInTheBox.Left:
                commandBoxLeft.ChangeImage(CommandBox.ImageColor.Orange);
                break;
            case SelectingInTheBox.Right:
                commandBoxRight.ChangeImage(CommandBox.ImageColor.Orange);
                break;
            default:
                break;
        }
    }
    void ResetCommandColor()
    {
        commandBoxCenter.ChangeImage(CommandBox.ImageColor.Blue);
        commandBoxTop.ChangeImage(CommandBox.ImageColor.Blue);
        commandBoxBottom.ChangeImage(CommandBox.ImageColor.Blue);
        commandBoxRight.ChangeImage(CommandBox.ImageColor.Blue);
        commandBoxLeft.ChangeImage(CommandBox.ImageColor.Blue);
    }

    public void DisplayCommand(bool isActive)
    {
        if (isActive)
        {
            commandParentObject.SetActive(true);
            commandParentObject.transform.DOLocalMove(commandDefaultPos, commandMoveSpeed);
        }
        else
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(commandParentObject.transform.DOLocalMove(commandHidePos, commandMoveSpeed))
                    .AppendCallback(() => commandParentObject.SetActive(false));
        }

    }

}

