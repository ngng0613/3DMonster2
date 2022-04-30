using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeckComposition : MonoBehaviour
{
    public enum Mode
    {
        Composition,
        Release
    }

    [SerializeField] protected PartOfMonsterList _partOfMonsterListPrefab;
    [SerializeField] protected GameObject _lane;
    [SerializeField] protected float _spinSpeed = 3.0f;

    [SerializeField] protected GameObject[] _partySlotPosition;
    [SerializeField] protected GameObject[] _subSlotPosition;
    [SerializeField] protected PartOfMonsterList[] _parts;

    [SerializeField] protected CardObject[] _cardList1 = new CardObject[5];
    [SerializeField] protected CardObject[] _cardList2 = new CardObject[5];
    [SerializeField] protected CardObject[] _cardList3 = new CardObject[5];

    public PartOfMonsterList[] SetParts = new PartOfMonsterList[3];

    public delegate void AfterFuncDelegate();
    public AfterFuncDelegate AfterFunc;

    /// <summary>
    /// 起動処理
    /// </summary>
    public virtual void Activate()
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
    public virtual void Deactivate()
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
        AfterFunc.Invoke();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// モンスターがパーティースロットにセットされた際にコルーチンを呼ぶ処理
    /// </summary>
    /// <param name="monster">セットしたモンスター</param>
    /// <param name="slotId">セットされたスロット番号</param>
    /// <param name="part">セットされたモンスターリスト内のオブジェクト</param>
    public virtual void SetMonsterSlot(MonsterBase monster, int slotId, PartOfMonsterList part)
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
        _parts[slotId - 1].Monster.InParty = true;
        StartCoroutine(SetMonsterAnimation(monster, slotId));

    }

    /// <summary>
    /// セットされたモンスターのデッキ内容を表示する処理
    /// </summary>
    /// <param name="monster">セットしたモンスター</param>
    /// <param name="slotId">セットされたパーティーのスロット番号</param>
    /// <returns></returns>
    public virtual IEnumerator SetMonsterAnimation(MonsterBase monster, int slotId)
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
    /// カードを回転させながら中身を更新する
    /// </summary>
    /// <param name="obj">対象カードオブジェクト</param>
    /// <param name="data">更新するカードデータ<param>
    /// <returns></returns>
    public virtual IEnumerator CardSpin(CardObject obj, CardData data)
    {
        while (true)
        {
            obj.transform.localEulerAngles += new Vector3(0, _spinSpeed * Time.deltaTime, 0);
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
            obj.transform.localEulerAngles -= new Vector3(0, _spinSpeed * Time.deltaTime, 0);
            if (obj.transform.localEulerAngles.y >= 180)
            {
                obj.transform.localEulerAngles = Vector3.zero;
                break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// デッキ編成の決定
    /// </summary>
    public virtual void Decide()
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
    public virtual void ReleaseFromParty(PartOfMonsterList part)
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
