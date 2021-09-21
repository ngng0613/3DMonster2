using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TargetView : MonoBehaviour
{

    [SerializeField] BattleCamera battleCamera;
    [SerializeField] float cameraMovementTime = 0.5f;
    [SerializeField] List<Transform> playerMonsterLookPositionList = new List<Transform>();
    [SerializeField] List<Transform> enemyMonsterLookPositionList = new List<Transform>();
    [SerializeField] List<MonsterBase> playerMonsterBaseList = new List<MonsterBase>();
    [SerializeField] List<MonsterBase> enemyMonsterBaseList = new List<MonsterBase>();
    [SerializeField] InputManager inputManager;
    [SerializeField] GameObject targetCirle;
    [SerializeField] TextMeshProUGUI targetName;
    [SerializeField] SoundManager soundManager;

    public int maximumNumberOfMonster = 3;
    public int selectedMonster = 0;
    public bool IsTheCameraMoving = false;

    List<bool> IsPlayermonsterAlive = new List<bool>() { false, false, false };
    List<bool> IsEnemymonsterAlive = new List<bool>() { false, false, false };

    Sequence sequence;
    public delegate void D(BattleMonsterTag.CharactorTag charactorNumber);
    D AfterDecidingTheTarget;

    public Action BackToBeforePhaseForBattleManager;

    public enum TargetType
    {
        PlayerSide,
        EnemySide
    }
    public TargetType targetType;

    public enum InputDirection
    {
        Right,
        Left
    }

    public void Setup(List<MonsterBase> playerList, List<MonsterBase> enemyList)
    {
        this.playerMonsterBaseList = playerList;
        this.enemyMonsterBaseList = enemyList;
    }

    /// <summary>
    /// キャラクターをズームして表示する
    /// </summary>
    /// <param name="type">味方か敵か</param>
    /// <param name="charactorNumber">キャラクターの番号</param>
    public void ViewTarget(int charactorNumber)
    {
        switch (charactorNumber)
        {
            case 0:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1, cameraMovementTime);
                break;

            case 1:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2, cameraMovementTime);
                break;

            case 2:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3, cameraMovementTime);
                break;

            case 10:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1, cameraMovementTime);
                targetName.text = enemyMonsterBaseList[0].GetNickname();
                break;

            case 11:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2, cameraMovementTime);
                targetName.text = enemyMonsterBaseList[1].GetNickname();
                break;

            case 12:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3, cameraMovementTime);
                targetName.text = enemyMonsterBaseList[0].GetNickname();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// ターゲットの準備
    /// </summary>
    public void TargetPreparation(TargetType type, bool[] IsTheTargetAlive, D AfterDecidingTheTarget)
    {
        targetType = type;
        this.AfterDecidingTheTarget = AfterDecidingTheTarget;
        if (type == TargetType.EnemySide)
        {
            for (int i = 0; i < IsTheTargetAlive.Length; i++)
            {
                IsEnemymonsterAlive[i] = IsTheTargetAlive[i];
            }

            if (IsEnemymonsterAlive[0] == true)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1, 0.0f);
                selectedMonster = 0;
            }
            else if (IsEnemymonsterAlive[1] == true)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2, 0.0f);
                selectedMonster = 1;
            }
            else if (IsEnemymonsterAlive[2] == true)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3, 0.0f);
                selectedMonster = 2;
            }
        }
        if (targetType == TargetType.PlayerSide)
        {
            targetName.text = playerMonsterBaseList[selectedMonster].GetNickname();

        }
        else
        {
            targetName.text = enemyMonsterBaseList[selectedMonster].GetNickname();
        }
        targetCirle.SetActive(true);

        InputManager.ResetInputSettings();
        InputManager.InputRight += () => ChangeTarget(InputDirection.Right);
        InputManager.InputRight += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputLeft += () => ChangeTarget(InputDirection.Left);
        InputManager.InputLeft += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputEnter += () => TargetDecidied();
        InputManager.InputEnter += () => soundManager.PlaySe(SoundManager.SeList.Decided);
        InputManager.InputCancel += () => BackToBeforePhase();
        InputManager.InputCancel += () => soundManager.PlaySe(SoundManager.SeList.Cancel);
    }


    /// <summary>
    /// 左右の入力によってズームするターゲットを変更する処理
    /// </summary>
    /// <param name="input"></param>
    public void ChangeTarget(InputDirection input)
    {


        if (IsTheCameraMoving)
        {
            return;
        }


        if (input == InputDirection.Right)
        {

            if (selectedMonster != maximumNumberOfMonster - 1)
            {
                selectedMonster++;
            }
            else
            {
                selectedMonster = 0;
            }
            int i = 0;
            while (!IsEnemymonsterAlive[selectedMonster])
            {
                if (selectedMonster != maximumNumberOfMonster - 1)
                {
                    selectedMonster++;
                }
                else
                {
                    selectedMonster = 0;
                }
                i++;
                if (i > 10)
                {
                    break;
                }
            }


        }
        else if (input == InputDirection.Left)
        {
            if (selectedMonster != 0)
            {
                selectedMonster--;
            }
            else
            {
                selectedMonster = maximumNumberOfMonster - 1;
            }
            int i = 0;
            while (!IsEnemymonsterAlive[selectedMonster])
            {
                if (selectedMonster != 0)
                {
                    selectedMonster--;
                }
                else
                {
                    selectedMonster = maximumNumberOfMonster - 1;
                }
                i++;
                if (i > 10)
                {
                    break;
                }
            }
        }
        if (targetType == TargetType.PlayerSide)
        {
            targetName.text = playerMonsterBaseList[selectedMonster].GetNickname();
        }
        else
        {
            targetName.text = enemyMonsterBaseList[selectedMonster].GetNickname();
        }
        TargetEnemyMonster(selectedMonster);
    }

    /// <summary>
    /// ターゲット（敵）にカメラを向ける処理
    /// </summary>
    /// <param name="targetNumber"></param>
    public void TargetEnemyMonster(int targetNumber)
    {
        IsTheCameraMoving = true;
        sequence = DOTween.Sequence();
        sequence.Append(battleCamera.transform.DOMove(enemyMonsterLookPositionList[targetNumber].transform.position, cameraMovementTime))
                .Insert(0.0f, battleCamera.transform.DORotate(enemyMonsterLookPositionList[0].transform.localEulerAngles, cameraMovementTime))
                .AppendCallback(() => IsTheCameraMoving = false);
    }

    /// <summary>
    /// ターゲット決定時の処理
    /// </summary>
    public void TargetDecidied()
    {
        if (IsTheCameraMoving)
        {
            return;
        }

        targetCirle.SetActive(false);
        sequence.Kill();
        InputManager.ResetInputSettings();
        if (targetType == TargetType.EnemySide)
        {
            selectedMonster += 10;
        }
        AfterDecidingTheTarget((BattleMonsterTag.CharactorTag)selectedMonster);
    }

    public void BackToBeforePhase()
    {
        targetCirle.SetActive(false);
        InputManager.ResetInputSettings();
        BackToBeforePhaseForBattleManager.Invoke();

    }
}
