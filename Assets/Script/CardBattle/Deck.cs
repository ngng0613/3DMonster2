using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    List<CardData> cardList = new List<CardData>();

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
        
    }

    /// <summary>
    /// 特定のカードをサーチする
    /// </summary>
    public void Search()
    {
        
    }
}
