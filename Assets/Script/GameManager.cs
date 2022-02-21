using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    static GameManager s_instance = new GameManager();
    public static GameManager Instance => s_instance;

    public List<MonsterBase> MonsterList = new List<MonsterBase>();
    public List<MonsterBase> MonsterParty = new List<MonsterBase>();
    public Vector2 PlayeraPos;

    public StageData NextBattleStage;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }


}
