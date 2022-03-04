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
    //倒したときのもらえる経験値
    [SerializeField] int _getExp;
    //倒したときにもらえるお金
    [SerializeField] int _money;

    [SerializeField] MonsterBase _evolutionyMonster;


    [SerializeField] List<StatusEffectBase> _statusEffectList = new List<StatusEffectBase>();

    public delegate void Delegate();
    public Delegate AfterAction;
    public Delegate AfterDead;

    [SerializeField] bool _checkTheEndOfAnimation = false;

    [SerializeField] Animator _animator;

    bool _acted = false;
    public string ActionName;

    [SerializeField] bool _checkTheEndOfDeadAnimation = false;

    [SerializeField] List<CardData> _cardDatas;

    bool _inParty = false;

    [Header("アニメーションのパラメーター名")]
    [SerializeField] string _actionParameterName;
    [SerializeField] string _actionNameIdle = "Idle";
    [SerializeField] int _actionNumberIdle = 0;
    [SerializeField] string _actionNameAttack = "Attack01";
    [SerializeField] int _actionNumberAttack = 10;
    [SerializeField] string _actionNameTakeDamage = "TakeDamage";
    [SerializeField] int _actionNumberTakeDamage = 50;

    public int Id { get => _id; set => _id = value; }
    public float IndividualID { get => _individualID; set => _individualID = value; }
    public string MonsterName { get => _monsterName; set => _monsterName = value; }
    public string NickName { get => _nickName; set => _nickName = value; }
    public Sprite Image { get => _image; set => _image = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    public int CurrentHp { get => _currentHp; set => _currentHp = value; }
    public int MaxMp { get => _maxMp; set => _maxMp = value; }
    public int CurrentMp { get => _currentMp; set => _currentMp = value; }
    public GameObject MyPrefab { get => _myPrefab; set => _myPrefab = value; }
    public List<StatusEffectBase> StatusEffectList { get => _statusEffectList; set => _statusEffectList = value; }
    public List<CardData> CardDatas { get => _cardDatas; set => _cardDatas = value; }
    public int GetExp { get => _getExp; set => _getExp = value; }
    public int Money { get => _money; set => _money = value; }
    public bool InParty { get => _inParty; set => _inParty = value; }
    public MonsterBase EvolutionyMonster { get => _evolutionyMonster; set => _evolutionyMonster = value; }


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
    public MonsterBase(string monsterName, int maxHp, int maxMp)
    {
        this.MonsterName = monsterName;
        this.NickName = monsterName;
        this.MaxHp = maxHp;
        this.MaxMp = maxMp;
    }

    public void Update()
    {
        if (_animator != null)
        {
            //アニメーションが終了しているか確認する
            if (_checkTheEndOfAnimation)
            {

                //アクションをしていないか確認
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName(ActionName))
                {
                    //アクション中
                    _acted = true;
                    Debug.Log(_animator.GetCurrentAnimatorStateInfo(0));
                }
                else
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
                        if (AfterAction != null)
                        {
                            AfterAction.Invoke();
                        }
                    }
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


    public void MotionIdle()
    {
        _animator.SetInteger(_actionParameterName, _actionNumberIdle);
        this.ActionName = _actionNameIdle;
    }

    public void MotionAttack()
    {
        _checkTheEndOfAnimation = true;
        _animator.SetInteger(_actionParameterName, _actionNumberAttack);
        this.ActionName = _actionNameAttack;
    }

    public void MotionTakeDamege()
    {
        _checkTheEndOfAnimation = true;
        _animator.SetInteger(_actionParameterName, _actionNumberTakeDamage);
        this.ActionName = _actionNameTakeDamage;
    }


    public void CheckEndOfDeadAnimation()
    {
        _checkTheEndOfDeadAnimation = true;
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
