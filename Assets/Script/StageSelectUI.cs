using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] GameObject uiObjects;
    [SerializeField] SoundManager soundManager;
    [SerializeField] List<StageData> stageDataList = new List<StageData>();
    [SerializeField] int listTopIndex = 0;
    [SerializeField] int select = 0;
    public static int DecidedBattleId = 0;
    [SerializeField] StageData[] viewObjectArray;

    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] Image[] enemyImageList = new Image[3];

    public delegate void Func();

    Func BattleStartFunction;
    Func AfterClosed;

    public enum Direction
    {
        Up,
        Down,
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="BattleStartFunction">戦闘開始処理</param>
    /// <param name="AfterClosingUi">Uiを閉じた後の処理</param>
    public void Setup(Func BattleStartFunction, Func AfterClosingUi)
    {
        this.BattleStartFunction = BattleStartFunction;
        this.AfterClosed = AfterClosingUi;
        for (int i = 0; i < viewObjectArray.Length; i++)
        {
            viewObjectArray[i].stageName = stageDataList[i].stageName;
            viewObjectArray[i].stageInfoText = stageDataList[i].stageInfoText;
            viewObjectArray[i].enemyData = stageDataList[i].enemyData;
            viewObjectArray[i].UpdateView();
        }
        select = 0;
        UpdateSelect();
    }

    public void SetInput()
    {
        InputManager.ResetInputSettings();
        InputManager.InputUp += () => InputVertical(Direction.Up);
        InputManager.InputUp += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputDown += () => InputVertical(Direction.Down);
        InputManager.InputDown += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputEnter += () => StartBattle();
        InputManager.InputEnter += () => soundManager.PlaySe(SoundManager.SeList.Decided);
        InputManager.InputCancel = () => EndProcess();
        InputManager.setupCompleted = true;

    }

    public void Activate()
    {
        Player.canMove = false;
        GameManager2.canTalk = false;
        soundManager.PlaySe(SoundManager.SeList.OpenMenu);
        SetInput();
        uiObjects.SetActive(true);
        UpdateInfo();
    }

    public void InputVertical(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:

                if (select > 0)
                {
                    select--;
                }
                else
                {
                    if (listTopIndex > 0)
                    {
                        listTopIndex--;
                    }
                }

                break;
            case Direction.Down:
                if (select < viewObjectArray.Length - 1)
                {
                    select++;
                }
                else
                {
                    if (listTopIndex + viewObjectArray.Length < stageDataList.Count)
                    {
                        listTopIndex++;
                    }
                }

                break;
            default:
                break;
        }
        UpdateSelect();
    }

    public void UpdateSelect()
    {
        for (int i = 0; i < viewObjectArray.Length; i++)
        {
            viewObjectArray[i].stageName = stageDataList[i + listTopIndex].stageName;
            viewObjectArray[i].stageInfoText = stageDataList[i + listTopIndex].stageInfoText;
            viewObjectArray[i].enemyData = stageDataList[i + listTopIndex].enemyData;
            viewObjectArray[i].UpdateView();
        }
        foreach (var item in viewObjectArray)
        {
            item.ChangeState(false);
        }
        viewObjectArray[select].ChangeState(true);

        UpdateInfo();
    }

    void UpdateInfo()
    {
        infoText.text = stageDataList[listTopIndex + select].stageInfoText;

        for (int i = 0; i < 3; i++)
        {
            enemyImageList[i].sprite = stageDataList[listTopIndex + select].enemyData[i].GetImage();
        }


    }

    void StartBattle()
    {
        BattleManager2.stageData = stageDataList[listTopIndex + select];
        Debug.Log("StartBattle!!");
        if (BattleStartFunction != null)
        {
            BattleStartFunction.Invoke();
        }
    }

    void EndProcess()
    {
        InputManager.ResetInputSettings();
        InputManager.setupCompleted = false;
        uiObjects.SetActive(false);
        AfterClosed.Invoke();
        Player.canMove = true;
        GameManager2.canTalk = true;
        soundManager.PlaySe(SoundManager.SeList.CloseMenu);
    }
}
