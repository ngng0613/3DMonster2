using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardObject : MonoBehaviour
{
    [SerializeField] CardData _cardData;
    [SerializeField] Canvas _canvas;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] Image _image;

    Vector3 _relativePos;
    Vector3 _firstMousePos;

    public delegate void Func();
    Func _handUpdateDelegate;

    Action _playedAction;

    public CardData CardData { get => _cardData;}
    public bool InHand = false;

    public void Setup(Func func)
    {
        _handUpdateDelegate = func;
        UpdateText();
    }

    /// <summary>
    /// カードをプレイした際の処理を追加する
    /// </summary>
    /// <param name="func">追加したい関数</param>
    public void SetAction(Func func)
    {
        _playedAction += () => func();
    }

    private void UpdateText()
    {
        _nameText.text = CardData._cardName;
        _image.sprite = CardData._mainImage;
    }

    IEnumerator PlayUpdate()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("解除");
                if (Input.mousePosition.y >= 600)
                {
                    Debug.Log($"{this.CardData._cardName}をプレイした");
                    if (_playedAction != null)
                    {
                        _playedAction.Invoke();
                    }
                }
                break;
            }
            yield return null;
        }
        yield return null;
    }

    public void OnPointerEnter()
    {
        this.gameObject.transform.DOLocalMoveY(156, 0.2f);
        this.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        _canvas.sortingOrder = 1;
    }
    public void OnPointerExit()
    {
        this.gameObject.transform.DOLocalMoveY(0, 0.2f);
        this.gameObject.transform.DOScale(Vector3.one, 0.2f);
        _canvas.sortingOrder = 0;
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            _relativePos = this.gameObject.transform.position;
            _firstMousePos = Input.mousePosition;
            StartCoroutine(PlayUpdate());
        }
    }

    public void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            this.gameObject.transform.position = _relativePos + Input.mousePosition - _firstMousePos;
        }
    }

    public void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _handUpdateDelegate.Invoke();
        }
    }

    public void ResetSortingOrder()
    {
        _canvas.sortingOrder = 0;
    }
}
