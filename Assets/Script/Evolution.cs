using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Evolution : DeckManagerBase
{
    [SerializeField] GameObject _confirmWindow;
    [SerializeField] PartOfMonsterList _evoMonsterObject;
    [SerializeField] GameObject _evoErrorObject;
    [SerializeField] GameObject _evoCanvas;
    [SerializeField] PartOfMonsterList _evoMonsterObjectInDirection;
    [SerializeField] MessageUi _warning2;
    [SerializeField] float _directionSpeed = 100;
    /// <summary>
    /// 進化演出後のWait時間
    /// </summary>
    [SerializeField] float _evoEndTime;
    float _tempDirectionSpeed;
    


    MonsterBase _evolvedMonster;

    /// <summary>
    /// 起動処理
    /// </summary>
    public override void Activate()
    {
        _parts = new PartOfMonsterList[6];

        this.gameObject.SetActive(true);

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
        }

    }

    public void StartEvolution()
    {
        StartCoroutine(EvolutionCoroutine());
    }

    IEnumerator EvolutionCoroutine()
    {
        _evoMonsterObjectInDirection.Monster = SetParts[0].Monster;
        _evoMonsterObjectInDirection.DisplayUpdate();
        _evoCanvas.SetActive(true);
        _tempDirectionSpeed = _directionSpeed;
        CloseConfirmWindow();
        for (int i = 0; i < 20; i++)
        {
            while (true)
            {
                _evoMonsterObjectInDirection.transform.localEulerAngles += new Vector3(0, _tempDirectionSpeed * Time.deltaTime, 0);
                if (_evoMonsterObjectInDirection.transform.localEulerAngles.y >= 90)
                {
                    break;
                }
                yield return null;
            }
            while (true)
            {
                _evoMonsterObjectInDirection.transform.localEulerAngles -= new Vector3(0, _tempDirectionSpeed * Time.deltaTime, 0);
                if (_evoMonsterObjectInDirection.transform.localEulerAngles.y >= 180)
                {
                    break;
                }
                yield return null;

            }
            _tempDirectionSpeed *= 1.5f;
        }
        _evoMonsterObjectInDirection.transform.localEulerAngles = Vector3.zero;
        _evoMonsterObjectInDirection.Monster = _evolvedMonster;
        _evoMonsterObjectInDirection.DisplayUpdate();
        GameManager.Instance.MonsterList[SetParts[0].Id] = _evolvedMonster;
        yield return new WaitForSeconds(_evoEndTime);
        Deactivate();

    }

    /// <summary>
    /// 確認画面の表示
    /// </summary>
    public void DisplayConfirmWindow()
    {
        if (_evoMonsterObject.isActiveAndEnabled == false)
        {
            _warning2.Activate();
            return;
        }
        _confirmWindow.SetActive(true);
    }

    public void CloseConfirmWindow()
    {
        _confirmWindow.SetActive(false);
    }

    public override void SetMonsterSlot(MonsterBase monster, int slotId, PartOfMonsterList part)
    {
        base.SetMonsterSlot(monster, slotId, part);
        if (monster.EvolutionyMonster != null)
        {
            _evoMonsterObject.Monster = monster.EvolutionyMonster;
            _evolvedMonster = monster.EvolutionyMonster;
            _evoMonsterObject.gameObject.SetActive(true);
            _evoMonsterObject.DisplayUpdate();
            StartCoroutine(SetMonsterAnimation(monster.EvolutionyMonster, 3));
        }
        else
        {
            _evoErrorObject.SetActive(true);
        }
    }

    public override void ReleaseFromParty(PartOfMonsterList part)
    {
        base.ReleaseFromParty(part);
        if (part.Monster.EvolutionyMonster != null)
        {
            _evoMonsterObject.gameObject.SetActive(false);
        }
        else
        {
            _evoErrorObject.SetActive(false);
        }
    }
}
