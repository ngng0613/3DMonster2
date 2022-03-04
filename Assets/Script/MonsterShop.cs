using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterShop : DeckManagerBase
{
    [SerializeField] GameObject _confirmWindow;
    [SerializeField] MessageUi _dontSet;
    int _beforeMonsterCount = 0;
    int _afterMonsterCount = 0;

    /// <summary>
    /// 起動処理
    /// </summary>
    public override void Activate()
    {
        int _beforeMonsterCount = GameManager.Instance.MonsterList.Count;
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
            part.Release = ReleaseFromParty;
            part.SetMonsterSlot = SetMonsterSlot;
            _parts[i] = part;
            part.Id = i;

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
        for (int i = 0; i < GameManager.Instance.MonsterPartyIdList.Count; i++)
        {
            if (part.Id == GameManager.Instance.MonsterPartyIdList[i])
            {
                part.BackToBasePos();
                _dontSet.Activate();
                return;
            }
        }
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
        //_parts[slotId - 1].Monster.InParty = true;
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
    /// カードを回転させながら中身を更新する
    /// </summary>
    /// <param name="obj">対象カードオブジェクト</param>
    /// <param name="data">更新するカードデータ<param>
    /// <returns></returns>
    public override IEnumerator CardSpin(CardObject obj, CardData data)
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
                SetParts[i] = null;
            }
        }
    }

    /// <summary>
    /// モンスターを所持モンスターリストから外す処理
    /// </summary>
    public void ReleaseFromMonsterList()
    {
        MonsterBase[] tempList = GameManager.Instance.MonsterList.ToArray();
        for (int i = 0; i < SetParts.Length; i++)
        {
            if (SetParts[i] == null)
            {
                continue;
            }
            int id = SetParts[i].Id;

            for (int k = 0; k < GameManager.Instance.MonsterPartyIdList.Count; k++)
            {
                if (id < GameManager.Instance.MonsterPartyIdList[k])
                {
                    Debug.Log(100);
                    GameManager.Instance.MonsterPartyIdList[k] -= 1;
                }
                else
                {
                    Debug.Log(10);
                }
            }

            Destroy(SetParts[i].gameObject);
            tempList[id] = null;
        }
        GameManager.Instance.MonsterList = tempList.ToList();
        GameManager.Instance.MonsterList.RemoveAll(item => item == null);

        for (int i = 0; i < GameManager.Instance.MonsterList.Count; i++)
        {
            Debug.Log(GameManager.Instance.MonsterList[i].NickName);
        }
        CloseConfirmWindow();
        Deactivate();
        Activate();
    }

    /// <summary>
    /// 確認画面の表示
    /// </summary>
    public void DisplayConfirmWindow()
    {
        _confirmWindow.SetActive(true);
    }

    public void CloseConfirmWindow()
    {
        _confirmWindow.SetActive(false);
    }

}
