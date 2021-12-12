using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<CardObject> _cardList = new List<CardObject>();

    /// <summary>
    /// デッキの初期設定
    /// </summary>
    /// <param name="cardList"></param>
    public void Setup(List<CardObject> cardList)
    {
        this._cardList = cardList;
        Shuffle();
    }

    public CardObject Draw()
    {
        CardObject drawCard = _cardList[0];
        _cardList.RemoveAt(0);
        drawCard.InHand = true;
        return drawCard;
    }

    /// <summary>
    /// デッキの中身をシャッフル
    /// </summary>
    public void Shuffle()
    {
        List<CardObject> temp = _cardList.OrderBy(a => Guid.NewGuid()).ToList();
        _cardList = temp;
        foreach (CardObject item in _cardList)
        {
            Debug.Log(item.CardData._cardName);
        }
    }

    /// <summary>
    /// 特定のカードをサーチする
    /// </summary>
    public void Search()
    {
    }
}
