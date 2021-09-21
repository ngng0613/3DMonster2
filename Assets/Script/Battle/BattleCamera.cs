using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleCamera : MonoBehaviour
{
    Sequence sequence;


    public enum CameraPosition
    {
        DefaultPositon,
        Player1,
        Player2,
        Player3,
        Enemy1,
        Enemy2,
        Enemy3
    }

    [SerializeField] Transform cameraTransform;

    [SerializeField] Transform cameraDefaultPos;

    [SerializeField] Transform playerMonster1CameraPos;
    [SerializeField] Transform playerMonster2CameraPos;
    [SerializeField] Transform playerMonster3CameraPos;
    [SerializeField] Transform enemyMonster1CameraPos;
    [SerializeField] Transform enemyMonster2CameraPos;
    [SerializeField] Transform enemyMonster3CameraPos;

    [SerializeField] Transform enemyViewStartPoint;
    [SerializeField] Transform enemyViewEndPoint;


    [SerializeField] float moveSpeed = 0.5f;
    public delegate void Func();

    public void SetCameraPosition(CameraPosition position)
    {
        switch (position)
        {
            case CameraPosition.DefaultPositon:
                this.transform.position = cameraDefaultPos.transform.position;
                this.transform.rotation = cameraDefaultPos.transform.rotation;
                break;

            case CameraPosition.Player1:
                this.transform.position = playerMonster1CameraPos.transform.position;
                this.transform.rotation = playerMonster1CameraPos.transform.rotation;
                break;
            case CameraPosition.Player2:
                this.transform.position = playerMonster2CameraPos.transform.position;
                this.transform.rotation = playerMonster2CameraPos.transform.rotation;
                break;
            case CameraPosition.Player3:
                this.transform.position = playerMonster3CameraPos.transform.position;
                this.transform.rotation = playerMonster3CameraPos.transform.rotation;
                break;
            case CameraPosition.Enemy1:
                this.transform.position = enemyMonster1CameraPos.transform.position;
                this.transform.rotation = enemyMonster1CameraPos.transform.rotation;
                break;
            case CameraPosition.Enemy2:
                this.transform.position = enemyMonster2CameraPos.transform.position;
                this.transform.rotation = enemyMonster2CameraPos.transform.rotation;
                break;
            case CameraPosition.Enemy3:
                this.transform.position = enemyMonster3CameraPos.transform.position;
                this.transform.rotation = enemyMonster3CameraPos.transform.rotation;
                break;
            default:
                break;
        }
    }

    public void SetCameraPosition(CameraPosition position, float speed)
    {
        switch (position)
        {
            case CameraPosition.DefaultPositon:
                MovePosition(cameraDefaultPos, speed);
                break;

            case CameraPosition.Player1:
                MovePosition(playerMonster1CameraPos, speed);
                break;
            case CameraPosition.Player2:
                MovePosition(playerMonster2CameraPos, speed);
                break;
            case CameraPosition.Player3:
                MovePosition(playerMonster3CameraPos, speed);
                break;
            case CameraPosition.Enemy1:
                MovePosition(enemyMonster1CameraPos, speed);
                break;
            case CameraPosition.Enemy2:
                MovePosition(enemyMonster2CameraPos, speed);
                break;
            case CameraPosition.Enemy3:
                MovePosition(enemyMonster3CameraPos, speed);
                break;
            default:
                break;
        }
    }
    void MovePosition(Transform PositionAfterMovement, float speed)
    {
        sequence = DOTween.Sequence();
        sequence.Append(cameraTransform.DOMove(PositionAfterMovement.position, speed))
                .Insert(0.0f, cameraTransform.DORotate(PositionAfterMovement.localEulerAngles, speed));
    }



    public void AllEnemyView(Func func)
    {
        this.transform.position = enemyViewStartPoint.position;
        this.transform.rotation = enemyViewStartPoint.rotation;
        sequence.Append(cameraTransform.DOMove(enemyViewEndPoint.position, moveSpeed))
                .AppendCallback(() => func());

    }

}
