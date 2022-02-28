using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    [SerializeField] Vector3 _firstPlayerPos;
    [SerializeField] int _monsterMaxCount = 6;
    [SerializeField] List<MonsterBase> _monsterList;
    [SerializeField] ShopManager _shopManager;
    [SerializeField] StageData _stageDataDefault;
    [SerializeField] Fade _fade;

    void Awake()
    {
        if (GameManager.Instance.IsFirst == true)
        {
            GameManager.Instance.IsFirst = false;
            GameManager.Instance.PlayeraPos = _firstPlayerPos;

        }
        if (GameManager.Instance.MonsterList.Count <= 0)
        {
            GameManager.Instance.MonsterList = _monsterList;
            for (int i = 0; i < 3; i++)
            {
                if (i >= _monsterList.Count)
                {
                    continue;
                }
                _monsterList[i].InParty = true;
            }
            GameManager.Instance.MonsterParty.Add(_monsterList[0]);
            GameManager.Instance.MonsterParty.Add(_monsterList[1]);
            GameManager.Instance.MonsterParty.Add(_monsterList[2]);
            GameManager.Instance.MonsterMaxCount = _monsterMaxCount;

            Debug.LogWarning("モンスターリストを初期化しました");

        }
        else
        {
            foreach (var item in GameManager.Instance.MonsterList)
            {
                Debug.Log(item.NickName);
            }
            _monsterList = GameManager.Instance.MonsterList;
        }
        if (_shopManager != null)
        {
            _shopManager.Setup();
        }
        if (GameManager.Instance.NextBattleStage == null)
        {
            GameManager.Instance.NextBattleStage = _stageDataDefault;
        }
    }

    public void Start()
    {
        _fade.FadeIn();
    }
}
