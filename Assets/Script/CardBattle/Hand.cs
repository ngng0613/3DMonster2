using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /// <summary>
    /// 手札
    /// </summary>
    List<CardData> cardList;

    /// <summary>
    /// 手札の追加
    /// </summary>
    void AddHand(CardData card)
    {
        cardList.Add(card);

    }

    void HandUpdate()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            
        }
    }
}
