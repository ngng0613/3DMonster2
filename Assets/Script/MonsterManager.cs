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

    public List<MonsterBase> GetAllMonsterList()
    {
        return _allMonsterList;
    }
}

