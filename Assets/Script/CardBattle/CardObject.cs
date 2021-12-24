using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public delegate void PlayedAction(CardData card);
    PlayedAction _playedActionDelegate;

    public delegate void Remove(CardObject card);
    Remove _remove;

    public CardData CardData { get => _cardData; }

    public bool InHand = false;

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="playedAction">プレイ時（カード使用時）に呼ぶアクション</param>
    /// <param name="func">手札に戻す処理時に呼ぶ関数</param>
    public void Setup(PlayedAction playedAction, Func func, Remove remove)
    {
        _playedActionDelegate = playedAction;
        _handUpdateDelegate = func;
        _remove = remove;
        UpdateText();
    }

    private void UpdateText()
    {
        _nameText.text = CardData.CardName;
        _image.sprite = CardData.MainImage;
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
                    Debug.Log($"{this.CardData.CardName}をプレイした");
                    if (_playedActionDelegate != null)
                    {
                        _playedActionDelegate.Invoke(_cardData);
                    }
                    _remove.Invoke(this);

                    this.transform.DOKill();
                    Destroy(this.gameObject);
                }
                _handUpdateDelegate.Invoke();
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
