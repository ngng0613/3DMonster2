using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject _resultTitle;
    Sequence _sequence;
    [SerializeField] Vector2 _titlePos;
    [SerializeField] float _displaySpeed = 1.0f;

    private void Start()
    {
        _sequence = DOTween.Sequence();
        AnimationStart();
    }
    public void AnimationStart()
    {
        _sequence.Append(_resultTitle.transform.DOLocalMoveX(_titlePos.x,_displaySpeed));
    }

}
