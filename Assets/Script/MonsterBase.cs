using System.Collections.Generic;
using UnityEngine;



public class MonsterBase : MonoBehaviour
{
    //ID
    [SerializeField] int id;

    //個別ID
    [SerializeField] float individual_ID;

    //名前
    [SerializeField] string monsterName;
    //ニックネーム
    [SerializeField] string nickName;

    //イメージ
    [SerializeField] Sprite image;

    //属性
    [SerializeField] Element.BattleElement element;

    //パッシブスキル
    [SerializeField] PassiveSkillBase pSkill;

    //最大HP
    [SerializeField] int maxHp;
    //現在のHP
    [SerializeField] int currentHp;
    //最大MP
    [SerializeField] int maxMp;
    //現在のMP
    [SerializeField] int currentMp;

    //攻撃力
    [SerializeField] int attack;
    //防御力
    [SerializeField] int defence;
    //特殊攻撃力
    [SerializeField] int spAttack;
    //特殊防御力
    [SerializeField] int spDefence;
    //素早さ
    [SerializeField] int speed;
    //運
    [SerializeField] int luck;
    //命中率
    [SerializeField] int hit;
    //回避率
    [SerializeField] int avoidance;

    //LVUP時上昇最大HP
    [SerializeField] int maxHpLevelUp;
    //LVUP時上昇最大MP
    [SerializeField] int maxMpLevelUp;

    //LVUP時上昇攻撃力
    [SerializeField] int attackLevelUp;
    //LVUP時上昇防御力
    [SerializeField] int defenceLevelUp;
    //LVUP時上昇特殊攻撃力
    [SerializeField] int spAttackLevelUp;
    //LVUP時上昇特殊防御力
    [SerializeField] int spDefenceLevelUp;
    //LVUP時上昇素早さ
    [SerializeField] int speedLevelUp;
    //LVUP時上昇運
    [SerializeField] int luckLevelUp;
    //攻撃時ディレイ
    public float attackDelay = 1;

    //ベースのPrefab
    [SerializeField] GameObject myPrefab;

    //実体化したオブジェクト
    public GameObject realObject;

    //レベル
    [SerializeField] int level;
    //経験値
    [SerializeField] int exp;

    //次のレベルまでの経験値
    [SerializeField] int expToNextLevel;

    //倒したときのもらえる経験値
    [SerializeField] int getExp;
    //倒したときにもらえるお金
    [SerializeField] int money;

    public enum MonsterState
    {
        Normal,
        Guard,
        Charge,



    }

    public MonsterState status = MonsterState.Normal;


    //所持アイテム ※あとで
    //Item item;

    //イベント当たり判定
    [SerializeField] bool isBattle = false;
    [SerializeField] int eventCollision;

    //戦闘時のキャラ番号
    public BattleMonsterTag.CharactorTag charactorTag;
    //タイムライン戦闘の際に使うクールタイム
    public float coolTime = 0;
    //戦闘の時のAI
    public CommandAI commandAi;

    public delegate void D();
    public D AfterAction;
    public D AfterDead;

    [SerializeField] bool checkTheEndOfAnimation = false;

    [SerializeField] Animator animator;

    bool acted = false;
    public string actionName;

    [SerializeField] bool checkTheEndOfDeadAnimation = false;

    [SerializeField] MonsterManager monsterManager;

    [SerializeField] List<SkillBase> skillList;


    /// <summary>
    /// モンスター情報の初期設定
    /// </summary>
    /// <param name="monsterName"></param>
    /// <param name="level"></param>
    /// <param name="maxHp"></param>
    /// <param name="maxMp"></param>
    /// <param name="attack"></param>
    /// <param name="defence"></param>
    /// <param name="spAttack"></param>
    /// <param name="spDefence"></param>
    /// <param name="speed"></param>
    public MonsterBase(string monsterName, int level,
        int maxHp, int maxMp, int attack, int defence, int spAttack, int spDefence, int speed)
    {
        this.monsterName = monsterName;
        this.level = level;
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.attack = attack;
        this.defence = defence;
        this.spAttack = spAttack;
        this.spDefence = spDefence;
        this.speed = speed;
    }

    public void Start()
    {
        commandAi = new CommandAI();
    }



    public void Update()
    {
        //アニメーションが終了しているか確認する
        if (checkTheEndOfAnimation)
        {
            //アクションをしていないか確認
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(actionName))
            {
                //アクション済みなら
                if (acted)
                {
                    //アクションを既に行っていたなら、アクション終了して通知
                    checkTheEndOfAnimation = false;
                    acted = false;
                    //アイドル状態に戻す
                    MotionIdle();
                
                    Debug.Log("アニメーション済み");
                    AfterAction.Invoke();
                }
            }
            else
            {
                //アクション中
                acted = true;
            }

        }
        if (checkTheEndOfDeadAnimation)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {

                acted = false;
                AfterDead.Invoke();
                checkTheEndOfDeadAnimation = false;
            }
        }

    }

    public SkillBase ThinkOfASkill()
    {
        if (commandAi)
        {
            Debug.Log("hey");
        }
        return commandAi.ChoiceSkill(skillList);
    }

    /// <summary>
    /// IDを取得する
    /// </summary>
    /// <returns>ID</returns>
    public int GetId()
    {
        return id;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public float GetIndividualID()
    {
        return individual_ID;

    }

    /// <summary>
    /// ニックネームを取得する
    /// </summary>
    /// <returns></returns>
    public string GetNickname()
    {
        if (nickName != "")
        {
            return nickName;
        }
        else
        {
            return monsterName;
        }
    }

    /// <summary>
    /// ニックネームを付ける
    /// </summary>
    /// <param name="nickName">新たなニックネーム</param>
    public void SetNickName(string nickName)
    {
        this.nickName = nickName;
    }

    public void SetID(int id)
    {
        this.id = id;
    }



    public void MotionIdle()
    {
        animator.SetInteger("BattleMode", 0);
        this.actionName = "IdleNormal";
    }

    public void MotionMove()
    {
        animator.SetInteger("BattleMode", 2);
        this.actionName = "WalkFWD";
    }

    public void MotionAttack()
    {
        animator.SetInteger("BattleMode", 10);
        this.actionName = "Attack01";
    }

    public void MotionTakeDamege()
    {
        animator.SetInteger("BattleMode", 50);
        this.actionName = "TakeDamage";
    }

    public void MotionVictory()
    {
        animator.SetInteger("BattleMode", 1000);
        this.actionName = "Victory";
    }

    public void MotionDead()
    {
        animator.SetInteger("BattleMode", 999);
        this.actionName = "Die";
    }

    public void CheckEndOfAnimation()
    {
        checkTheEndOfAnimation = true;
    }

    public void CheckEndOfAnimation(string actionName)
    {
        checkTheEndOfAnimation = true;
        this.actionName = actionName;

    }

    public void CheckEndOfDeadAnimation()
    {
        checkTheEndOfDeadAnimation = true;
    }

    public PassiveSkillBase GetPassiveSkill()
    {
        return pSkill;
    }

    public int Getlevel()
    {
        return level;
    }

    public void Setlevel(int level)
    {
        this.level = level;
    }

    public Sprite GetImage()
    {
        return image;
    }

    public void SetImage(Sprite image)
    {
        this.image = image;
    }

    public int GetAttackValue()
    {
        return attack;
    }
    public void SetAttackValue(int value)
    {
        attack = value;
    }

    public int GetDefenceValue()
    {
        return defence;
    }
    public void SetDefenceValue(int value)
    {
        defence = value;
    }

    public int GetSpAttackValue()
    {
        return spAttack;
    }
    public void SetSPAttackValue(int value)
    {
        spAttack = value;
    }

    public int GetSpDefenceValue()
    {
        return spDefence;
    }
    public void SetSPDefenceValue(int value)
    {
        spDefence = value;
    }

    public int GetSpeedValue()
    {
        return speed;
    }
    public void SetSpeedValue(int value)
    {
        speed = value;
    }

    public int GetMaxHPValue()
    {
        return maxHp;
    }
    public void SetMaxHPValue(int value)
    {

        maxHp = value;
    }
    public int GetMaxMPValue()
    {
        return maxMp;
    }
    public void SetMaxMPValue(int value)
    {
        maxMp = value;
    }

    public int GetCurrentHPValue()
    {
        return currentHp;
    }
    public void SetCurrentHPValue(int value)
    {

        currentHp = value;
    }

    public int GetCurrentMPValue()
    {
        return currentMp;
    }
    public void SetCurrentMPValue(int value)
    {
        currentMp = value;
    }

    public void SetLevelValue(int level)
    {
        this.level = level;
    }

    public int GetLevelValue()
    {
        return level;
    }

    public void SetEXPValue(int exp)
    {
        this.exp = exp;
    }

    public int GetEXPValue()
    {
        return exp;
    }

    public void SetEXPToNextLevel(int exp_To_NextLevel)
    {
        this.expToNextLevel = exp_To_NextLevel;
    }

    public int GetEXPToNextLevel()
    {
        return expToNextLevel;
    }

    public void SetSkillList(List<SkillBase> list)
    {
        skillList = list;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
        }
    }

    public List<SkillBase> GetSkillList()
    {
        return skillList;
    }

    public SkillBase GetRandomSkill()
    {
        int random;
        SkillBase usedSkill;
        while (true)
        {
            random = Random.Range(0, skillList.Count);
            usedSkill = this.skillList[random];
            if (usedSkill.GetUsedMp() <= currentMp)
            {
                break;
            }
        }


        //Debug.Log(usedSkill.GetName() + "をランダムに選択しました");

        return usedSkill;
    }

    public void SkillAcquisition(SkillBase skill)
    {
        skillList.Add(skill);
    }
    /// <summary>
    /// 経験値アップ
    /// </summary>
    /// <param name="plusExp">追加経験値</param>
    /// <returns>レベルアップしたかどうかを返す</returns>
    public bool EXPUp(int plusExp)
    {
        bool result = false;
        while (true)
        {
            if ((exp + plusExp) >= expToNextLevel)
            {
                int nextExp = exp + plusExp - expToNextLevel;
                LevelUp(nextExp);

                float expToNextLevelFloat = expToNextLevel * 1.2f;
                expToNextLevel = (int)expToNextLevelFloat;

                result = true;
            }
            else
            {
                exp += plusExp;
                break;
            }
        }
        return result;

    }

    public void LevelUp(int exp)
    {
        this.exp = exp;
        level++;
        maxHp += maxHpLevelUp;
        maxMp += maxMpLevelUp;
        currentHp = maxHp;
        currentMp = maxMp;
        attack += attackLevelUp;
        defence += defenceLevelUp;
        spAttack += spAttackLevelUp;
        spDefence += spDefenceLevelUp;
        speed += speedLevelUp;


    }

    public int GetExp_WhenKilled()
    {
        return getExp;
    }

    public int GetMoney_WhenKilled()
    {
        return money;
    }

    public void UseMp(int point)
    {
        currentMp -= point;
    }

    public void SetPrefab(GameObject prefab)
    {
        myPrefab = prefab;
    }
    public GameObject GetPrefab()
    {
        return myPrefab;
    }

    public Element.BattleElement GetElement()
    {
        return element;
    }

}
