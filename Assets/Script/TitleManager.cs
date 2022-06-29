using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// 音を鳴らすために設定する項目
    /// </summary>
    [SerializeField] SoundManager _soundManager;
    /// <summary>
    /// 画面のフェード処理をするために設定する項目
    /// </summary>
    [SerializeField] Fade _fade;
    /// <summary>
    /// タイトルからシーン移動する際の、フィールドシーンのシーン名
    /// </summary>
    [SerializeField] string _fieldSceneName;
    /// <summary>
    /// シーン移動後のプレイヤーの初期位置
    /// </summary>
    [SerializeField] Vector3 _firstPlayerPos;
    /// <summary>
    /// プレイヤーのHP設定
    /// </summary>
    [SerializeField] int _playerHp;
    /// <summary>
    /// モンスターの初期設定
    /// </summary>
    [SerializeField] List<MonsterBase> _monsterList;
    /// <summary>
    /// モンスターを持てる最大所持数
    /// </summary>
    [SerializeField] int _monsterMaxCount;
    /// <summary>
    /// プレイヤーの名前
    /// </summary>
    [SerializeField] string _playerName;


    bool _Start = false;
    public void Start()
    {
        //フレーム抜け回避のため（これを書かないと、マップで当たり判定ぬけする）
        Application.targetFrameRate = 60;

        GameManager.Instance.PlayeraPos = _firstPlayerPos;
        GameManager.Instance.PlayerMaxHp = _playerHp;
        GameManager.Instance.PlayerCurrentHp = _playerHp;
        GameManager.Instance.PlayerStatusEffectList = new List<StatusEffectBase>();

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
        GameManager.Instance.PlayerName = "Player";
        GameManager.Instance.MonsterMaxCount = _monsterMaxCount;
        GameManager.Instance.MonsterPartyIdList = new List<int>() { 0, 1, 2 };
        GameManager.Instance.PlayerCurrentHp = GameManager.Instance.PlayerMaxHp;
        GameManager.Instance.PlayerMaxMp = GameManager.Instance.MonsterParty.Sum(x => x.MaxMp);
        Debug.LogWarning("モンスターリストを初期化しました");

        _fade.AfterFunction = GoToField;
        _fade.FadeOut();
    }

    public void GoToField()
    {
        SceneManager.LoadScene(_fieldSceneName);
    }
}
