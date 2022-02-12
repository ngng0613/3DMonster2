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

    public void ChangeScene(string sceneName)
    {
        SceneManager.sceneLoaded += A;
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void A(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("aaa");
    }

}
