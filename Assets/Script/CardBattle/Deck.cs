using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<CardData> cardList = new List<CardData>();

    private void Start()
    {
        Shuffle();
    }

    /// <summary>
    /// デッキの初期設定
    /// </summary>
    /// <param name="cardList"></param>
    public void Setup(List<CardData> cardList)
    {
        this.cardList = cardList;
        Shuffle();
    }

    /// <summary>
    /// デッキの中身をシャッフル
    /// </summary>
    public void Shuffle()
    {
        System.Random random = new System.Random();

        List<CardData> temp = cardList.OrderBy(a => Guid.NewGuid()).ToList();
        cardList = temp;
        foreach (CardData item in cardList)
        {
            Debug.Log(item.cardName);
        }

    }

    /// <summary>
    /// 特定のカードをサーチする
    /// </summary>
    public void Search()
    {
        Shuffle();
    }
}
