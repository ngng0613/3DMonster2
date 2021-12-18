using System.Collections.Generic;
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
    private GameObject realObject;
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

    public MonsterState Status = MonsterState.Normal;

    //戦闘時のキャラ番号
    public BattleMonsterTag.CharactorTag CharactorTag;

    //戦闘の時のAI
    public CommandAI CommandAi;

    public delegate void Delegate();
    public Delegate AfterAction;
    public Delegate AfterDead;

    [SerializeField] bool _checkTheEndOfAnimation = false;

    [SerializeField] Animator _animator;

    bool _acted = false;
    public string ActionName;

    [SerializeField] bool _checkTheEndOfDeadAnimation = false;

    [SerializeField] List<CardData> _cardDatas;

    public int Id { get => _id; set => _id = value; }
    public float IndividualID { get => _individualID; set => _individualID = value; }
    public string MonsterName { get => _monsterName; set => _monsterName = value; }
    public string NickName { get => _nickName; set => _nickName = value; }
    public Sprite Image { get => _image; set => _image = value; }
    public Element.BattleElement Element { get => _element; set => _element = value; }
    public PassiveSkillBase PSkill { get => _pSkill; set => _pSkill = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    public int CurrentHp { get => _currentHp; set => _currentHp = value; }
    public int MaxMp { get => _maxMp; set => _maxMp = value; }
    public int CurrentMp { get => _currentMp; set => _currentMp = value; }
    public GameObject MyPrefab { get => _myPrefab; set => _myPrefab = value; }
    public GameObject RealObject { get => realObject; set => realObject = value; }


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
        this.MonsterName = monsterName;
        this.MaxHp = maxHp;
        this.MaxMp = maxMp;
    }

    public void Start()
    {
        CommandAi = new CommandAI();
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

    /// <summary>
    /// IDを取得する
    /// </summary>
    /// <returns>ID</returns>
    public int GetId()
    {
        return Id;
    }

    public void SetId(int id)
    {
        this.Id = id;
    }

    public float GetIndividualID()
    {
        return IndividualID;

    }

    /// <summary>
    /// ニックネームを取得する
    /// </summary>
    /// <returns></returns>
    public string GetNickname()
    {
        if (NickName != "")
        {
            return NickName;
        }
        else
        {
            return MonsterName;
        }
    }

    /// <summary>
    /// ニックネームを付ける
    /// </summary>
    /// <param name="nickName">新たなニックネーム</param>
    public void SetNickName(string nickName)
    {
        this.NickName = nickName;
    }

    public void SetID(int id)
    {
        this.Id = id;
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
        return PSkill;
    }
    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp < 0)
        {
            CurrentHp = 0;
        }
    }


}
