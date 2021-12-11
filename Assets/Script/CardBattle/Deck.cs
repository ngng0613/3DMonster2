using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<CardObject> cardList = new List<CardObject>();

    /// <summary>
    /// デッキの初期設定
    /// </summary>
    /// <param name="cardList"></param>
    public void Setup(List<CardObject> cardList)
    {
        this.cardList = cardList;
        Shuffle();
    }

    public CardObject Draw()
    {
        CardObject drawCard = cardList[0];
        cardList.RemoveAt(0);
        drawCard.InHand = true;
        return drawCard;
    }

    /// <summary>
    /// デッキの中身をシャッフル
    /// </summary>
    public void Shuffle()
    {
        List<CardObject> temp = cardList.OrderBy(a => Guid.NewGuid()).ToList();
        cardList = temp;
        foreach (CardObject item in cardList)
        {
            Debug.Log(item.CardData.cardName);
        }
    }

    /// <summary>
    /// 特定のカードをサーチする
    /// </summary>
    public void Search()
    {
    }
}
