using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : DeckManagerBase
{
    /// <summary>
    /// 起動処理
    /// </summary>
    public override void Activate()
    {
        _parts = new PartOfMonsterList[6];

        this.gameObject.SetActive(true);

        int partyCount = 0;

        for (int i = 0; i < GameManager.Instance.MonsterList.Count; i++)
        {
            if (i >= _subSlotPosition.Length)
            {
                return;
            }
            if (GameManager.Instance.MonsterList[i] == null)
            {
                return;
            }
            PartOfMonsterList part = Instantiate(_partOfMonsterListPrefab, _lane.transform);
            part.transform.position = _subSlotPosition[i].transform.position;
            part.BasePos = _subSlotPosition[i].transform.position;
            part.Monster = GameManager.Instance.MonsterList[i];
            part.DisplayUpdate();
            part.SetMonsterSlot = SetMonsterSlot;
            part.Release = ReleaseFromParty;
            _parts[i] = part;
            part.Id = i;

            //パーティーに参加済みのモンスターはあらかじめパーティースロットにセットしておく
            if (part.Monster.InParty == true && partyCount < _partySlotPosition.Length)
            {
                part.transform.position = _partySlotPosition[partyCount].transform.position;
                SetMonsterSlot(part.Monster, partyCount + 1, part);
                partyCount++;
            }

        }

    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Deactivate()
    {
        for (int i = 0; i < _parts.Length; i++)
        {
            if (_parts[i] == null)
            {
                continue;
            }
            Destroy(_parts[i].gameObject);
            _parts[i] = null;
        }
        if (AfterFunc != null)
        {
            AfterFunc.Invoke();
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// モンスターがパーティースロットにセットされた際にコルーチンを呼ぶ処理
    /// </summary>
    /// <param name="monster">セットしたモンスター</param>
    /// <param name="slotId">セットされたスロット番号</param>
    /// <param name="part">セットされたモンスターリスト内のオブジェクト</param>
    public override void SetMonsterSlot(MonsterBase monster, int slotId, PartOfMonsterList part)
    {
        if (SetParts[slotId - 1] != null)
        {
            SetParts[slotId - 1].BackToBasePos();
        }
        SetParts[slotId - 1] = part;
        switch (slotId)
        {
            case 1:

                Debug.Log($"{monster.MonsterName}をセットしました");
                break;


            default:
                break;
        }
        _parts[slotId - 1].Monster.InParty = true;
        StartCoroutine(SetMonsterAnimation(monster, slotId));

    }

    /// <summary>
    /// セットされたモンスターのデッキ内容を表示する処理
    /// </summary>
    /// <param name="monster">セットしたモンスター</param>
    /// <param name="slotId">セットされたパーティーのスロット番号</param>
    /// <returns></returns>
    public override IEnumerator SetMonsterAnimation(MonsterBase monster, int slotId)
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


    /// <summary>
    /// デッキ編成の決定
    /// </summary>
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
                Debug.Log("モンスターが3体セットされていません");
                break;
            }

        }
        if (monsterList.Count >= 3)
        {
            GameManager.Instance.MonsterParty = monsterList;
        }
    }

    /// <summary>
    /// パーティーからモンスターが外れた際の処理
    /// </summary>
    /// <param name="part"></param>
    public override void ReleaseFromParty(PartOfMonsterList part)
    {
        for (int i = 0; i < SetParts.Length; i++)
        {
            if (SetParts[i] == part)
            {
                part.Monster.InParty = false;
                SetParts[i] = null;
            }
        }
    }
}

