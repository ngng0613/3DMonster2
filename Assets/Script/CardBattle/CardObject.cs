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
    [SerializeField] TextMeshProUGUI _costText;
    [SerializeField] TextMeshProUGUI _flavourtText;
    [SerializeField] Image _image;
    [SerializeField] Image _arrowImage;
    [SerializeField] float _playArea;

    int _sortingOrder = 0;
    Vector3 _relativePos;
    Vector3 _firstMousePos;

    public delegate void Func();
    Func _handUpdateDelegate;

    public delegate void PlayedAction(CardData card);
    PlayedAction _playedActionDelegate;

    public delegate void Remove(CardObject card);
    Remove _remove;

    public delegate bool CheckDelegate(CardData card);
    public CheckDelegate Check;

    public CardData Data { get => _cardData; set => _cardData = value; }

    public bool InHand = false;

    public bool InBattle = false;

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="playedAction">プレイ時（カード使用時）に呼ぶアクション</param>
    /// <param name="handUpdate">手札に戻す処理時に呼ぶ関数</param>
    public void Setup(PlayedAction playedAction, Func handUpdate, Remove remove)
    {
        this.gameObject.transform.localScale = Vector3.one;
        _playedActionDelegate = playedAction;
        _handUpdateDelegate = handUpdate;
        _remove = remove;
        UpdateText();
        Color tempColor = _arrowImage.color;
        tempColor.a = 0;
        _arrowImage.color = tempColor;
    }


    public void UpdateText()
    {
        _nameText.text = Data.CardName;
        _image.sprite = Data.MainImage;
        _image.color = Color.white;
        _costText.text = Data.Cost.ToString();
        _flavourtText.text = Data.FlavourText;
    }

    IEnumerator PlayUpdate()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
               
                if (Input.mousePosition.y >= _playArea)
                {
                    if (Check.Invoke(Data) == false)
                    {
                        Debug.LogWarning("マナが不足しています");
                        _handUpdateDelegate.Invoke();
                        break;
                    }
                    Debug.Log($"{this.Data.CardName}をプレイした");
                    if (_playedActionDelegate != null)
                    {
                        _playedActionDelegate.Invoke(_cardData);
                    }
                    else
                    {
                        Debug.Log("効果がない");
                    }
                    _remove.Invoke(this);

                    this.transform.DOKill();
                    this.transform.position = new Vector3(-200, -500, -1000);
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
        if (InBattle == true)
        {
            this.gameObject.transform.DOLocalMoveY(156, 0.2f);
            this.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            _canvas.sortingOrder = 50;
            Color tempColor = _arrowImage.color;
            tempColor.a = 1;
            _arrowImage.color = tempColor;
        }
    }
    public void OnPointerExit()
    {
        if (InBattle == true)
        {
            this.gameObject.transform.DOLocalMoveY(0, 0.2f);
            this.gameObject.transform.DOScale(Vector3.one, 0.2f);
            _canvas.sortingOrder = _sortingOrder;
            Color tempColor = _arrowImage.color;
            tempColor.a = 0;
            _arrowImage.color = tempColor;
        }

    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && InBattle)
        {
            _relativePos = this.gameObject.transform.position;
            _firstMousePos = Input.mousePosition;
            StartCoroutine(PlayUpdate());
        }
    }

    public void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && InBattle)
        {
            this.gameObject.transform.position = _relativePos + Input.mousePosition - _firstMousePos;
            Debug.Log(Input.mousePosition.y);
        }
    }

    public void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && InBattle)
        {
            _handUpdateDelegate.Invoke();
        }
    }

    public void ResetSortingOrder()
    {
        _canvas.sortingOrder = 0;
    }
    public void SetSortingOrder(int order)
    {
        _sortingOrder = order;
        _canvas.sortingOrder = order;
    }
}
