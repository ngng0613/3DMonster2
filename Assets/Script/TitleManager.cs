﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;

    [SerializeField] Fade _fade;
    [SerializeField] string _fieldSceneName;
    [SerializeField] Vector3 _firstPlayerPos;


    [SerializeField] List<MonsterBase> _monsterList;
    [SerializeField] int _monsterMaxCount;

    public void Start()
    {
        GameManager.Instance.PlayeraPos = _firstPlayerPos;
    }

    public void ChangeScene()
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
        GameManager.Instance.MonsterParty.Add(_monsterList[0]);
        GameManager.Instance.MonsterParty.Add(_monsterList[1]);
        GameManager.Instance.MonsterParty.Add(_monsterList[2]);
        GameManager.Instance.MonsterMaxCount = _monsterMaxCount;
        GameManager.Instance.MonsterPartyIdList = new List<int>() { 0, 1, 2 };
        Debug.LogWarning("モンスターリストを初期化しました");


        _fade.AfterFunction = GoToField;
        _fade.FadeOut();
    }

    public void GoToField()
    {
        SceneManager.LoadScene(_fieldSceneName);
    }
}
