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
    public Vector2 PlayeraPos;
    public MonsterBase PlayerMonster = new MonsterBase("Player", 100, 5);
    public int PlayerHp = 100;
    public int MonsterMaxCount = 6;

    public StageData NextBattleStage;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }


}
