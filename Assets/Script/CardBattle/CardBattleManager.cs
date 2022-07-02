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

//プレイヤーのどちら側か
public enum Side
{
    Player,
    Enemy,
}

public class CardBattleManager : MonoBehaviour
{
    [SerializeField] int _debugPlayerHp;

    /// <summary>
    /// Dotween用のシークエンス
    /// </summary>
    Sequence sequence;
    /// <summary>
    /// 戦闘で参照するステージデータ
    /// </summary>
    StageData _stageData;
    /// <summary>
    /// _stageDataがnullだった場合に使用するデバッグ用ステージデータ
    /// </summary>
    [SerializeField] StageData _stageDataForDebug;
    /// <summary>
    /// ゲームのプレイヤー側のデッキクラス
    /// </summary>
    [SerializeField] Deck _playerDeck;
    /// <summary>
    /// Enemy側のデッキクラス
    /// </summary>
    [SerializeField] Deck _enemyDeck;

    [SerializeField] int _numberOfDrawCard;
    /// <summary>
    /// バトルエフェクトの再生をするクラス
    /// </summary>
    [SerializeField] EffectAnimation _effectAnim;



    /// <summary>
    ///  デッキを画面外に置く際のポジション
    /// </summary>
    [SerializeField] Vector3 _deckPos;
    /// <summary>
    /// ダメージ表示の高さ
    /// </summary>
    [SerializeField] Vector3 _damageViewPos;

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

    /// <summary>
    /// デバッグ用のモンスタークラスのリスト
    /// </summary>
    [SerializeField] List<MonsterBase> _debugMonsters;
    /// <summary>
    /// 現在がどの段階かを示す変数
    /// </summary>
    [SerializeField] Phase _phase = Phase.Start;
    /// <summary>
    /// アニメーションの表示速度
    /// </summary>
    [SerializeField] float _tweenSpeed = 1.0f;
    /// <summary>
    /// 今処理中のカード処理が存在するかどうか
    /// </summary>
    [SerializeField] bool _isPlayingCard = false;
    /// <summary>
    /// スキル
    /// </summary>
    [SerializeField] StatusEffectBase _guardStatusEffect;

    /*
     * マネージャー
     */
    /// <summary>
    /// 音関連の処理を行うクラス
    /// </summary>
    [SerializeField] SoundManager _soundManager;
    /// <summary>
    /// モンスターの処理を行うクラス
    /// </summary>
    [SerializeField] MonsterManager monsterManager;
    /// <summary>
    /// 手札クラス
    /// </summary>
    [SerializeField] Hand _hand;
    /// <summary>
    /// 体力表示のゲージのプレハブ
    /// </summary>
    [SerializeField] HpGauge _enemyStatusPrefab;
    /// <summary>
    /// 戦闘ダメージ計算のクラス
    /// </summary>
    [SerializeField] DamageCalculator _damageCalculator;
    /// <summary>
    /// プレイヤーに付与されている状態表示のクラス
    /// </summary>
    [SerializeField] StatusIconView _playerStatusIconView;
    /// <summary>
    /// 敵に付与されている状態表示を管理するクラス
    /// </summary>
    [SerializeField] StatusIconView _enemyStatusIconView;
    /// <summary>
    /// 戦闘結果表示を管理するクラス
    /// </summary>
    [SerializeField] ResultManager _resultManager;
    /// <summary>
    /// ゲームオーバー画面表示のクラス
    /// </summary>
    [SerializeField] GameResultCanvas _gameResultManager;

    /*
     * オブジェクト
     */
    /// <summary>
    /// カードオブジェクトのプレハブ
    /// </summary>
    [SerializeField] CardObject _cardObjectPrefab;
    /// <summary>
    /// 実体化した敵のオブジェクトのリスト
    /// </summary>
    [SerializeField] List<GameObject> _enemyMonsterObjectsList = new List<GameObject>();
    /// <summary>
    /// 味方モンスターの表示位置
    /// </summary>
    [SerializeField] List<Transform> _playerMonstersPositionList = new List<Transform>();
    /// <summary>
    /// 敵モンスターの表示位置
    /// </summary>
    [SerializeField] List<Transform> _enemyMonsterPositionList = new List<Transform>();
    /// <summary>
    /// 敵モンスターデータのリスト
    /// </summary>
    [SerializeField] List<MonsterBase> _enemyMonsterBaseList = new List<MonsterBase>();

    /// <summary>
    /// 被ダメージ表示のプレハブ
    /// </summary>
    [SerializeField] DamageView _damageViewPrefab;
    /// <summary>
    /// カメラクラス
    /// </summary>
    Camera _cameraComponent;
    /// <summary>
    /// フェード機能のクラス
    /// </summary>
    [SerializeField] Fade _fade;
    /// <summary>
    /// 勝利時に表示するオブジェクト
    /// </summary>
    [SerializeField] GameObject _gameVictoryObjects;
    /// <summary>
    /// 敗北時に表示するオブジェクト
    /// </summary>
    [SerializeField] GameObject _gameLoseObjects;
    /// <summary>
    /// 次へ進むボタン
    /// </summary>
    [SerializeField] GameObject _nextButton;
    /// <summary>
    /// 戦闘開始時のアニメーションのキャンバスグループ　透過の調整で使う
    /// </summary>
    [SerializeField] CanvasGroup _startAnimationCanvasGroup;
    /// <summary>
    /// プレイヤーの体力ゲージ
    /// </summary>
    [SerializeField] HpGauge _playerHpGauge;
    /// <summary>
    /// 敵の体力ゲージ
    /// </summary>
    [SerializeField] HpGauge _enemyHpGauge;
    /// <summary>
    /// 敵のAI
    /// </summary>
    [SerializeField] EnemyAi _enemyAi;
    /// <summary>
    /// 戦闘結果キャンバス
    /// </summary>
    [SerializeField] Canvas _resultCanvas;
    /// <summary>
    /// ゲーム結果キャンバス
    /// </summary>
    [SerializeField] Canvas _gameResultCanvas;
    /// <summary>
    /// ターン終了ボタン
    /// </summary>
    [SerializeField] GameObject _turnEndButton;
    /// <summary>
    /// マナが足りない表示オブジェクト
    /// </summary>
    [SerializeField] MessageUi _lackOfMana;

    UnityEngine.Random _random = new UnityEngine.Random();
    delegate void Func();

    public TrashDelegate Trash;

    void Start()
    {
        Application.targetFrameRate = 60;
        //初期化
        //戦闘開始回数を増やす
        GameManager.Instance.BattleCount++;

        //もしプレイヤーのHPが0の場合、デバッグ用データを代入する
        if (GameManager.Instance.PlayerMaxHp <= 0)
        {
            Debug.Log("デバッグ用のHPを使用します");
            GameManager.Instance.PlayerMaxHp = _debugPlayerHp;
            GameManager.Instance.PlayerCurrentHp = _debugPlayerHp;

        }

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
        //お互いのHPゲージの表示の設定をする
        _playerHpGauge.SetData(GameManager.Instance.PlayerName, GameManager.Instance.PlayerMaxHp);
        _enemyHpGauge.SetData(_enemyMonsterBaseList[0].MonsterName, _enemyMonsterBaseList[0].MaxHp);
        _playerHpGauge.UpdateHp();
        _enemyHpGauge.UpdateHp(_enemyMonsterBaseList[0]);
        _playerHpGauge.UpdateMp();

        //一度ゲームオブジェクトのデッキをつくる
        List<MonsterBase>monsterParty = GameManager.Instance.MonsterParty;

        List<CardObject> playerObjectDeck = new List<CardObject>();
        for (int i = 0; i < monsterParty.Count; i++)
        {
            for (int k = 0; k < monsterParty[i].CardDatas.Count; k++)
            {
                CardObject tempCard = Instantiate(_cardObjectPrefab);
                tempCard.InBattle = true;
                tempCard.Data = monsterParty[i].CardDatas[k];
                tempCard.Check = CheckIfCanUseCardPlayerSide;
                //画面外で保存
                tempCard.transform.position = _deckPos;
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
            tempCard.transform.position = _deckPos;
            tempCard.gameObject.transform.SetParent(_enemyDeck.transform);
            enemyObjectDeck.Add(tempCard);
        }
        _enemyDeck.Setup(enemyObjectDeck);
        _enemyAi.Deck = _enemyDeck;
        //カード使用時の処理の追加
        _enemyAi.Hand.Setup(PlayCard, _enemyDeck.Trash);



        GameManager.Instance.PlayerMaxMp = monsterParty.Sum(x => x.MaxMp);
        GameManager.Instance.PlayerCurrentMp = GameManager.Instance.PlayerMaxMp;
        Debug.Log("MaxMp = " + GameManager.Instance.PlayerMaxMp);
        GameManager.Instance.PlayerStatusEffectList = new List<StatusEffectBase>();
        PhaseStart();
    }

    public void Update()
    {
        //デバッグ用勝利コマンド
        if (Input.GetKeyDown(KeyCode.W))
        {
            PhaseWin();
        }
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
        GameManager.Instance.PlayerCurrentMp = GameManager.Instance.PlayerMaxMp;
    
        List<StatusEffectBase> statusList = GameManager.Instance.PlayerStatusEffectList;
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
        _playerStatusIconView.UpdateView(GameManager.Instance.PlayerStatusEffectList);


        int numberOfDraw = 3;
        for (int i = 0; i < numberOfDraw; i++)
        {
            yield return DrawCard();
            yield return Wait(0.3f);
        }
        UpdateMana();
    }

    /// <summary>
    /// マナ表示の更新
    /// </summary>
    void UpdateMana()
    {
        _playerHpGauge.UpdateMp();

        if (_phase == Phase.Player)
        {
            //手札全体の中で最小コストのカードを探し、そのカードのコストを一時的に保存する
            int minimumCostInHand = Int32.MaxValue;
            foreach (var card in _hand.CardList)
            {
                if (card.Data.Cost < minimumCostInHand)
                {
                    minimumCostInHand = card.Data.Cost;
                }
            }

            if (_hand.CardList.Count == 0)
            {
                _turnEndButton.SetActive(false);
            }
            else if (GameManager.Instance.PlayerCurrentMp <= 0 || minimumCostInHand > GameManager.Instance.PlayerCurrentMp)
            {
                _turnEndButton.SetActive(false);
            }
            else
            {
                _turnEndButton.SetActive(true);
            }
        }

        _enemyHpGauge.UpdateMp(_enemyMonsterBaseList[0]);
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
        _turnEndButton.SetActive(true);
        if (_phase == Phase.Player && _isPlayingCard == false)
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
        _enemyAi.UpdateHandCountView();

        //敵はMP2倍
        _enemyMonsterBaseList[0].CurrentMp = _enemyMonsterBaseList[0].MaxMp * 2;
        List<CardData> combo = _enemyAi.Think();
        for (int i = 0; i < combo.Count; i++)
        {
            if (GameManager.Instance.PlayerCurrentHp <= 0 || _enemyMonsterBaseList[0].CurrentHp <= 0)
            {
                yield break;
            }
            DamageView spellNameView = Instantiate(_damageViewPrefab, _enemyMonsterPositionList[0].transform.position, Quaternion.identity);
            spellNameView.Setup(0, combo[i].CardName, Color.white, _cameraComponent);
            spellNameView.transform.position += _damageViewPos;
            spellNameView.Activate();
            Debug.Log("敵コンボ数：" + combo.Count);
            Debug.Log($"敵は{combo[i].CardName}をプレイした");

            for (int k = 0; k < combo[i].CardSpellBases.Count; k++)
            {

                CardSpellBase partOfSpell = combo[i].CardSpellBases[k];
                if (partOfSpell.SpellSound != null)
                {
                    _soundManager.PlaySe(partOfSpell.SpellSound);
                }
                else
                {
                    Debug.Log("Not SE");
                }

                switch (partOfSpell.Type)
                {
                    case SpellType.Attack:

                        int damage = _damageCalculator.Calculate(_enemyMonsterBaseList[0].StatusEffectList, GameManager.Instance.PlayerStatusEffectList, partOfSpell);

                        GameManager.Instance.PlayerCurrentHp -= damage;
                        _playerStatusIconView.IconPopup(_guardStatusEffect);
                        //ダメージ処理
                        DamageView damageView = Instantiate(_damageViewPrefab, _playerMonstersPositionList[0].transform.position, Quaternion.identity);
                        damageView.transform.position += new Vector3(0, 2, -3);
                        damageView.Setup(damage, "", Color.white, _cameraComponent);
                        damageView.Activate();
                        _playerHpGauge.UpdateHp();
                        _playerHpGauge.gameObject.transform.DOShakePosition(0.5f, 0.5f, 100);
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
                            foreach (var tempStatus in GameManager.Instance.PlayerStatusEffectList)
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
                                GameManager.Instance.PlayerStatusEffectList.Add(status);
                            }
                            _playerStatusIconView.UpdateView(GameManager.Instance.PlayerStatusEffectList);
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
                            _enemyStatusIconView.UpdateView(_enemyMonsterBaseList[0].StatusEffectList);
                        }

                        break;
                    case SpellType.Debuff:
                        break;
                    case SpellType.Draw:
                        break;
                    case SpellType.DisCard:
                        break;
                    case SpellType.Heal:

                        _enemyMonsterBaseList[0].CurrentHp += partOfSpell.EffectValue;
                        if (_enemyMonsterBaseList[0].CurrentHp > _enemyMonsterBaseList[0].MaxHp)
                        {
                            _enemyMonsterBaseList[0].CurrentHp = _enemyMonsterBaseList[0].MaxHp;
                        }
                        _enemyHpGauge.UpdateHp(_enemyMonsterBaseList[0]);

                        break;
                    default:
                        break;
                }
                _enemyStatusIconView.UpdateView(_enemyMonsterBaseList[0].StatusEffectList);

                _enemyAi.Hand.RemoveCard(combo[i]);
                _enemyAi.UpdateHandCountView();
                UpdateMana();
                CheckIfDead();
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        _enemyAi.UpdateHandCountView();
        if (_phase == Phase.Player)
        {
            yield return null;
        }
        yield return PhaseDraw();
    }
    public void CheckIfDead()
    {
        if (GameManager.Instance.PlayerCurrentHp <= 0)
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
        if (GameManager.Instance.PlayerCurrentMp >= card.Cost)
        {
            GameManager.Instance.PlayerCurrentMp -= card.Cost;
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
            _playerStatusIconView.UpdateView(GameManager.Instance.PlayerStatusEffectList);
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
            if (GameManager.Instance.PlayerCurrentHp <= 0 || _enemyMonsterBaseList[0].CurrentHp <= 0)
            {
                yield break;
            }
            _isPlayingCard = true;
            CardSpellBase partOfSpell = card.CardSpellBases[i];
            if (partOfSpell.SpellSound != null)
            {
                _soundManager.PlaySe(partOfSpell.SpellSound);
            }
            else
            {
                Debug.Log("Not SE");
            }
            switch (partOfSpell.Type)
            {
                case SpellType.Attack:
                    _effectAnim.PlayAnimation(0);
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
                        _enemyStatusIconView.UpdateView(_enemyMonsterBaseList[0].StatusEffectList);
                    }
                    else if (partOfSpell.Target == SpellTarget.Player)
                    {
                        foreach (var tempStatus in GameManager.Instance.PlayerStatusEffectList)
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
                            GameManager.Instance.PlayerStatusEffectList.Add(status);
                        }
                        _playerStatusIconView.UpdateView(GameManager.Instance.PlayerStatusEffectList);
                    }

                    break;
                case SpellType.Debuff:
                    break;
                case SpellType.Draw:
                    break;
                case SpellType.DisCard:
                    break;
                case SpellType.Heal:

                    GameManager.Instance.PlayerCurrentHp += partOfSpell.EffectValue;
                    if (GameManager.Instance.PlayerCurrentHp > GameManager.Instance.PlayerMaxHp)
                    {
                        GameManager.Instance.PlayerCurrentHp = GameManager.Instance.PlayerMaxHp;
                    }
                    _playerHpGauge.UpdateHp();

                    break;

                default:
                    break;
            }
            _enemyHpGauge.UpdateHp(_enemyMonsterBaseList[0]);
            yield return new WaitForSeconds(0.5f);
            _enemyStatusIconView.UpdateView(_enemyMonsterBaseList[0].StatusEffectList);
            CheckIfDead();
            UpdateMana();
            _isPlayingCard = false;
        }
    }

    /// <summary>
    /// 攻撃処理のコルーチン
    /// </summary>
    /// <param name="spell"></param>
    void AttackCoroutine(CardSpellBase spell)
    {

        int damage = _damageCalculator.Calculate(GameManager.Instance.PlayerStatusEffectList, _enemyMonsterBaseList[0].StatusEffectList, spell);

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
        _enemyHpGauge.UpdateHp(_enemyMonsterBaseList[0]);
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
        GameManager.Instance.PlayerMaxHp = GameManager.Instance.PlayerCurrentHp;
        GameManager.Instance.PlayerStatusEffectList = new List<StatusEffectBase>();
        SceneManager.LoadScene(GameManager.Instance.FieldMapName);
    }
}

