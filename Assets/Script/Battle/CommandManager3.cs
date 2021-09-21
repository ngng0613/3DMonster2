using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CommandManager3 : MonoBehaviour
{
    public delegate void D(SkillBase skill);
    D delegate1;
    D AfterDecidingTheCommand;
    public event Action a { add => ad += value; remove => ad -= value; }
    protected Action ad; //継承用

    public enum Mode
    {
        Command,
        Skill,
        Item
    }
    Mode mode = Mode.Command;
    public enum SelectingInTheCircle
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
    public SelectingInTheCircle selectingInTheCircle;

    float timeCount = 0;
    public bool isActive { get; set; } = false;

    List<SkillBase> skillList = new List<SkillBase>();
    SkillBase SelectedSkill;

    int SelectedSkillNumber = 0;

    [SerializeField] InputManager inputManager;
    [SerializeField] SkillBase defaultSkill;
    [SerializeField] Color commandDefaultColor1;
    [SerializeField] Color commandDefaultColor2;
    [SerializeField] Color commandSelectColor;

    [SerializeField] GameObject commandObject;
    [SerializeField] CommandCircle commandCenter;
    [SerializeField] CommandCircle command4Top;
    [SerializeField] CommandCircle command4Bottom;
    [SerializeField] CommandCircle command4Right;
    [SerializeField] CommandCircle command4Left;

    [SerializeField] CommandCircle command8Top;
    [SerializeField] CommandCircle command8Bottom;
    [SerializeField] CommandCircle command8Right;
    [SerializeField] CommandCircle command8Left;
    [SerializeField] CommandCircle command8UpperRight;
    [SerializeField] CommandCircle command8UpperLeft;
    [SerializeField] CommandCircle command8LowerRight;
    [SerializeField] CommandCircle command8LowerLeft;


    public void Setup(D d)
    {
        AfterDecidingTheCommand = d;

        SetInput();
    }
    void SetInput()
    {

        switch (mode)
        {
            case Mode.Command:
                InputManager.InputEnter += InputEnter;
                InputManager.InputUp = () => CommandCircle(SelectingInTheCircle.Up);
                InputManager.InputDown = () => CommandCircle(SelectingInTheCircle.Down);
                InputManager.InputRight = () => CommandCircle(SelectingInTheCircle.Right);
                InputManager.InputLeft = () => CommandCircle(SelectingInTheCircle.Left);
                InputManager.InputNeutral = () => CommandCircle(SelectingInTheCircle.Neutral);
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
    }
    void InputCancel()
    {
        if (mode == Mode.Command)
        {

        }

    }

    public void ChangeCommandColor()
    {

    }

    void CommandCircle(SelectingInTheCircle selectingInTheCircle)
    {
        ResetCommandColor();

        switch (selectingInTheCircle)
        {
            case SelectingInTheCircle.Neutral:
                //commandCenter.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.Up:
                command4Top.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.Down:
                command4Bottom.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.Left:
                command4Left.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.Right:
                command4Right.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.UpperRight:
                command8UpperRight.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.UpperLeft:
                command8UpperLeft.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.LowerRight:
                command8LowerRight.ChangeColor(commandSelectColor);
                break;
            case SelectingInTheCircle.LowerLeft:
                command8LowerLeft.ChangeColor(commandSelectColor);
                break;
            default:
                break;
        }
    }
    void ResetCommandColor()
    {
        commandCenter.ChangeColor(commandDefaultColor2);
        command4Top.ChangeColor(commandDefaultColor1);
        command4Bottom.ChangeColor(commandDefaultColor1);
        command4Right.ChangeColor(commandDefaultColor1);
        command4Left.ChangeColor(commandDefaultColor1);
    }





}
