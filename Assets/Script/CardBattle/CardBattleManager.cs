using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public enum Side
{
    Player,
    Enemy,
}

public class CardBattleManager : MonoBehaviour
{
    Sequence sequence;
    StageData _stageData;
    [SerializeField] StageData _stageDataForDebug;
    [SerializeField] Deck _playerDeck;
    [SerializeField] Deck _enemyDeck;

    public enum Phase
    {
        Start,
        Player,
        Preparation,
        Command,
        ChooseSkill,
        Target,
        Attack,
        AfterAttack,
        Enemy,
        Wait,
        Win,
        Lose,
    }

    [SerializeField] List<MonsterBase> _debugMonsters;

    [SerializeField] Phase _phase = Phase.Start;
    public bool KeyReception { get; set; } = false;

    float _cameraMovementTime = 1.0f;
    [SerializeField] float _tweenSpeed = 1.0f;

    MonsterBase _thisTurnActorMonsterBase;
    MonsterBase _thisTurnTargetMonsterBase;


    /// <summary>
    /// 攻撃待機しているキャラクター（同時に攻撃順が回った際に使う）
    /// </summary>
    List<MonsterBase> _attackWaitingList;

    //スキル
    [SerializeField] StatusEffectBase _guardStatusEffect;

    /*
     * マネージャー
     */
    [SerializeField] SoundManager _soundManager;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] Hand _hand;
    [SerializeField] HpGauge _enemyStatusPrefab;
    [SerializeField] DamageCalculator _damageCalculator;
    [SerializeField] StatusIconView _playerStatusIconView;
    [SerializeField] StatusIconView _enemyStatusIconView;
    [SerializeField] ResultManager _resultManager;
    [SerializeField] GameResultCanvas _gameResultManager;

    /*
     * オブジェクト
     */
    [SerializeField] CardObject _cardObjectPrefab;
    [SerializeField] List<GameObject> _playerMonsterObjectsList = new List<GameObject>();
    [SerializeField] List<GameObject> _enemyMonsterObjectsList = new List<GameObject>();
    [SerializeField] List<Transform> _playerMonstersPositionList = new List<Transform>();
    [SerializeField] List<Transform> _enemyMonsterPositionList = new List<Transform>();
    [SerializeField] List<MonsterBase> _playerMonsterBaseList = new List<MonsterBase>();
    [SerializeField] List<MonsterBase> _enemyMonsterBaseList = new List<MonsterBase>();
    [SerializeField] List<Transform> _playerMonsterLookPositionList = new List<Transform>();
    [SerializeField] List<Transform> _enemyMonsterLookPositionList = new List<Transform>();
    DamageView _damageView;
    [SerializeField] DamageView _damageViewPrefab;
    Camera _cameraComponent;
    [SerializeField] Color _damageWeakColor;
    [SerializeField] Color _damageResistColor;
    [SerializeField] Fade _fade;
    [SerializeField] GameObject _gameVictoryObjects;
    [SerializeField] GameObject _gameLoseObjects;
    [SerializeField] GameObject _nextButton;
    [SerializeField] CanvasGroup _startAnimationCanvasGroup;
    [SerializeField] HpGauge _playerHpGauge;
    [SerializeField] HpGauge _enemyHpGauge;
    [SerializeField] EnemyAi _enemyAi;
    [SerializeField] Canvas _resultCanvas;
    [SerializeField] Canvas _gameResultCanvas;
    [SerializeField] GameObject _turnEndButton;
    [SerializeField] MessageUi _lackOfMana;

    UnityEngine.Random _random = new UnityEngine.Random();
    delegate void Func();

    public TrashDelegate Trash;


    void Start()
    {
        Application.targetFrameRate = 60;
        //初期化

        MonsterBase playerMonster = GameManager.Instance.PlayerMonster;
        playerMonster.CurrentHp = GameManager.Instance.PlayerHp;
        if (GameManager.Instance.MonsterParty.Count == 0)
        {
            GameManager.Instance.MonsterParty = _debugMonsters;
        }
        List<MonsterBase> monsterList = GameManager.Instance.MonsterParty;
        playerMonster.MaxMp = monsterList.Sum(x => x.MaxMp);
        _playerMonsterBaseList[0] = playerMonster;
        _playerHpGauge.Setup(playerMonster, _cameraComponent, null);
        _playerStatusIconView.Monster = _playerMonsterBaseList[0];
        _enemyStatusIconView.Monster = _enemyMonsterBaseList[0];
        _resultManager.BackToMap = this.BackToMap;

        //ステージデータ読み込み
        _stageData = GameManager.Instance.NextBattleStage;
        if (_stageData != null)
        {
            _enemyMonsterBaseList[0] = _stageData.EnemyMonster;
            SetEnemyMonsterBase(_enemyMonsterObjectsList[0], 0, _enemyMonsterPositionList[0], _enemyMonsterBaseList[0]);
        }
        else
        {
            Debug.LogWarning("ステージデータがありませんでした.デバッグ用のステージデータを読み込みます");
            _stageData = _stageDataForDebug;
            //敵情報のセット
            for (int i = 0; i < _enemyMonsterObjectsList.Count; i++)
            {
                if (_enemyMonsterObjectsList[i])
                {
                    Debug.Log($"1:{_enemyMonsterObjectsList.Count}, 2:{_enemyMonsterPositionList.Count} 3:{_enemyMonsterBaseList.Count}  ");
                    SetEnemyMonsterBase(_enemyMonsterObjectsList[i], i, _enemyMonsterPositionList[i], _enemyMonsterBaseList[i]);
                }
            }
        }



        //一度ゲームオブジェクトのデッキをつくる

        monsterList = GameManager.Instance.MonsterParty;

        List<CardObject> playerObjectDeck = new List<CardObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            for (int k = 0; k < monsterList[i].CardDatas.Count; k++)
            {
                CardObject tempCard = Instantiate(_cardObjectPrefab);
                tempCard.InBattle = true;
                tempCard.Data = monsterList[i].CardDatas[k];
                tempCard.Check = CheckIfCanUseCardPlayerSide;
                //画面外で保存
                tempCard.transform.position = new Vector3(-200, -500, -1000);
                tempCard.gameObject.transform.SetParent(_playerDeck.transform);
                playerObjectDeck.Add(tempCard);
            }
        }

        _playerDeck.Setup(playerObjectDeck);
        //カード使用時の処理の追加
        _hand.Setup(PlayCard, _playerDeck.Trash);

        //敵もゲームオブジェクトのデッキをつくる
        _enemyAi.Setup(_enemyMonsterBaseList[0]);
        List<CardObject> enemyObjectDeck = new List<CardObject>();
        Debug.Log($"敵のデッキ枚数：{_enemyMonsterBaseList[0].CardDatas.Count}");
        for (int i = 0; i < _enemyMonsterBaseList[0].CardDatas.Count; i++)
        {
            CardObject tempCard = Instantiate(_cardObjectPrefab);
            tempCard.InBattle = true;
            tempCard.Data = _enemyMonsterBaseList[0].CardDatas[i];
            tempCard.Check = CheckIfCanUseCardEnemySide;
            //画面外で保存
            tempCard.transform.position = new Vector3(200, -500, -1000);
            tempCard.gameObject.transform.SetParent(_enemyDeck.transform);
            enemyObjectDeck.Add(tempCard);
        }
        _enemyDeck.Setup(enemyObjectDeck);
        _enemyAi.Deck = _enemyDeck;
        //カード使用時の処理の追加
        _enemyAi.Hand.Setup(PlayCard, _enemyDeck.Trash);

        PhaseStart();
    }

    void PhaseStart()
    {
        sequence = DOTween.Sequence();
        sequence
        .Append(_startAnimationCanvasGroup.DOFade(1.0f, 1.0f))
        .AppendInterval(0.5f)
        .AppendCallback(() => _soundManager.PlaySe(SoundManager.SeList.BattleStart))
        .Append(_startAnimationCanvasGroup.gameObject.transform.DOScale(10.0f, 1.0f))
        .Insert(1.5f, _startAnimationCanvasGroup.DOFade(0.0f, 0.5f))
        .InsertCallback(1.5f, () =>
        {
            PhasePreparation();
        });
    }

    /// <summary>
    /// カードをドローする
    /// </summary>
    IEnumerator DrawCard()
    {
        CardObject card = _playerDeck.Draw();
        if (card != null)
        {
            _hand.AddHand(card);
        }
        yield return null;
    }

    void PhasePreparation()
    {
        _phase = Phase.Preparation;
        _soundManager.PlayBgm();

        StartCoroutine(PhaseDraw());
    }

    /// <summary>
    /// ドローするフェイズ
    /// </summary>
    /// <returns></returns>
    IEnumerator PhaseDraw()
    {
        _phase = Phase.Player;
        _playerMonsterBaseList[0].CurrentMp = _playerMonsterBaseList[0].MaxMp;
        UpdateMana();
        List<StatusEffectBase> statusList = _playerMonsterBaseList[0].StatusEffectList;
        for (int i = 0; i < statusList.Count; i++)
        {
            if (statusList[i].Count > 0)
            {
                Debug.Log(statusList[i].Name + "のステータスカウントは" + statusList[i].Count);
                //statusList[i].Count--;
                if (statusList[i].Count == 0)
                {
                    statusList.RemoveAt(i);
                    //参照するインデックスにズレが出るため、一度戻す
                    if (statusList.Count <= 0)
                    {
                        break;
                    }
                    i--;
                }

            }
        }
        _playerStatusIconView.UpdateView();

        int numberOfDraw = 3;
        for (int i = 0; i < numberOfDraw; i++)
        {
            yield return DrawCard();
            yield return Wait(0.3f);
        }
    }

    /// <summary>
    /// マナ表示の更新
    /// </summary>
    void UpdateMana()
    {
        _playerHpGauge.UpdateMp(_playerMonsterBaseList[0].CurrentMp);
        if (_playerMonsterBaseList[0].CurrentMp <= 0)
        {
            _turnEndButton.SetActive(false);
        }

        _enemyHpGauge.UpdateMp(_enemyMonsterBaseList[0].CurrentMp);
    }

    /// <summary>
    /// 一時的に処理をwaitする関数
    /// </summary>
    /// <param name="waitForSeconds"></param>
    /// <returns></returns>
    IEnumerator Wait(float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
    }



    public void PhaseEnemyStart()
    {
        if (_phase == Phase.Player)
        {
            StartCoroutine(PhaseEnemyTurn());
        }
    }

    /// <summary>
    /// 敵のターンの処理
    /// </summary>
    /// <returns></returns>
    IEnumerator PhaseEnemyTurn()
    {
        _phase = Phase.Enemy;
        _turnEndButton.SetActive(true);
        _enemyMonsterBaseList[0].CurrentMp = _enemyMonsterBaseList[0].MaxMp;
        List<CardData> combo = _enemyAi.Think();
        for (int i = 0; i < combo.Count; i++)
        {
            Debug.Log("hey");
            DamageView spellNameView = Instantiate(_damageViewPrefab, _enemyMonsterPositionList[0].transform.position, Quaternion.identity);
            spellNameView.Setup(0, combo[i].CardName, Color.white, _cameraComponent);
            spellNameView.transform.position += new Vector3(0, 2, -3);
            spellNameView.Activate();
            Debug.Log("敵コンボ数：" + combo.Count);
            Debug.Log($"敵は{combo[i].CardName}をプレイした");
            for (int k = 0; k < combo[i].CardSpellBases.Count; k++)
            {
                CardSpellBase partOfSpell = combo[i].CardSpellBases[k];
                switch (partOfSpell.Type)
                {
                    case SpellType.Attack:

                        int damage = _damageCalculator.Calculate(_enemyMonsterBaseList[0], _playerMonsterBaseList[0], partOfSpell);

                        _playerMonsterBaseList[0].TakeDamage(damage);
                        _playerStatusIconView.IconPopup(_guardStatusEffect);
                        //ダメージ処理
                        DamageView damageView = Instantiate(_damageViewPrefab, _playerMonstersPositionList[0].transform.position, Quaternion.identity);
                        damageView.transform.position += new Vector3(0, 2, -3);
                        damageView.Setup(damage, "", Color.white, _cameraComponent);
                        damageView.Activate();
                        _playerHpGauge.UpdateHp(_playerMonsterBaseList[0].CurrentHp);
                        _playerHpGauge.gameObject.transform.DOShakePosition(0.5f, 0.5f, 100);
                        //_playerStatusIconView.UpdateView();
                        break;
                    case SpellType.Guard:


                        break;
                    case SpellType.Buff:

                        StatusEffectBase status = Instantiate(partOfSpell.Status);
                        status.Name = partOfSpell.Status.Name;
                        status.Id = partOfSpell.Status.Id;
                        status.IconSprite = partOfSpell.Status.IconSprite;
                        bool alreadyHave = false;
                        ///敵から見て、敵はプレイヤー
                        if (partOfSpell.Target == SpellTarget.Enemy)
                        {
                            foreach (var tempStatus in _playerMonsterBaseList[0].StatusEffectList)
                            {
                                if (status.Name == tempStatus.Name)
                                {
                                    tempStatus.Count += partOfSpell.EffectValue;
                                    alreadyHave = true;
                                    break;
                                }
                            }
                            if (alreadyHave == false)
                            {
                                status.Count = partOfSpell.EffectValue;
                                _playerMonsterBaseList[0].StatusEffectList.Add(status);
                            }
                            _playerStatusIconView.UpdateView();
                        }
                        else if (partOfSpell.Target == SpellTarget.Player)
                        {
                            foreach (var tempStatus in _enemyMonsterBaseList[0].StatusEffectList)
                            {
                                if (status.Name == tempStatus.Name)
                                {
                                    tempStatus.Count += partOfSpell.EffectValue;
                                    alreadyHave = true;
                                    break;
                                }
                            }
                            if (alreadyHave == false)
                            {
                                status.Count = partOfSpell.EffectValue;
                                _enemyMonsterBaseList[0].StatusEffectList.Add(status);
                            }
                            _enemyStatusIconView.UpdateView();
                        }

                        break;
                    case SpellType.Debuff:
                        break;
                    case SpellType.Draw:
                        break;
                    case SpellType.DisCard:
                        break;
                    default:
                        break;
                }
                _enemyStatusIconView.UpdateView();
                UpdateMana();
                CheckIfDead();
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1);
        }

        yield return PhaseDraw();
    }


    public void CheckIfDead()
    {

        if (_playerMonsterBaseList[0].CurrentHp <= 0)
        {
            PhaseLose();
        }
        if (_enemyMonsterBaseList[0].CurrentHp <= 0)
        {
            PhaseWin();
        }
    }

    public void PhaseWin()
    {
        //リザルト表示
        _resultManager.Setup(_enemyMonsterBaseList[0]);
        _resultCanvas.gameObject.SetActive(true);
        _resultManager.AnimationStart();

    }

    public void PhaseLose()
    {
        //リザルト表示
        _gameResultCanvas.gameObject.SetActive(true);
        _gameResultManager.AnimationStart();
    }

    /// <summary>
    /// カードを使用可能か確認し、使用可能ならマナを消費する
    /// </summary>
    /// <param name="card">確認するカード</param>
    /// <returns></returns>
    public bool CheckIfCanUseCardPlayerSide(CardData card)
    {
        if (_playerMonsterBaseList[0].CurrentMp >= card.Cost)
        {
            _playerMonsterBaseList[0].CurrentMp -= card.Cost;
            UpdateMana();
            return true;
        }
        else
        {
            LackOfMana();
            return false;
        }
    }
    /// <summary>
    /// カードを使用可能か確認し、使用可能ならマナを消費する
    /// </summary>
    /// <param name="card">確認するカード</param>
    /// <returns></returns>
    public bool CheckIfCanUseCardEnemySide(CardData card)
    {
        if (_enemyMonsterBaseList[0].CurrentMp >= card.Cost)
        {
            _enemyMonsterBaseList[0].CurrentMp -= card.Cost;
            UpdateMana();

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// カードを使用する
    /// </summary>
    public void PlayCard(CardData card)
    {
        if (_phase == Phase.Player)
        {
            StartCoroutine(PlayCardCorotine(card));
            _playerStatusIconView.UpdateView();
        }
    }

    /// <summary>
    /// カードを使用した際の処理
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public IEnumerator PlayCardCorotine(CardData card)
    {
        for (int i = 0; i < card.CardSpellBases.Count; i++)
        {
            CardSpellBase partOfSpell = card.CardSpellBases[i];
            switch (partOfSpell.Type)
            {
                case SpellType.Attack:
                    AttackCoroutine(partOfSpell);
                    break;
                case SpellType.Guard:

                    break;
                case SpellType.Buff:
                    StatusEffectBase status = Instantiate(partOfSpell.Status);
                    status.Name = partOfSpell.Status.Name;
                    status.Id = partOfSpell.Status.Id;
                    status.IconSprite = partOfSpell.Status.IconSprite;
                    bool alreadyHave = false;
                    if (partOfSpell.Target == SpellTarget.Enemy)
                    {
                        foreach (var tempStatus in _enemyMonsterBaseList[0].StatusEffectList)
                        {
                            if (status.Name == tempStatus.Name)
                            {
                                tempStatus.Count += partOfSpell.EffectValue;
                                alreadyHave = true;
                                break;
                            }
                        }
                        if (alreadyHave == false)
                        {
                            status.Count = partOfSpell.EffectValue;
                            _enemyMonsterBaseList[0].StatusEffectList.Add(status);
                        }
                        _enemyStatusIconView.UpdateView();
                    }
                    else if (partOfSpell.Target == SpellTarget.Player)
                    {
                        foreach (var tempStatus in _playerMonsterBaseList[0].StatusEffectList)
                        {
                            if (status.Name == tempStatus.Name)
                            {
                                tempStatus.Count += partOfSpell.EffectValue;
                                alreadyHave = true;
                                break;
                            }
                        }
                        if (alreadyHave == false)
                        {
                            status.Count = partOfSpell.EffectValue;
                            _playerMonsterBaseList[0].StatusEffectList.Add(status);
                        }
                        _playerStatusIconView.UpdateView();
                    }


                    break;
                case SpellType.Debuff:
                    break;
                case SpellType.Draw:
                    break;
                case SpellType.DisCard:
                    break;
                default:
                    break;
            }
            _enemyHpGauge.UpdateHp(_enemyMonsterBaseList[0].CurrentHp);
            yield return new WaitForSeconds(1f);
            _enemyStatusIconView.UpdateView();
            CheckIfDead();
        }


    }

    /// <summary>
    /// 攻撃処理のコルーチン
    /// </summary>
    /// <param name="spell"></param>
    void AttackCoroutine(CardSpellBase spell)
    {

        int damage = _damageCalculator.Calculate(_playerMonsterBaseList[0], _enemyMonsterBaseList[0], spell);

        _enemyMonsterBaseList[0].TakeDamage(damage);
        _enemyStatusIconView.IconPopup(_guardStatusEffect);

        //アニメーションの再生
        _enemyMonsterBaseList[0].MotionTakeDamege();
        _enemyHpGauge.gameObject.transform.DOShakePosition(0.5f, 1.0f, 100);

        //ダメージ処理

        DamageView damageView = Instantiate(_damageViewPrefab, _enemyMonsterPositionList[0].transform.position, Quaternion.identity);
        damageView.transform.position += new Vector3(0, 2, -8);
        damageView.Setup(damage, "", Color.white, _cameraComponent);
        damageView.Activate();

    }


    /// <summary>
    /// 敵モンスターデータの設定
    /// </summary>
    /// <param name="enemyObject">生成した敵オブジェクトの保存先</param>
    /// <param name="enemyNumber">敵番号</param>
    /// <param name="enemyPos">敵オブジェクトの配置箇所</param>
    /// <param name="enemyMonsterBase">敵の情報</param>
    void SetEnemyMonsterBase(GameObject enemyObject, int enemyNumber, Transform enemyPos, MonsterBase enemyMonsterBase)
    {
        enemyObject = Instantiate(monsterManager.GetMonsterPrefab(_stageData.EnemyMonster.Id), enemyPos.transform.position, Quaternion.identity);

        Vector3 tempDir = enemyObject.transform.localEulerAngles;
        tempDir.y += 180;
        enemyObject.transform.localEulerAngles = tempDir;
        enemyObject.transform.SetParent(enemyPos);
        enemyObject.transform.localPosition = new Vector3(0, 0, 0);
        enemyMonsterBase = enemyObject.GetComponent<MonsterBase>();
        _enemyMonsterBaseList[enemyNumber] = enemyMonsterBase;
        //敵ステータスの表示
        _enemyHpGauge.Setup(enemyMonsterBase, _cameraComponent, null);
        _enemyStatusIconView.Monster = _enemyMonsterBaseList[0];
    }

    /// <summary>
    /// マナが不足している際の処理
    /// </summary>
    public void LackOfMana()
    {
        _lackOfMana.Activate();
    }


    void BackToMap()
    {
        GameManager.Instance.PlayerHp = _playerMonsterBaseList[0].CurrentHp;
        SceneManager.LoadScene("FieldMap");
    }
}

