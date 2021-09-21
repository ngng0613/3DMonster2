using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnDisplay : MonoBehaviour
{
    [SerializeField] Image playerTurnImage;
    [SerializeField] Image enemyTurnImage;

    [SerializeField] Vector3 displayPos;
    [SerializeField] Vector3 hidePos;

    [SerializeField] float tweenSpeed = 0.6f;

    public enum DisplayPattern
    {
        None,
        PlayerTurn,
        EnemyTurn,
    }


    public void DisplayImage(DisplayPattern pattern)
    {
        Sequence sequence = DOTween.Sequence();
        switch (pattern)
        {
            case DisplayPattern.None:
                playerTurnImage.transform.DOLocalMoveY(hidePos.y, tweenSpeed);
                enemyTurnImage.transform.DOLocalMoveY(hidePos.y, tweenSpeed);

                break;

            case DisplayPattern.PlayerTurn:
                sequence = DOTween.Sequence();
                if (enemyTurnImage.transform.localPosition == displayPos)
                {
                    sequence.Append(enemyTurnImage.transform.DOLocalMoveY(hidePos.y, tweenSpeed));
                }
                sequence.Append(playerTurnImage.transform.DOLocalMoveY(displayPos.y, tweenSpeed));
                break;

            case DisplayPattern.EnemyTurn:

                sequence = DOTween.Sequence();
                if (playerTurnImage.transform.localPosition == displayPos)
                {
                    sequence.Append(playerTurnImage.transform.DOLocalMoveY(hidePos.y, tweenSpeed));
                }
                sequence.Append(enemyTurnImage.transform.DOLocalMoveY(displayPos.y, tweenSpeed));
                break;

            default:
                break;
        }

    }
}
