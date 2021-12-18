﻿using System.Collections.Generic;
using UnityEngine;



public class MonsterBase : MonoBehaviour
{
    //ID
    [SerializeField] int _id;
    //個別ID
    [SerializeField] float _individualID;
    //名前
    [SerializeField] string _monsterName;
    //ニックネーム
    [SerializeField] string _nickName;
    //イメージ
    [SerializeField] Sprite _image;
    //属性
    [SerializeField] Element.BattleElement _element;
    //パッシブスキル
    [SerializeField] PassiveSkillBase _pSkill;
    //最大HP
    [SerializeField] int _maxHp;
    //現在のHP
    [SerializeField] int _currentHp;
    //最大MP
    [SerializeField] int _maxMp;
    //現在のMP
    [SerializeField] int _currentMp;

    //ベースのPrefab
    [SerializeField] GameObject _myPrefab;

    //実体化したオブジェクト
    public GameObject RealObject;

    //レベル
    [SerializeField] int _level;
    //経験値
    [SerializeField] int _exp;
    //次のレベルまでの経験値
    [SerializeField] int _expToNextLevel;
    //倒したときのもらえる経験値
    [SerializeField] int _getExp;
    //倒したときにもらえるお金
    [SerializeField] int _money;

    public enum MonsterState
    {
        Normal,
        Guard,
        Charge,
    }

    public MonsterState status = MonsterState.Normal;

    //戦闘時のキャラ番号
    public BattleMonsterTag.CharactorTag charactorTag;

    //戦闘の時のAI
    public CommandAI _commandAi;

    public delegate void D();
    public D AfterAction;
    public D AfterDead;

    [SerializeField] bool _checkTheEndOfAnimation = false;

    [SerializeField] Animator _animator;

    bool _acted = false;
    public string ActionName;

    [SerializeField] bool _checkTheEndOfDeadAnimation = false;

    [SerializeField] List<CardData> cardDatas;


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
        this._monsterName = monsterName;
        this._level = level;
        this._maxHp = maxHp;
        this._maxMp = maxMp;
        this._attack = attack;
        this._defence = defence;
        this._spAttack = spAttack;
        this._spDefence = spDefence;
        this._speed = speed;
    }

    public void Start()
    {
        _commandAi = new CommandAI();
    }

    public void Update()
    {
        //アニメーションが終了しているか確認する
        if (_checkTheEndOfAnimation)
        {
            //アクションをしていないか確認
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(ActionName))
            {
                //アクション済みなら
                if (_acted)
                {
                    //アクションを既に行っていたなら、アクション終了して通知
                    _checkTheEndOfAnimation = false;
                    _acted = false;
                    //アイドル状態に戻す
                    MotionIdle();
                
                    Debug.Log("アニメーション済み");
                    AfterAction.Invoke();
                }
            }
            else
            {
                //アクション中
                _acted = true;
            }

        }
        if (_checkTheEndOfDeadAnimation)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {

                _acted = false;
                AfterDead.Invoke();
                _checkTheEndOfDeadAnimation = false;
            }
        }

    }

    public SkillBase ThinkOfASkill()
    {
        if (_commandAi)
        {
            Debug.Log("hey");
        }
        return _commandAi.ChoiceSkill(skillList);
    }

    /// <summary>
    /// IDを取得する
    /// </summary>
    /// <returns>ID</returns>
    public int GetId()
    {
        return _id;
    }

    public void SetId(int id)
    {
        this._id = id;
    }

    public float GetIndividualID()
    {
        return _individualID;

    }

    /// <summary>
    /// ニックネームを取得する
    /// </summary>
    /// <returns></returns>
    public string GetNickname()
    {
        if (_nickName != "")
        {
            return _nickName;
        }
        else
        {
            return _monsterName;
        }
    }

    /// <summary>
    /// ニックネームを付ける
    /// </summary>
    /// <param name="nickName">新たなニックネーム</param>
    public void SetNickName(string nickName)
    {
        this._nickName = nickName;
    }

    public void SetID(int id)
    {
        this._id = id;
    }



    public void MotionIdle()
    {
        _animator.SetInteger("BattleMode", 0);
        this.ActionName = "IdleNormal";
    }

    public void MotionMove()
    {
        _animator.SetInteger("BattleMode", 2);
        this.ActionName = "WalkFWD";
    }

    public void MotionAttack()
    {
        _animator.SetInteger("BattleMode", 10);
        this.ActionName = "Attack01";
    }

    public void MotionTakeDamege()
    {
        _animator.SetInteger("BattleMode", 50);
        this.ActionName = "TakeDamage";
    }

    public void MotionVictory()
    {
        _animator.SetInteger("BattleMode", 1000);
        this.ActionName = "Victory";
    }

    public void MotionDead()
    {
        _animator.SetInteger("BattleMode", 999);
        this.ActionName = "Die";
    }

    public void CheckEndOfAnimation()
    {
        _checkTheEndOfAnimation = true;
    }

    public void CheckEndOfAnimation(string actionName)
    {
        _checkTheEndOfAnimation = true;
        this.ActionName = actionName;

    }

    public void CheckEndOfDeadAnimation()
    {
        _checkTheEndOfDeadAnimation = true;
    }

    public PassiveSkillBase GetPassiveSkill()
    {
        return _pSkill;
    }

    public int Getlevel()
    {
        return _level;
    }

    public void Setlevel(int level)
    {
        this._level = level;
    }

    public Sprite GetImage()
    {
        return _image;
    }

    public void SetImage(Sprite image)
    {
        this._image = image;
    }

    public int GetAttackValue()
    {
        return _attack;
    }
    public void SetAttackValue(int value)
    {
        _attack = value;
    }

    public int GetDefenceValue()
    {
        return _defence;
    }
    public void SetDefenceValue(int value)
    {
        _defence = value;
    }

    public int GetSpAttackValue()
    {
        return _spAttack;
    }
    public void SetSPAttackValue(int value)
    {
        _spAttack = value;
    }

    public int GetSpDefenceValue()
    {
        return _spDefence;
    }
    public void SetSPDefenceValue(int value)
    {
        _spDefence = value;
    }

    public int GetSpeedValue()
    {
        return _speed;
    }
    public void SetSpeedValue(int value)
    {
        _speed = value;
    }

    public int GetMaxHPValue()
    {
        return _maxHp;
    }
    public void SetMaxHPValue(int value)
    {

        _maxHp = value;
    }
    public int GetMaxMPValue()
    {
        return _maxMp;
    }
    public void SetMaxMPValue(int value)
    {
        _maxMp = value;
    }

    public int GetCurrentHPValue()
    {
        return _currentHp;
    }
    public void SetCurrentHPValue(int value)
    {

        _currentHp = value;
    }

    public int GetCurrentMPValue()
    {
        return _currentMp;
    }
    public void SetCurrentMPValue(int value)
    {
        _currentMp = value;
    }

    public void SetLevelValue(int level)
    {
        this._level = level;
    }

    public int GetLevelValue()
    {
        return _level;
    }

    public void SetEXPValue(int exp)
    {
        this._exp = exp;
    }

    public int GetEXPValue()
    {
        return _exp;
    }

    public void SetEXPToNextLevel(int exp_To_NextLevel)
    {
        this._expToNextLevel = exp_To_NextLevel;
    }

    public int GetEXPToNextLevel()
    {
        return _expToNextLevel;
    }

    public void SetSkillList(List<SkillBase> list)
    {
        skillList = list;
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp < 0)
        {
            _currentHp = 0;
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
            if (usedSkill.GetUsedMp() <= _currentMp)
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
            if ((_exp + plusExp) >= _expToNextLevel)
            {
                int nextExp = _exp + plusExp - _expToNextLevel;
                LevelUp(nextExp);

                float expToNextLevelFloat = _expToNextLevel * 1.2f;
                _expToNextLevel = (int)expToNextLevelFloat;

                result = true;
            }
            else
            {
                _exp += plusExp;
                break;
            }
        }
        return result;

    }

    public void LevelUp(int exp)
    {
        this._exp = exp;
        _level++;
        _maxHp += _maxHpLevelUp;
        _maxMp += _maxMpLevelUp;
        _currentHp = _maxHp;
        _currentMp = _maxMp;
        _attack += _attackLevelUp;
        _defence += _defenceLevelUp;
        _spAttack += _spAttackLevelUp;
        _spDefence += _spDefenceLevelUp;
        _speed += _speedLevelUp;


    }

    public int GetExp_WhenKilled()
    {
        return _getExp;
    }

    public int GetMoney_WhenKilled()
    {
        return _money;
    }

    public void UseMp(int point)
    {
        _currentMp -= point;
    }

    public void SetPrefab(GameObject prefab)
    {
        _myPrefab = prefab;
    }
    public GameObject GetPrefab()
    {
        return _myPrefab;
    }

    public Element.BattleElement GetElement()
    {
        return _element;
    }

}
