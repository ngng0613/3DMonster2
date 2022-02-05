using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeckComposition : MonoBehaviour
{
    [SerializeField] PartOfMonsterList _partOfMonsterListPrefab;
    [SerializeField] GameObject _lane;
    [SerializeField] float _spinSpeed = 3.0f;

    [SerializeField] GameObject[] _partsPosition;

    [SerializeField] CardObject[] _cardList1 = new CardObject[5];
    [SerializeField] CardObject[] _cardList2 = new CardObject[5];
    [SerializeField] CardObject[] _cardList3 = new CardObject[5];

    public PartOfMonsterList[] SetParts = new PartOfMonsterList[3];

    public void Start()
    {

        for (int i = 0; i < _partsPosition.Length; i++)
        {
            PartOfMonsterList part = Instantiate(_partOfMonsterListPrefab, _lane.transform);
            part.transform.position = _partsPosition[i].transform.position;
            part.BasePos = _partsPosition[i].transform.position;
            part.Monster = GameManager.Instance.MonsterList[i];
            part.DisplayUpdate();
            part.SetMonsterSlot = SetMonsterSlot;
            part.Release = ReleaseMonster;
        }

    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void SetMonsterSlot(MonsterBase monster, int slotId, PartOfMonsterList part)
    {
        if (SetParts[slotId - 1] != null)
        {
            SetParts[slotId - 1].BackToBasePos();
        }
        SetParts[slotId - 1] = part;
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
        StartCoroutine(SetMonsterAnimation(monster, slotId));


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
            obj.transform.localEulerAngles += new Vector3(0, _spinSpeed, 0);
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
            obj.transform.localEulerAngles -= new Vector3(0, _spinSpeed, 0);
            if (obj.transform.localEulerAngles.y >= 180)
            {
                obj.transform.localEulerAngles = Vector3.zero;
                break;
            }
            yield return null;
        }
    }

    public void Decide()
    {
        List<MonsterBase> monsterList = new List<MonsterBase>();
        for (int i = 0; i < SetParts.Length; i++)
        {
            if (SetParts[i] != null)
            {
                monsterList.Add(SetParts[i].Monster);
            }
            else
            {
                Error("モンスターが3体セットされていません");
                break;
            }

        }
        if (monsterList.Count >= 3)
        {
            GameManager.Instance.MonsterList = monsterList;
        }
    }

    public void ReleaseMonster(PartOfMonsterList part)
    {
        for (int i = 0; i < SetParts.Length; i++)
        {
            if (SetParts[i] == part)
            {
                SetParts[i] = null;
            }

        }
    }

    public void Error(string text)
    {
        Debug.Log(text);
    }

    public void End()
    {
        this.gameObject.SetActive(false);
    }
}
