﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetupManager : MonoBehaviour
{
    [SerializeField] Vector3 _firstPlayerPos;
    [SerializeField] int _monsterMaxCount;
    [SerializeField] List<MonsterBase> _monsterList;
    [SerializeField] ShopManager _shopManager;
    [SerializeField] StageData _stageDataDefault;
    [SerializeField] Fade _fade;
    [SerializeField] string _titleSceneName;

    void Awake()
    {
        if (GameManager.Instance.IsFirst == true)
        {
            GameManager.Instance.IsFirst = false;
            GameManager.Instance.PlayeraPos = _firstPlayerPos;
            GameManager.Instance.MonsterList = new List<MonsterBase>();
        }
        Debug.Log(GameManager.Instance.MonsterList.Count);
        if (GameManager.Instance.MonsterList.Count <= 0)
        {
            GameManager.Instance.MonsterList = _monsterList;
            foreach (var item in GameManager.Instance.MonsterList)
            {
                item.InParty = false;
            }
            for (int i = 0; i < 3; i++)
            {
                if (i >= _monsterList.Count)
                {
                    continue;
                }
                _monsterList[i].InParty = true;
            }
            GameManager.Instance.MonsterParty = new List<MonsterBase>();
            GameManager.Instance.MonsterParty.Add(_monsterList[0]);
            GameManager.Instance.MonsterParty.Add(_monsterList[1]);
            GameManager.Instance.MonsterParty.Add(_monsterList[2]);
            GameManager.Instance.MonsterMaxCount = _monsterMaxCount;
            GameManager.Instance.TitleName = _titleSceneName;
            int mp = 0;
            for (int i = 0; i < GameManager.Instance.MonsterParty.Count; i++)
            {
                mp += GameManager.Instance.MonsterParty[i].MaxMp;
            }
            GameManager.Instance.PlayerMaxMp = mp;
            GameManager.Instance.PlayerCurrentMp = mp;
            Debug.LogWarning("モンスターリストを初期化しました");

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
        if (_fade != null)
        {
            _fade.FadeIn();
        }

    }

    public void Update()
    {
    }
}
