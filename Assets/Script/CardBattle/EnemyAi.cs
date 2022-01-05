using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] MonsterBase _monster;
    [SerializeField] int _aiLevel = 0;
    [SerializeField] Deck _deck;
    [SerializeField] Hand _hand;
    [SerializeField] int _mana = 0;

    [SerializeField] int _ratingScore = 0;
    ComboData _decidedCombo = null;

    [Header("コンボリスト"), SerializeField] List<ComboData> comboDictionary;

    public Deck Deck { get => _deck; set => _deck = value; }
    public Hand Hand { get => _hand; set => _hand = value; }

    public void Setup(MonsterBase monster)
    {
        this._monster = monster;
        _hand.Setup(null, _deck.Trash);
        _hand.IsEnemy = true;
    }

    public void Draw()
    {
        for (int i = 0; i < 2; i++)
        {
            CardObject card = _deck.Draw();
            if (card != null)
            {
                _hand.AddHand(card);
            }
        }


    }

    /// <summary>
    /// コンボを思考し、使用するカードのリストを返す
    /// また、使用したカードは手札から削除する
    /// </summary>
    /// <returns>使用するカードのリスト</returns>
    public List<CardData> Think()
    {
        Draw();
        List<CardObject> cardObjectList = _hand.CardList;
        List<CardData> handList = new List<CardData>();
        _decidedCombo = null;
        for (int i = 0; i < cardObjectList.Count; i++)
        {
            handList.Add(cardObjectList[i].CardData);
        }

        //使用可能なコンボのデータを入れるList
        List<ComboData> compareComboList = new List<ComboData>();

        for (int i = 0; i < handList.Count; i++)
        {
            Debug.Log(handList[i]);
            CardData firstCard = handList[i];
            ///コンボを思考するうえでコンボで既に使用したカードをチェックするため、boolで管理する
            bool[] useCard = new bool[handList.Count];
            useCard[i] = true;
            for (int k = 0; k < comboDictionary.Count; k++)
            {
                ComboData combo = comboDictionary[k];
                //そのコンボでどれだけマナを消費するか
                int useMana = 0;
                for (int x = 0; x < combo.OrderOfCards.Count; x++)
                {
                    useMana += combo.OrderOfCards[x].Cost;
                }
                //消費マナが足りない場合はスキップする
                if (useMana > _monster.CurrentMp)
                {
                    continue;
                }
                if (comboDictionary[k].OrderOfCards.Count == 0)
                {
                    continue;
                }
                if (firstCard == comboDictionary[k].OrderOfCards[0])
                {
                    //コンボ可能かどうかを示す変数
                    bool canDo = false;
                    List<CardData> comboCardList = comboDictionary[k].OrderOfCards;
                    //コンボに必要なカードがあるか検索
                    for (int m = 1; m < comboCardList.Count; m++)
                    {
                        canDo = false;
                        for (int n = 0; n < handList.Count; n++)
                        {
                            ///対象が使用済みカードの場合飛ばす
                            if (useCard[n] == true)
                            {
                                continue;
                            }
                            if (comboCardList[m] == handList[n])
                            {
                                //コンボに必要なカード(m)と等しいコンボ要因(n)が見つかったら
                                canDo = true;
                            }
                        }
                        if (canDo == false)
                        {
                            break;
                        }
                    }
                    if (canDo == true)
                    {
                        compareComboList.Add(comboDictionary[k]);
                    }
                }
            }
            //ソートする
            if (compareComboList.Count > 0)
            {
                Debug.Log(" *** コンボ成立！ ***");
                List<ComboData> sortedComboList = compareComboList.OrderBy(x => x.score).ToList();
                _decidedCombo = sortedComboList[0];
            }
        }
        List<CardData> useCardList = new List<CardData>();
        if (_decidedCombo != null)
        {
            for (int i = 0; i < _decidedCombo.OrderOfCards.Count; i++)
            {
                useCardList.Add(_decidedCombo.OrderOfCards[i]);
                _hand.RemoveCard(_decidedCombo.OrderOfCards[i]);
            }

        }
        else
        {
            //もしコンボがないなら左端から使える分だけカードをプレイする
            int tempMp = _monster.CurrentMp;
            int i = 0;
            while (true)
            {
                if (handList[i].Cost < tempMp)
                {
                    useCardList.Add(handList[i]);
                    tempMp -= handList[i].Cost;
                    _hand.RemoveCard(handList[i]);

                }

                i++;

                if (i >= handList.Count || tempMp <= 0)
                {
                    break;
                }
            }

        }
        Debug.Log("使用カード枚数" + useCardList.Count);
        return useCardList;
    }
}

[System.Serializable]
public class ComboData
{
    public List<CardData> OrderOfCards;
    [Header("評価点")]
    public int score = 0;
}
