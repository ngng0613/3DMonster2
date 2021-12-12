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
    [SerializeField] List<MonsterBase> allMonsterList;

    /// <summary>
    /// 所持しているモンスターのリスト
    /// </summary>
    [SerializeField] List<MonsterBase> possessionMonsterList = new List<MonsterBase>();

    public static List<MonsterBase> partyMonsterList;

    public int numberOfPossessionMonster = 1;
    public MonsterBase monsterNullData;


    /// <summary>
    /// 現在存在している
    /// </summary>
    [SerializeField] List<float> individual_IDList = new List<float>();


    [System.Serializable]
    public class MonsterSaveData
    {
        //ID
        public int id;

        //個別ID
        public float individual_ID;

        //名前
        public string monsterName;
        //ニックネーム
        public string nickName;

        //イメージ
        public Sprite image;

        //属性
        public string elements;

        //最大HP
        public int maxHp;
        //現在のHP
        public int currentHp;
        //最大MP
        public int maxMp;
        //現在のMP
        public int currentMp;

        //攻撃力
        public int attack;
        //防御力
        public int defence;
        //特殊攻撃力
        public int spAttack;
        //特殊防御力
        public int spDefence;
        //素早さ
        public int speed;
        //運
        public int luck;
        //命中率
        public int hit;
        //回避率
        public int avoidance;

        //ベースのPrefab
        public GameObject myPrefab;


        //レベル
        public int level;
        //経験値
        public int exp;

        //次のレベルまでの経験値
        public int expToNextLevel;

        public List<SkillBase> skillList;

    }

    [SerializeField] bool isFirst = false;


    private void Start()
    {
        if (isFirst)
        {
            MonsterManager.partyMonsterList = possessionMonsterList;
        }
        //gameManager = GetComponent<GameManager>();
    }

    public void SetDebugParty()
    {
        MonsterManager.partyMonsterList = possessionMonsterList;
    }

    public void Setup()
    {
        numberOfPossessionMonster = 0;
        foreach (var item in possessionMonsterList)
        {
            if (item != null)
            {
                numberOfPossessionMonster++;
            }
        }
        partyMonsterList = possessionMonsterList;

    }



    public void RemoveMonster(int i)
    {
        possessionMonsterList.RemoveAt(i);
        numberOfPossessionMonster--;
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
        return allMonsterList[id].gameObject;
    }
    /// <summary>
    /// モンスター情報を仮セーブ
    /// </summary>
    public void TemporarilyPlayerMonstersSave()
    {
        for (int i = 0; i < possessionMonsterList.Count; i++)
        {
            if (possessionMonsterList[i] == null)
            {
                return;
            }


            MonsterSaveData saveData = new MonsterSaveData();

            saveData.id = possessionMonsterList[i].GetId();
            saveData.nickName = possessionMonsterList[i].GetNickname();
            saveData.maxHp = possessionMonsterList[i].GetMaxHPValue();
            saveData.maxMp = possessionMonsterList[i].GetMaxMPValue();
            saveData.currentHp = possessionMonsterList[i].GetCurrentHPValue();
            saveData.currentMp = possessionMonsterList[i].GetCurrentMPValue();
            saveData.attack = possessionMonsterList[i].GetAttackValue();
            saveData.defence = possessionMonsterList[i].GetDefenceValue();
            saveData.spAttack = possessionMonsterList[i].GetSpAttackValue();
            saveData.spDefence = possessionMonsterList[i].GetSpDefenceValue();
            saveData.speed = possessionMonsterList[i].GetSpeedValue();
            saveData.exp = possessionMonsterList[i].GetEXPValue();
            saveData.level = possessionMonsterList[i].GetLevelValue();
            saveData.expToNextLevel = possessionMonsterList[i].GetEXPToNextLevel();

            saveData.skillList = possessionMonsterList[i].GetSkillList();




            saveMonsterData_temp(saveData, i);

        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void TemporarilyPlayerMonstersLoad()
    {
        for (int i = 0; i < possessionMonsterList.Count; i++)
        {
            if (possessionMonsterList[i] == null)
            {
                return;
            }
            MonsterSaveData savedata = loadMonsterData_temp(i);

            possessionMonsterList[i].SetNickName(savedata.nickName);
            possessionMonsterList[i].SetID(savedata.id);
            possessionMonsterList[i].SetMaxHPValue(savedata.maxHp);
            possessionMonsterList[i].SetMaxMPValue(savedata.maxMp);
            possessionMonsterList[i].SetCurrentHPValue(savedata.currentHp);
            possessionMonsterList[i].SetCurrentMPValue(savedata.currentMp);
            possessionMonsterList[i].SetAttackValue(savedata.attack);
            possessionMonsterList[i].SetDefenceValue(savedata.defence);
            possessionMonsterList[i].SetSPAttackValue(savedata.spAttack);
            possessionMonsterList[i].SetSPDefenceValue(savedata.spDefence);
            possessionMonsterList[i].SetSpeedValue(savedata.speed);

            possessionMonsterList[i].SetLevelValue(savedata.level);
            possessionMonsterList[i].SetEXPValue(savedata.exp);
            possessionMonsterList[i].SetEXPToNextLevel(savedata.expToNextLevel);

            possessionMonsterList[i].SetSkillList(savedata.skillList);


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

        friend.GetComponent<MonsterBase>().SetNickName(savedata.nickName);
        friend.GetComponent<MonsterBase>().SetID(savedata.id);
        friend.GetComponent<MonsterBase>().SetMaxHPValue(savedata.maxHp);
        friend.GetComponent<MonsterBase>().SetMaxMPValue(savedata.maxMp);

        friend.GetComponent<MonsterBase>().SetCurrentHPValue(savedata.maxHp);
        friend.GetComponent<MonsterBase>().SetCurrentMPValue(savedata.maxMp);

        friend.GetComponent<MonsterBase>().SetAttackValue(savedata.attack);
        friend.GetComponent<MonsterBase>().SetDefenceValue(savedata.defence);
        friend.GetComponent<MonsterBase>().SetSPAttackValue(savedata.spAttack);
        friend.GetComponent<MonsterBase>().SetSPDefenceValue(savedata.spDefence);
        friend.GetComponent<MonsterBase>().SetSpeedValue(savedata.speed);

        friend.GetComponent<MonsterBase>().SetLevelValue(savedata.level);
        friend.GetComponent<MonsterBase>().SetEXPValue(savedata.exp);
        friend.GetComponent<MonsterBase>().SetEXPToNextLevel(savedata.expToNextLevel);

        possessionMonsterList[i].SetSkillList(savedata.skillList);



    }
    /// <summary>
    /// 指定したリストの位置に指定したモンスターを設定
    /// </summary>
    /// <param name="number">リスト番号</param>
    /// <param name="monster">モンスター</param>
    public void SetPossessionMonsterList(int number, GameObject monster)
    {
        possessionMonsterList[number] = monster.GetComponent<MonsterBase>();
    }

    public void SetPossessionMonsterList(List<MonsterBase> monsterList)
    {
        possessionMonsterList = monsterList;
        SetPartyMonsterList();
    }
    public void SetPartyMonsterList()
    {
        partyMonsterList = possessionMonsterList;
    }

    public List<MonsterBase> GetPossessionMonsterList()
    {
        return possessionMonsterList;
    }

    public string GetSavedWord()
    {
        return savedWord;
    }

    public void Reset_individual_IDList()
    {
        individual_IDList = new List<float>();
    }

    public bool AddIndividualIDList(float individual_ID)
    {
        foreach (var item in individual_IDList)
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
        for (int i = 0; i < possessionMonsterList.Count; i++)
        {
            if (possessionMonsterList[i] == null)
            {
                return;
            }


            MonsterSaveData saveData = new MonsterSaveData();

            saveData.id = possessionMonsterList[i].GetId();
            saveData.nickName = possessionMonsterList[i].GetNickname();
            saveData.maxHp = possessionMonsterList[i].GetMaxHPValue();
            saveData.maxMp = possessionMonsterList[i].GetMaxMPValue();
            saveData.currentHp = possessionMonsterList[i].GetCurrentHPValue();
            saveData.currentMp = possessionMonsterList[i].GetCurrentMPValue();
            saveData.attack = possessionMonsterList[i].GetAttackValue();
            saveData.defence = possessionMonsterList[i].GetDefenceValue();
            saveData.spAttack = possessionMonsterList[i].GetSpAttackValue();
            saveData.spDefence = possessionMonsterList[i].GetSpDefenceValue();
            saveData.speed = possessionMonsterList[i].GetSpeedValue();
            saveData.exp = possessionMonsterList[i].GetEXPValue();
            saveData.level = possessionMonsterList[i].GetLevelValue();
            saveData.expToNextLevel = possessionMonsterList[i].GetEXPToNextLevel();

            saveData.skillList = possessionMonsterList[i].GetSkillList();




            saveMonsterData(id, saveData, i);

        }

    }

    /// <summary>
    /// 本格的なロード
    /// </summary>
    /// <param name="id"></param>
    public void PlayerMonstersLoad(int id)
    {
        for (int i = 0; i < possessionMonsterList.Count; i++)
        {
            MonsterSaveData savedata = loadMonsterData(id, i);
            if (savedata == null)
            {
                return;
            }

            possessionMonsterList[i] = monsterNullData;

            possessionMonsterList[i].SetNickName(savedata.nickName);
            possessionMonsterList[i].SetID(savedata.id);
            possessionMonsterList[i].SetMaxHPValue(savedata.maxHp);
            possessionMonsterList[i].SetMaxMPValue(savedata.maxMp);
            possessionMonsterList[i].SetCurrentHPValue(savedata.currentHp);
            possessionMonsterList[i].SetCurrentMPValue(savedata.currentMp);
            possessionMonsterList[i].SetAttackValue(savedata.attack);
            possessionMonsterList[i].SetDefenceValue(savedata.defence);
            possessionMonsterList[i].SetSPAttackValue(savedata.spAttack);
            possessionMonsterList[i].SetSPDefenceValue(savedata.spDefence);
            possessionMonsterList[i].SetSpeedValue(savedata.speed);

            possessionMonsterList[i].SetLevelValue(savedata.level);
            possessionMonsterList[i].SetEXPValue(savedata.exp);
            possessionMonsterList[i].SetEXPToNextLevel(savedata.expToNextLevel);

            possessionMonsterList[i].SetSkillList(savedata.skillList);


        }
    }

    public List<MonsterBase> GetAllMonsterList()
    {
        return allMonsterList;
    }
}

