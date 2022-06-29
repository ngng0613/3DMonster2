using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    static GameManager s_instance = new GameManager();
    public static GameManager Instance => s_instance;

    public bool IsFirst = true;

    /// <summary>
    /// 現在連れているモンスターのリスト
    /// </summary>
    public List<MonsterBase> MonsterList = new List<MonsterBase>();
    /// <summary>
    /// 現在戦闘要員として編成中のモンスターのリスト
    /// </summary>
    public List<MonsterBase> MonsterParty = new List<MonsterBase>();
    /// <summary>
    /// 現在戦闘要員として編成中のモンスターのIDのリスト
    /// </summary>
    public List<int> MonsterPartyIdList = new List<int>() { 0, 1, 2 };
    /// <summary>
    /// マップ上のプレイヤーの位置情報
    /// </summary>
    public Vector2 PlayeraPos;
    /// <summary>
    /// プレイヤーの名前
    /// </summary>
    public string PlayerName = "Player";
    /// <summary>
    /// プレイヤーの最大HP
    /// </summary>
    public int PlayerMaxHp = 15;
    public int PlayerCurrentHp = 15;
    public int PlayerMaxMp = 3;
    public int PlayerCurrentMp = 0;
    public List<StatusEffectBase> PlayerStatusEffectList = new List<StatusEffectBase>();
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
