﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    static GameManager s_instance = new GameManager();
    public static GameManager Instance => s_instance;

    public bool IsFirst = true;

    public List<MonsterBase> MonsterList = new List<MonsterBase>();
    public List<MonsterBase> MonsterParty = new List<MonsterBase>();
    public List<int> MonsterPartyIdList = new List<int>() { 0,1,2};
    public Vector2 PlayeraPos;
    public MonsterBase PlayerMonster = new MonsterBase("Player", 50, 5);
    public int PlayerHp = 50;
    public int MonsterMaxCount = 6;
    public int BattleCount = 0;
    public int CaptureCount = 0;
    public int FriendCount = 0;

    public StageData NextBattleStage;
    public string TitleName;
    public string FieldMapName;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }


}
