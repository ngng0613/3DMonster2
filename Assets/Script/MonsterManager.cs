using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 所持しているモンスターの管理
/// </summary>
public class MonsterManager : MonoBehaviour
{
    /// <summary>
    /// 全種類のモンスターリスト
    /// </summary>
    [SerializeField] List<MonsterBase> _allMonsterList;

    /// <summary>
    /// 所持しているモンスターのリスト
    /// </summary>
    [SerializeField] List<MonsterBase> _possessionMonsterList = new List<MonsterBase>();

    public static List<MonsterBase> PartyMonsterList;

    public int NumberOfPossessionMonster = 1;
    public MonsterBase MonsterNullData;


    /// <summary>
    /// 現在存在している
    /// </summary>
    [SerializeField] List<float> _individualIDList = new List<float>();


    [System.Serializable]
    public class MonsterSaveData
    {
        //ID
        public int Id;

        //個別ID
        public float IndividualID;

        //名前
        public string MonsterName;
        //ニックネーム
        public string NickName;

        //イメージ
        public Sprite Image;

        //属性
        public string Elements;

        //最大HP
        public int MaxHp;
        //現在のHP
        public int CurrentHp;
        //最大MP
        public int MaxMp;
        //現在のMP
        public int CurrentMp;

        //攻撃力
        public int Attack;
        //防御力
        public int Defence;
        //特殊攻撃力
        public int SpAttack;
        //特殊防御力
        public int SpDefence;
        //素早さ
        public int Speed;
        //運
        public int Luck;
        //命中率
        public int Hit;
        //回避率
        public int Avoidance;

        //ベースのPrefab
        public GameObject MyPrefab;


        //レベル
        public int Level;
        //経験値
        public int Exp;

        //次のレベルまでの経験値
        public int ExpToNextLevel;

        public List<SkillBase> SkillList;

    }

    [SerializeField] bool _isFirst = false;


    private void Start()
    {
        if (_isFirst)
        {
            MonsterManager.PartyMonsterList = _possessionMonsterList;
        }
        //gameManager = GetComponent<GameManager>();
    }

    public void SetDebugParty()
    {
        MonsterManager.PartyMonsterList = _possessionMonsterList;
    }

    public void Setup()
    {
        NumberOfPossessionMonster = 0;
        foreach (var item in _possessionMonsterList)
        {
            if (item != null)
            {
                NumberOfPossessionMonster++;
            }
        }
        PartyMonsterList = _possessionMonsterList;

    }



    public void RemoveMonster(int i)
    {
        _possessionMonsterList.RemoveAt(i);
        NumberOfPossessionMonster--;
    }
    int saveNumber = 0;
    string savedWord = "SAVEDATA:";

    public int GetPossessionMonsterID(int number)
    {
        int id = 0;
        return id;
    }




    /// <summary>
    /// 受け取ったIDから、対象となるモンスターのプレハブを返す。
    /// </summary>
    /// <param name="id">取得したいモンスターのID</param>
    /// <returns>モンスターのオブジェクト</returns>
    public GameObject GetMonsterPrefab(int id)
    {
        return _allMonsterList[id].gameObject;
    }
    /// <summary>
    /// モンスター情報を仮セーブ
    /// </summary>
    public void TemporarilyPlayerMonstersSave()
    {
        for (int i = 0; i < _possessionMonsterList.Count; i++)
        {
            if (_possessionMonsterList[i] == null)
            {
                return;
            }


            MonsterSaveData saveData = new MonsterSaveData();

            saveData.Id = _possessionMonsterList[i].GetId();
            saveData.NickName = _possessionMonsterList[i].GetNickname();
            saveData.MaxHp = _possessionMonsterList[i].GetMaxHPValue();
            saveData.MaxMp = _possessionMonsterList[i].GetMaxMPValue();
            saveData.CurrentHp = _possessionMonsterList[i].GetCurrentHPValue();
            saveData.CurrentMp = _possessionMonsterList[i].GetCurrentMPValue();
            saveData.Attack = _possessionMonsterList[i].GetAttackValue();
            saveData.Defence = _possessionMonsterList[i].GetDefenceValue();
            saveData.SpAttack = _possessionMonsterList[i].GetSpAttackValue();
            saveData.SpDefence = _possessionMonsterList[i].GetSpDefenceValue();
            saveData.Speed = _possessionMonsterList[i].GetSpeedValue();
            saveData.Exp = _possessionMonsterList[i].GetEXPValue();
            saveData.Level = _possessionMonsterList[i].GetLevelValue();
            saveData.ExpToNextLevel = _possessionMonsterList[i].GetEXPToNextLevel();

            saveData.SkillList = _possessionMonsterList[i].GetSkillList();




            saveMonsterData_temp(saveData, i);

        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void TemporarilyPlayerMonstersLoad()
    {
        for (int i = 0; i < _possessionMonsterList.Count; i++)
        {
            if (_possessionMonsterList[i] == null)
            {
                return;
            }
            MonsterSaveData savedata = loadMonsterData_temp(i);

            _possessionMonsterList[i].SetNickName(savedata.NickName);
            _possessionMonsterList[i].SetID(savedata.Id);
            _possessionMonsterList[i].SetMaxHPValue(savedata.MaxHp);
            _possessionMonsterList[i].SetMaxMPValue(savedata.MaxMp);
            _possessionMonsterList[i].SetCurrentHPValue(savedata.CurrentHp);
            _possessionMonsterList[i].SetCurrentMPValue(savedata.CurrentMp);
            _possessionMonsterList[i].SetAttackValue(savedata.Attack);
            _possessionMonsterList[i].SetDefenceValue(savedata.Defence);
            _possessionMonsterList[i].SetSPAttackValue(savedata.SpAttack);
            _possessionMonsterList[i].SetSPDefenceValue(savedata.SpDefence);
            _possessionMonsterList[i].SetSpeedValue(savedata.Speed);

            _possessionMonsterList[i].SetLevelValue(savedata.Level);
            _possessionMonsterList[i].SetEXPValue(savedata.Exp);
            _possessionMonsterList[i].SetEXPToNextLevel(savedata.ExpToNextLevel);

            _possessionMonsterList[i].SetSkillList(savedata.SkillList);


        }
    }

    /// <summary>
    /// 仮保存されているMonsterBaseを呼び出し、ゲームオブジェクト内のMonsterBaseを上書きする
    /// </summary>
    /// <param name="friend">　上書きするモンスター（GameObject）</param>
    /// <param name="i"> パーティ内の読み込みたいモンスターの先頭からの番号</param>
    public void TemporarilyPlayerMonstersLoad(GameObject friend, int i)
    {
        MonsterSaveData savedata = loadMonsterData_temp(i);

        friend.GetComponent<MonsterBase>().SetNickName(savedata.NickName);
        friend.GetComponent<MonsterBase>().SetID(savedata.Id);
        friend.GetComponent<MonsterBase>().SetMaxHPValue(savedata.MaxHp);
        friend.GetComponent<MonsterBase>().SetMaxMPValue(savedata.MaxMp);

        friend.GetComponent<MonsterBase>().SetCurrentHPValue(savedata.MaxHp);
        friend.GetComponent<MonsterBase>().SetCurrentMPValue(savedata.MaxMp);

        friend.GetComponent<MonsterBase>().SetAttackValue(savedata.Attack);
        friend.GetComponent<MonsterBase>().SetDefenceValue(savedata.Defence);
        friend.GetComponent<MonsterBase>().SetSPAttackValue(savedata.SpAttack);
        friend.GetComponent<MonsterBase>().SetSPDefenceValue(savedata.SpDefence);
        friend.GetComponent<MonsterBase>().SetSpeedValue(savedata.Speed);

        friend.GetComponent<MonsterBase>().SetLevelValue(savedata.Level);
        friend.GetComponent<MonsterBase>().SetEXPValue(savedata.Exp);
        friend.GetComponent<MonsterBase>().SetEXPToNextLevel(savedata.ExpToNextLevel);

        _possessionMonsterList[i].SetSkillList(savedata.SkillList);



    }
    /// <summary>
    /// 指定したリストの位置に指定したモンスターを設定
    /// </summary>
    /// <param name="number">リスト番号</param>
    /// <param name="monster">モンスター</param>
    public void SetPossessionMonsterList(int number, GameObject monster)
    {
        _possessionMonsterList[number] = monster.GetComponent<MonsterBase>();
    }

    public void SetPossessionMonsterList(List<MonsterBase> monsterList)
    {
        _possessionMonsterList = monsterList;
        SetPartyMonsterList();
    }
    public void SetPartyMonsterList()
    {
        PartyMonsterList = _possessionMonsterList;
    }

    public List<MonsterBase> GetPossessionMonsterList()
    {
        return _possessionMonsterList;
    }

    public string GetSavedWord()
    {
        return savedWord;
    }

    public void Reset_individual_IDList()
    {
        _individualIDList = new List<float>();
    }

    public bool AddIndividualIDList(float individual_ID)
    {
        foreach (var item in _individualIDList)
        {
            if (item == individual_ID)
            {
                return false;
            }

        }
        return true;
    }

    /// <summary>
    /// モンスターデータをJSONにして保存
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="monster_Id"></param>
    public void saveMonsterData_temp(MonsterSaveData monster, int monster_Id)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(monster);

        writer = new StreamWriter(Application.dataPath + "/Temporary_saveMonsterData" + monster_Id + ".json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public MonsterSaveData loadMonsterData_temp(int monster_Id)
    {
        string datastr = "";
        StreamReader reader;

        reader = new StreamReader(Application.dataPath + "/Temporary_saveMonsterData" + monster_Id + ".json");
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<MonsterSaveData>(datastr);
    }


    /// <summary>
    /// 本格的なセーブ時にモンスターデータをJSONにして保存する
    /// ※これ単体では機能しない
    /// </summary>
    /// <param name="saveID">セーブ番号</param>
    /// <param name="monster"></param>
    /// <param name="monster_Id"></param>
    public void saveMonsterData(int saveID, MonsterSaveData monster, int monster_Id)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(monster);

        writer = new StreamWriter(Application.dataPath + "/SaveData/saveID" + saveID + "saveMonsterData" + monster_Id + ".json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// 受け取ったセーブデータ番号、モンスター番号をもとにJSONファイルを読み取り、モンスターデータとして返す
    /// </summary>
    /// <param name="loadID">ロード番号</param>
    /// <param name="monster_Id"></param>
    /// <returns></returns>
    public MonsterSaveData loadMonsterData(int loadID, int monster_Id)
    {
        string datastr = "";
        StreamReader reader;

        if (!System.IO.File.Exists(Application.dataPath + "/SaveData/saveID" + loadID + "saveMonsterData" + monster_Id + ".json"))
        {
            return null;
        }

        reader = new StreamReader(Application.dataPath + "/SaveData/saveID" + loadID + "saveMonsterData" + monster_Id + ".json");
        datastr = reader.ReadToEnd();
        reader.Close();

        if (datastr == "")
        {
            return null;
        }
        return JsonUtility.FromJson<MonsterSaveData>(datastr);
    }


    /// <summary>
    /// 本格的なセーブ
    /// </summary>
    /// <param name="id"></param>
    public void PlayerMonstersSave(int id)
    {
        for (int i = 0; i < _possessionMonsterList.Count; i++)
        {
            if (_possessionMonsterList[i] == null)
            {
                return;
            }


            MonsterSaveData saveData = new MonsterSaveData();

            saveData.Id = _possessionMonsterList[i].GetId();
            saveData.NickName = _possessionMonsterList[i].GetNickname();
            saveData.MaxHp = _possessionMonsterList[i].GetMaxHPValue();
            saveData.MaxMp = _possessionMonsterList[i].GetMaxMPValue();
            saveData.CurrentHp = _possessionMonsterList[i].GetCurrentHPValue();
            saveData.CurrentMp = _possessionMonsterList[i].GetCurrentMPValue();
            saveData.Attack = _possessionMonsterList[i].GetAttackValue();
            saveData.Defence = _possessionMonsterList[i].GetDefenceValue();
            saveData.SpAttack = _possessionMonsterList[i].GetSpAttackValue();
            saveData.SpDefence = _possessionMonsterList[i].GetSpDefenceValue();
            saveData.Speed = _possessionMonsterList[i].GetSpeedValue();
            saveData.Exp = _possessionMonsterList[i].GetEXPValue();
            saveData.Level = _possessionMonsterList[i].GetLevelValue();
            saveData.ExpToNextLevel = _possessionMonsterList[i].GetEXPToNextLevel();

            saveData.SkillList = _possessionMonsterList[i].GetSkillList();




            saveMonsterData(id, saveData, i);

        }

    }

    /// <summary>
    /// 本格的なロード
    /// </summary>
    /// <param name="id"></param>
    public void PlayerMonstersLoad(int id)
    {
        for (int i = 0; i < _possessionMonsterList.Count; i++)
        {
            MonsterSaveData savedata = loadMonsterData(id, i);
            if (savedata == null)
            {
                return;
            }

            _possessionMonsterList[i] = MonsterNullData;

            _possessionMonsterList[i].SetNickName(savedata.NickName);
            _possessionMonsterList[i].SetID(savedata.Id);
            _possessionMonsterList[i].SetMaxHPValue(savedata.MaxHp);
            _possessionMonsterList[i].SetMaxMPValue(savedata.MaxMp);
            _possessionMonsterList[i].SetCurrentHPValue(savedata.CurrentHp);
            _possessionMonsterList[i].SetCurrentMPValue(savedata.CurrentMp);
            _possessionMonsterList[i].SetAttackValue(savedata.Attack);
            _possessionMonsterList[i].SetDefenceValue(savedata.Defence);
            _possessionMonsterList[i].SetSPAttackValue(savedata.SpAttack);
            _possessionMonsterList[i].SetSPDefenceValue(savedata.SpDefence);
            _possessionMonsterList[i].SetSpeedValue(savedata.Speed);

            _possessionMonsterList[i].SetLevelValue(savedata.Level);
            _possessionMonsterList[i].SetEXPValue(savedata.Exp);
            _possessionMonsterList[i].SetEXPToNextLevel(savedata.ExpToNextLevel);

            _possessionMonsterList[i].SetSkillList(savedata.SkillList);


        }
    }

    public List<MonsterBase> GetAllMonsterList()
    {
        return _allMonsterList;
    }
}

