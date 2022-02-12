using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Banner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool _isActive = false;
    [SerializeField] float _moveSpeed = 1.0f;
    Vector3 _deactivePos;
    [SerializeField] Vector3 _activePos;
    Sequence _sequence;


    private void Start()
    {
        _sequence = DOTween.Sequence();
        _deactivePos = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sequence.Kill();
        _isActive = true;
        _sequence.Append(this.transform.DOLocalMoveX(_activePos.x, 1 / _moveSpeed));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _sequence.Kill();
        _isActive = false;
        _sequence.Append(this.transform.DOLocalMoveX(_deactivePos.x, 1 / _moveSpeed));
    }
}
