using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /// <summary>
    /// 手札
    /// </summary>
    [SerializeField] List<CardObject> _cardList = new List<CardObject>();

    [SerializeField] CardObject _debugCard;

    /// <summary>
    /// 手札の追加
    /// </summary>
    public void AddHand(CardObject card)
    {
        _cardList.Add(card);

    }

    public void AddHandDebug()
    {
        CardObject newCard = Instantiate(_debugCard, this.gameObject.transform);
        newCard.Setup(HandUpdate);
        _cardList.Add(newCard);
        
        HandUpdate();
    }
    void HandUpdate()
    {
        for (int i = 0; i < _cardList.Count; i++)
        {
            _cardList[i].ResetSortingOrder();
            _cardList[i].transform.localPosition = new Vector2(i * 256, 0);
        }
    }
}
