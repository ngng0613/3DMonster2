using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Banner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool _isActive = false;
    [SerializeField] float _moveSpeed = 1.0f;
    Vector3 _deactivePos;
    [SerializeField] Vector3 _hidePos;
    [SerializeField] Vector3 _activePos;
    Sequence _sequence;

    public delegate void OnClickEventDelegate();
    public OnClickEventDelegate OnClickEvent;

    private void Start()
    {
        _sequence = DOTween.Sequence();
        _deactivePos = transform.localPosition;
    }

    public void MoveToNeutralPos()
    {
        _sequence.Kill();
        _isActive = false;
        _sequence.Append(this.transform.DOLocalMoveX(_deactivePos.x, 1 / _moveSpeed));
    }

    public void Hide()
    {
        _sequence.Kill();
        _isActive = true;
        _sequence.Append(this.transform.DOLocalMoveX(_hidePos.x, 1 / _moveSpeed * 1.5f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sequence.Kill();
        _isActive = true;
        _sequence.Append(this.transform.DOLocalMoveX(_activePos.x, 1 / _moveSpeed));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MoveToNeutralPos();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent.Invoke();
    }
}
