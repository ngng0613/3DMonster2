﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TitleManager : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;
    [SerializeField] Fade _fade;
    [SerializeField] string _fieldSceneName;
    [SerializeField] Vector3 _firstPlayerPos;
    [SerializeField] int _playerHp;


    [SerializeField] List<MonsterBase> _monsterList;
    [SerializeField] int _monsterMaxCount;

    bool _Start = false;
    public void Start()
    {
        //フレーム抜け回避のため（これを書かないと、マップで当たり判定ぬけする）
        Application.targetFrameRate = 60;

        GameManager.Instance.PlayeraPos = _firstPlayerPos;
        GameManager.Instance.PlayerHp = _playerHp;
        GameManager.Instance.PlayerMonster.MaxHp = _playerHp;

        GameManager.Instance.FriendCount = 0;
        GameManager.Instance.CaptureCount = 0;
        GameManager.Instance.BattleCount = 0;
        GameManager.Instance.MonsterPartyIdList = new List<int>();
    }

    /// <summary>
    /// シーン移動と同時にゲーム初期化処理もおこなう
    /// </summary>
    public void ChangeScene()
    {
        //すでにこの関数が呼ばれているなら、処理を停止する
        if (_Start == true)
        {
            return;
        }
        _Start = true;
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
        GameManager.Instance.MonsterMaxCount = _monsterMaxCount;
        GameManager.Instance.MonsterPartyIdList = new List<int>() { 0, 1, 2 };
        GameManager.Instance.PlayerMonster.CurrentHp = GameManager.Instance.PlayerMonster.MaxHp;
        GameManager.Instance.PlayerMonster.MaxMp = GameManager.Instance.MonsterParty.Sum(x => x.MaxMp);
        Debug.LogWarning("モンスターリストを初期化しました");

        _fade.AfterFunction = GoToField;
        _fade.FadeOut();
    }

    public void GoToField()
    {
        SceneManager.LoadScene(_fieldSceneName);
    }
}
