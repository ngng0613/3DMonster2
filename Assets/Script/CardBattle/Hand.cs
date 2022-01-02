using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void TrashDelegate(CardObject card);

public class Hand : MonoBehaviour
{
    public bool IsEnemy = false; 
    /// <summary>
    /// 手札
    /// </summary>
    [SerializeField] List<CardObject> _cardList = new List<CardObject>();

    [SerializeField] CardObject _debugCard;

    CardObject.PlayedAction _playedActionDelegate;
    TrashDelegate _trash;

    public List<CardObject> CardList { get => _cardList; }

    public void Setup(CardObject.PlayedAction playedAction, TrashDelegate trash)
    {
        _playedActionDelegate = playedAction;
        _trash = trash;
    }

    /// <summary>
    /// 手札の追加
    /// </summary>
    public void AddHand(CardObject card)
    {
        
        CardList.Add(card);
        if (IsEnemy == false)
        {
            card.transform.SetParent(this.transform);
            card.Setup(_playedActionDelegate, HandUpdate, RemoveCard);
            HandUpdate();
        }
    }

    public void RemoveCard(CardObject card)
    {
        CardObject removeCard = card;
        _trash(removeCard);
        CardList.Remove(card);
    }
    public void RemoveCard(CardData card)
    {
        for (int i = 0; i < _cardList.Count; i++)
        {
            if (_cardList[i].CardData == card)
            {
                _trash(_cardList[i]);
                _cardList.RemoveAt(i);
                break;
            }
        }
    }

    public void AddHandDebug()
    {
        CardObject newCard = Instantiate(_debugCard, this.gameObject.transform);
        newCard.Setup(_playedActionDelegate, HandUpdate, RemoveCard);
        CardList.Add(newCard);

        HandUpdate();
    }
    void HandUpdate()
    {
        ListUpdate();
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].ResetSortingOrder();
            CardList[i].transform.localPosition = new Vector2(i * 256, 0);
        }
    }

    void ListUpdate()
    {
        while (true)
        {
            bool flag = true;
            for (int i = 0; i < CardList.Count; i++)
            {
                if (CardList[i] == null)
                {
                    CardList.RemoveAt(i);
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
