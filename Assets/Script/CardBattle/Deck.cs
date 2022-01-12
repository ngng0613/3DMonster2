using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck operator +(Deck a, Deck b)
    {
        Deck newDeck = new Deck();
        newDeck._cardList.AddRange(a._cardList);
        newDeck._cardList.AddRange(b._cardList);
        return newDeck; 
    }

    //デッキの中身
    [SerializeField] List<CardObject> _cardList = new List<CardObject>();
    //捨て札
    [SerializeField] List<CardObject> _trashList = new List<CardObject>();

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
        if (_cardList.Count <= 0)
        {
            if (_trashList.Count <= 0)
            {
                return null;
            }
            _cardList = _trashList;
            _trashList = new List<CardObject>();
            Shuffle();
        }
        
        CardObject drawCard = _cardList[0];
        _cardList.RemoveAt(0);
        drawCard.InHand = true;
        return drawCard;
    }

    public void Trash(CardObject card)
    {
        _trashList.Add(card);
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
            Debug.Log(item.Data.CardName);
        }
    }

    /// <summary>
    /// 特定のカードをサーチする
    /// </summary>
    public void Search()
    {
    }
}
