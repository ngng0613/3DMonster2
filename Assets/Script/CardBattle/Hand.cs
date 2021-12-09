using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /// <summary>
    /// 手札
    /// </summary>
    [SerializeField] List<CardObject> cardList = new List<CardObject>();

    [SerializeField] CardObject debugCard;

    /// <summary>
    /// 手札の追加
    /// </summary>
    public void AddHand(CardObject card)
    {
        cardList.Add(card);

    }

    public void AddHandDebug()
    {
        CardObject newCard = Instantiate(debugCard, this.gameObject.transform);
        newCard.Setup(HandUpdate);
        cardList.Add(newCard);
        
        HandUpdate();
    }
    void HandUpdate()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].ResetSortingOrder();
            cardList[i].transform.localPosition = new Vector2(i * 256, 0);
        }
    }
}
