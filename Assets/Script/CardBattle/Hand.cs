using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    /// <summary>
    /// 手札
    /// </summary>
    [SerializeField] List<CardObject> _cardList = new List<CardObject>();

    [SerializeField] CardObject _debugCard;

    CardObject.PlayedAction _playedActionDelegate;


    public void Setup(CardObject.PlayedAction playedAction)
    {
        _playedActionDelegate = playedAction;
    }

    /// <summary>
    /// 手札の追加
    /// </summary>
    public void AddHand(CardObject card)
    {
        _cardList.Add(card);
    }

    public void RemoveCard(CardObject card)
    {
        CardObject removeCard = card;
        _cardList.Remove(card);
    }

    public void AddHandDebug()
    {
        CardObject newCard = Instantiate(_debugCard, this.gameObject.transform);
        newCard.Setup(_playedActionDelegate, HandUpdate, RemoveCard);
        _cardList.Add(newCard);

        HandUpdate();
    }
    void HandUpdate()
    {
        Debug.Log("Updated");
        ListUpdate();
        for (int i = 0; i < _cardList.Count; i++)
        {
            _cardList[i].ResetSortingOrder();
            _cardList[i].transform.localPosition = new Vector2(i * 256, 0);
        }
    }

    void ListUpdate()
    {
        while (true)
        {
            bool flag = true;
            for (int i = 0; i < _cardList.Count; i++)
            {
                if (_cardList[i] == null)
                {
                    _cardList.RemoveAt(i);
                    flag = false;
                    break;
                }
            }
            if (flag == true)
            {
                break;
            }
        }
        
    }
}
