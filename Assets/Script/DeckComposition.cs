using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckComposition : MonoBehaviour
{
    [SerializeField] PartOfMonsterList _partOfMonsterListPrefab;
    [SerializeField] GameObject _lane;

    [SerializeField] GameObject _partsPosition;

    [SerializeField] CardObject[] _cardList1 = new CardObject[5];
    [SerializeField] CardObject[] _cardList2 = new CardObject[5];
    [SerializeField] CardObject[] _cardList3 = new CardObject[5];

    private void Start()
    {
        PartOfMonsterList part = Instantiate(_partOfMonsterListPrefab, _lane.transform);
        part.SetMonsterSlot = SetMonsterSlot;
    }

    public void SetMonsterSlot(MonsterBase monster, int slotId)
    {
        StartCoroutine(SetMonsterAnimation(monster, slotId));
        switch (slotId)
        {
            case 1:
                Debug.Log($"スロット1に{monster.MonsterName}をセットしました");
                break;

            case 2:
                Debug.Log($"スロット2に{monster.MonsterName}をセットしました");
                break;

            case 3:
                Debug.Log($"スロット3に{monster.MonsterName}をセットしました");
                break;

            default:
                break;
        }

    }

    public IEnumerator SetMonsterAnimation(MonsterBase monster, int slotId)
    {
        switch (slotId)
        {
            case 1:
                for (int i = 0; i < _cardList1.Length; i++)
                {
                    yield return CardSpin(_cardList1[i], monster.CardDatas[i]);

                }
                break;

            case 2:
                for (int i = 0; i < _cardList1.Length; i++)
                {
                    yield return CardSpin(_cardList2[i], monster.CardDatas[i]);

                }
                break;

            case 3:
                for (int i = 0; i < _cardList1.Length; i++)
                {
                    yield return CardSpin(_cardList3[i], monster.CardDatas[i]);

                }
                break;

            default:
                break;
        }



    }


    public IEnumerator CardSpin(CardObject obj, CardData data)
    {
        while (true)
        {
            obj.transform.localEulerAngles += new Vector3(0, 3, 0);
            if (obj.transform.localEulerAngles.y >= 90)
            {
                obj.transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            }

            yield return null;
        }
        obj.Data = data;
        obj.UpdateText();
        while (true)
        {
            obj.transform.localEulerAngles -= new Vector3(0, 3, 0);
            if (obj.transform.localEulerAngles.y >= 180)
            {
                obj.transform.localEulerAngles = Vector3.zero;
                break;
            }
            yield return null;
        }
    }

}
