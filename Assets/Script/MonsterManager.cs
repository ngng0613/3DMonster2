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
        public int Id;

        public string MonsterName;

        public string NickName;

        public Sprite Image;

        public int MaxHp;
        public int CurrentHp;
        public int MaxMp;
        public int CurrentMp;

        public GameObject MyPrefab;

        public int Level;
        public int Exp;

        public int ExpToNextLevel;

    }

    [SerializeField] bool _isFirst = false;


    private void Start()
    {
        if (_isFirst)
        {
            MonsterManager.PartyMonsterList = _possessionMonsterList;
        }
    }

    public void SetDebugParty()
    {
        GameManager.Instance.MonsterParty = _possessionMonsterList;
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

