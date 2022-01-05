﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class CardBattleManager : MonoBehaviour
{
    Sequence sequence;
    public static StageData StageData;
    [SerializeField] StageData _stageDataForDebug;
    [SerializeField] Deck _playerDeck;
    [SerializeField] Deck _enemyDeck;
    public enum Phase
    {
        start = 0,
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
    [SerializeField] Phase _phase = Phase.start;
    SkillBase _defaultSkill;
    public bool KeyReception { get; set; } = false;
    int _maximumNumberOfMonster = 3;
    int _numberOfPossessionMonster = 0;
    int _turnOrder = 0;

    float _cameraMovementTime = 1.0f;
    [SerializeField] float _tweenSpeed = 1.0f;


    BattleMonsterTag.CharactorTag _thisTurnActor;
    BattleMonsterTag.CharactorTag _thisTurnTarget;
    MonsterBase _thisTurnActorMonsterBase;
    MonsterBase _thisTurnTargetMonsterBase;


    List<BattleMonsterTag.CharactorTag> _turnListTurnCharactor = new List<BattleMonsterTag.CharactorTag>();
    List<MonsterBase> _turnOrderListMonsterBase = new List<MonsterBase>();

    /// <summary>
    /// 攻撃待機しているキャラクター（同時に攻撃順が回った際に使う）
    /// </summary>
    List<MonsterBase> _attackWaitingList;

    //スキル
    SkillBase _useSkill;
    [SerializeField] SkillBase _guardSkill;

    /*
     * マネージャー
     */
    [SerializeField] SoundManager _soundManager;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] SkillManager _skillManager;
    [SerializeField] Hand _hand;
    [SerializeField] BattleCamera _battleCamera;
    [SerializeField] SEManager _seManager;
    [SerializeField] TargetView _targetView;
    [SerializeField] EffectManager _effectManager;
    [SerializeField] SkillView _skillView;
    [SerializeField] HpGauge _enemyStatusPrefab;
    [SerializeField] List<HpGauge> _playerHpList = new List<HpGauge>();
    [SerializeField] List<HpGauge> _playerHpListInWorld = new List<HpGauge>();
    [SerializeField] List<HpGauge> _enemyHpListInWorld = new List<HpGauge>();
    [SerializeField] TurnDisplay _turnDisplay;
    [SerializeField] IconManager _iconManager;
    [SerializeField] DamageCalculator _damageCalculator;

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
    [SerializeField] BattleMessage _battleMessage;
    DamageView _damageView;
    [SerializeField] DamageView _damageViewPrefab;
    [SerializeField] StatusView2[] _statusListPlayerSide = new StatusView2[3];
    Camera _cameraComponent;
    [SerializeField] Color _damageWeakColor;
    [SerializeField] Color _damageResistColor;
    [SerializeField] Fade _fade;
    [SerializeField] GameObject _gameVictoryObjects;
    [SerializeField] GameObject _gameLoseObjects;
    [SerializeField] GameObject _nextButton;
    [SerializeField] CanvasGroup _startAnimationCanvasGroup;
    [SerializeField] HpGauge _playerHpGauge;
    [SerializeField] EnemyAi _enemyAi;
    [SerializeField]

    UnityEngine.Random _random = new UnityEngine.Random();
    delegate void Func();

    public TrashDelegate Trash;


    void Start()
    {
        Application.targetFrameRate = 60;

        //初期化

        _turnOrderListMonsterBase = new List<MonsterBase>();
        _cameraComponent = _battleCamera.GetComponent<Camera>();
        MonsterBase _playerMonster = _playerMonsterBaseList[0];
        _playerHpGauge.Setup(_playerMonster.MonsterName, _playerMonster.MaxHp, _playerMonster.CurrentHp, _cameraComponent, null);
        //ステージデータ読み込み
        if (StageData != null)
        {
            for (int i = 0; i < StageData.EnemyData.Count; i++)
            {
                SetEnemyMonsterBase(_enemyMonsterObjectsList[i], i, _enemyMonsterPositionList[i], _enemyMonsterBaseList[i]);
            }
        }
        else
        {
            Debug.LogWarning("ステージデータがありませんでした.デバッグ用のステージデータを読み込みます");
            StageData = _stageDataForDebug;
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
        if (MonsterManager.PartyMonsterList == null)
        {
            monsterManager.SetDebugParty();
        }


        //一度ゲームオブジェクトのデッキをつくる
        List<CardObject> playerObjectDeck = new List<CardObject>();
        for (int i = 0; i < _playerMonster.CardDatas.Count; i++)
        {
            CardObject tempCard = Instantiate(_cardObjectPrefab);
            tempCard.CardData = _playerMonster.CardDatas[i];
            //画面外で保存
            tempCard.transform.position = new Vector3(-200, -500, -1000);
            tempCard.gameObject.transform.SetParent(_playerDeck.transform);
            playerObjectDeck.Add(tempCard);
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
            tempCard.CardData = _enemyMonsterBaseList[0].CardDatas[i];
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

    public void WriteMessage(string message)
    {
        _battleMessage.UpdateMessage(message);
    }

    void PhaseStart()
    {
        sequence = DOTween.Sequence();
        sequence
        .Append(_startAnimationCanvasGroup.DOFade(1.0f, 1.0f))
        .AppendInterval(2.0f)
        .AppendCallback(() => _soundManager.PlaySe(SoundManager.SeList.BattleStart))
        .Append(_startAnimationCanvasGroup.gameObject.transform.DOScale(10.0f, 1.0f))
        .Insert(3.0f, _startAnimationCanvasGroup.DOFade(0.0f, 0.5f))
        .InsertCallback(3.0f, () =>
        {
            //フェードアウト
            if (_fade.isActiveAndEnabled == true)
            {
                _fade.FadeOut(1.0f, () => PhasePreparation());
            }
            else
            {
                PhasePreparation();
            }
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


        List<MonsterBase> allMonsterBaseList = new List<MonsterBase>();
        allMonsterBaseList.AddRange(_playerMonsterBaseList);
        allMonsterBaseList.AddRange(_enemyMonsterBaseList);

        _turnOrder = 0;
        _phase = Phase.Wait;
        StartCoroutine(PhaseDraw());
    }

    /// <summary>
    /// ドローするフェイズ
    /// </summary>
    /// <returns></returns>
    IEnumerator PhaseDraw()
    {
        List<StatusEffectBase> statusList = _playerMonsterBaseList[0].StatusEffectList;
        for (int i = 0; i < statusList.Count; i++)
        {
            if (statusList[i].Count > 0)
            {
                Debug.Log(statusList[i].Name + "のステータスカウントは" + statusList[i].Count);
                statusList[i].Count--;
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
        if (_playerMonsterBaseList[0].StatusEffectList.Count > 0 && _enemyMonsterBaseList[0].StatusEffectList.Count > 0)
        {
            if (_playerMonsterBaseList[0].StatusEffectList[0] == _enemyMonsterBaseList[0].StatusEffectList[0])
            {
                Debug.Log("一緒です");
            }
        }
   
        int numberOfDraw = 3;
        for (int i = 0; i < numberOfDraw; i++)
        {
            yield return DrawCard();
            yield return Wait(0.3f);
        }
    }

    IEnumerator Wait(float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
    }

    public void PhaseEnemyView()
    {
        sequence.AppendInterval(1.0f)
                .AppendCallback(() =>
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
                    _turnOrder = 0;
                    _phase = Phase.Wait;
                });
    }

    public void PhaseEnemyStart()
    {
        StartCoroutine(PhaseEnemyTurn());
    }

    IEnumerator PhaseEnemyTurn()
    {
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
                        //ダメージ処理
                        DamageView damageView = Instantiate(_damageViewPrefab, _playerMonstersPositionList[0].transform.position, Quaternion.identity);
                        damageView.Setup(damage, "", Color.white, _cameraComponent);
                        damageView.transform.position += new Vector3(0, 2, -3);
                        damageView.Activate();
                        _playerHpGauge.UpdateStatus(_playerMonsterBaseList[0].CurrentHp);

                        break;
                    case SpellType.Guard:


                        break;
                    case SpellType.Buff:

                        StatusEffectBase status = new StatusEffectBase();
                        status.Name = partOfSpell.Status.Name;
                        status.Id = partOfSpell.Status.Id;
                        status.Icon = partOfSpell.Status.Icon;
                        bool alreadyHave = false;
                        foreach (var tempStatus in _enemyMonsterBaseList[0].StatusEffectList)
                        {
                            if (status == tempStatus)
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
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1);
        }

        yield return PhaseDraw();
    }



    void PhaseTarget()
    {
        _phase = Phase.Target;
        List<bool> targetList = new List<bool>();
        for (int i = 0; i < _maximumNumberOfMonster; i++)
        {
            if (_enemyMonsterBaseList[i])
            {
                targetList.Add(CheckIfAlived(_enemyMonsterBaseList[i]));
            }
        }
        _targetView.TargetPreparation(TargetView.TargetType.EnemySide, targetList.ToArray(), SetTarget);
    }

    public void PhaseDead()
    {
        switch (_thisTurnTarget)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1);
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2);
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3);
                break;
            default:
                break;
        }
        _thisTurnTargetMonsterBase.AfterDead = PhaseDead2;
        _thisTurnTargetMonsterBase.CheckEndOfDeadAnimation();
        _thisTurnTargetMonsterBase.MotionDead();
    }

    public void PhaseDead2()
    {
        for (int i = 0; i < _attackWaitingList.Count; i++)
        {
            if (_attackWaitingList[i] == _thisTurnTargetMonsterBase)
            {
                _attackWaitingList.Remove(_thisTurnTargetMonsterBase);
            }
        }

        for (int i = 0; i < _statusListPlayerSide.Length; i++)
        {
            _statusListPlayerSide[i].ChangeState(false);
        }

        //全滅チェック
        BattleMonsterTag.CharactorTag targetTag = _thisTurnTargetMonsterBase.CharactorTag;
        if (targetTag == BattleMonsterTag.CharactorTag.Enemy1 || targetTag == BattleMonsterTag.CharactorTag.Enemy2 || targetTag == BattleMonsterTag.CharactorTag.Enemy3)
        {
            bool allDeadFlag = true;
            for (int i = 0; i < _enemyMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(_enemyMonsterBaseList[i]))
                {
                    allDeadFlag = false;
                    break;
                }
            }
            if (allDeadFlag == true)
            {
                VictoryProduction();
                return;
            }
        }

        if (targetTag == BattleMonsterTag.CharactorTag.Player1 || targetTag == BattleMonsterTag.CharactorTag.Player2 || targetTag == BattleMonsterTag.CharactorTag.Player3)
        {
            bool allDeadFlag = true;
            for (int i = 0; i < _playerMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(_playerMonsterBaseList[i]))
                {
                    allDeadFlag = false;
                    break;
                }
            }
            if (allDeadFlag == true)
            {
                LoseProduction();
                return;
            }

        }

        if (_attackWaitingList.Count == 0)
        {
            _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
            _attackWaitingList = null;
            _phase = Phase.Wait;
        }
        else
        {
            _phase = Phase.Wait;
        }
    }

    /// <summary>
    /// 戦闘終了時の処理
    /// </summary>
    /// <param name="phase"></param>
    public void PhaseEndGame(Phase phase)
    {
        if (phase == Phase.Win)
        {

            _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
        }
    }

    /// <summary>
    /// カードを使用する
    /// </summary>
    public void PlayCard(CardData card)
    {
        StartCoroutine(PlayCardCorotine(card));
    }

    public IEnumerator PlayCardCorotine(CardData card)
    {
        for (int i = 0; i < card.CardSpellBases.Count; i++)
        {
            CardSpellBase spell = card.CardSpellBases[i];
            switch (spell.Type)
            {
                case SpellType.Attack:
                    AttackCoroutine(spell);
                    break;
                case SpellType.Guard:

                    break;
                case SpellType.Buff:

                    StatusEffectBase status = new StatusEffectBase();
                    status.Name = spell.Status.Name;
                    status.Id = spell.Status.Id;
                    status.Icon = spell.Status.Icon;
                    bool alreadyHave = false;
                    foreach (var tempStatus in _playerMonsterBaseList[0].StatusEffectList)
                    {
                        if (status == tempStatus)
                        {
                            tempStatus.Count += spell.EffectValue;
                            alreadyHave = true;
                            break;
                        }
                    }
                    if (alreadyHave == false)
                    {
                        status.Count = spell.EffectValue;
                        _playerMonsterBaseList[0].StatusEffectList.Add(status);
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

            yield return new WaitForSeconds(0.3f);
        }


    }

    void AttackCoroutine(CardSpellBase spell)
    {

        int damage = _damageCalculator.Calculate(_playerMonsterBaseList[0], _enemyMonsterBaseList[0], spell);

        _enemyMonsterBaseList[0].TakeDamage(damage);

        //アニメーションの再生

        //ダメージ処理
        DamageView damageView = Instantiate(_damageViewPrefab, _enemyMonsterPositionList[0].transform.position, Quaternion.identity);
        damageView.Setup(damage, "", Color.white, _cameraComponent);
        damageView.transform.position += new Vector3(0, 1, -10);
        damageView.Activate();

        _enemyHpListInWorld[0].UpdateStatus(_enemyMonsterBaseList[0].CurrentHp);
    }

    public void TurnEnd()
    {

    }

    /// <summary>
    /// 味方モンスターデータの設定
    /// </summary>
    /// <param name="playerObject">生成した味方オブジェクトの保存先</param>
    /// <param name="playerNumber">味方番号</param>
    /// <param name="playerPos">味方のオブジェクトの配置箇所</param>
    /// <param name="playerMonsterBase">味方の情報</param>
    void SetPlayerMonsterBase(GameObject playerObject, int playerNumber, Transform playerPos, MonsterBase playerMonsterBase)
    {
        //味方情報の更新
        playerObject = Instantiate(monsterManager.GetMonsterPrefab(_playerMonsterBaseList[playerNumber].Id));
        //複製したモンスターオブジェクトの親に、既に存在しているポジションオブジェクトを設定
        playerObject.transform.SetParent(playerPos);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerMonsterBase = playerObject.GetComponent<MonsterBase>();
        playerMonsterBase.CharactorTag = (BattleMonsterTag.CharactorTag)playerNumber;
        _playerMonsterBaseList[playerNumber] = playerMonsterBase;

        //味方ステータスの表示
        HpGauge newHpView = Instantiate(_enemyStatusPrefab);
        newHpView.transform.SetParent(playerPos);
        newHpView.transform.position = playerPos.position + new Vector3(0, 2.5f, 0);
        _playerHpListInWorld.Add(newHpView);

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
        enemyObject = Instantiate(monsterManager.GetMonsterPrefab(StageData.EnemyData[enemyNumber].Id), enemyPos.transform.position, Quaternion.identity);

        Vector3 tempDir = enemyObject.transform.localEulerAngles;
        tempDir.y += 180;
        enemyObject.transform.localEulerAngles = tempDir;
        enemyObject.transform.SetParent(enemyPos);
        enemyObject.transform.localPosition = new Vector3(0, 0, 0);
        enemyMonsterBase = enemyObject.GetComponent<MonsterBase>();
        enemyMonsterBase.CharactorTag = (BattleMonsterTag.CharactorTag)enemyNumber + 10;
        _enemyMonsterBaseList[enemyNumber] = enemyMonsterBase;
        //敵ステータスの表示
        HpGauge newHpView = Instantiate(_enemyStatusPrefab);
        newHpView.Setup(enemyMonsterBase.MonsterName, enemyMonsterBase.MaxHp, enemyMonsterBase.CurrentHp, _cameraComponent, null);
        newHpView.transform.SetParent(enemyPos);
        newHpView.transform.position = enemyPos.position + new Vector3(10, 2.5f, 0);
        newHpView.transform.localEulerAngles = new Vector3(0, 180, 0);
        _enemyHpListInWorld.Add(newHpView);
    }

    /// <summary>
    /// 対象モンスターが生きているか確認
    /// </summary>
    /// <param name="monster">確認する対象モンスター</param>
    /// <returns>生きている = true, 死んでいる = false</returns>
    bool CheckIfAlived(MonsterBase monster)
    {
        if (!monster)
        {
            return false;
        }
        if (monster.CurrentHp > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetUseSkill(SkillBase skill)
    {
        _useSkill = skill;
        if (_useSkill.GetSkillType() == SkillBase.SkillType.Guard || _useSkill.GetSkillType() == SkillBase.SkillType.Charge) //防御,チャージ時
        {
            _thisTurnTarget = _thisTurnActor;
            _thisTurnTargetMonsterBase = _thisTurnActorMonsterBase;
            return;
        }

        PhaseTarget();
    }

    /// <summary>
    /// ターゲットの設定
    /// </summary>
    /// <param name="charactor"></param>
    public void SetTarget(BattleMonsterTag.CharactorTag charactor)
    {
        _thisTurnTarget = charactor;
        switch (_thisTurnTarget)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                _thisTurnTargetMonsterBase = _playerMonsterBaseList[0];
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                _thisTurnTargetMonsterBase = _playerMonsterBaseList[1];
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                _thisTurnTargetMonsterBase = _playerMonsterBaseList[2];
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                _thisTurnTargetMonsterBase = _enemyMonsterBaseList[0];
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                _thisTurnTargetMonsterBase = _enemyMonsterBaseList[1];
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                _thisTurnTargetMonsterBase = _enemyMonsterBaseList[2];
                break;
            default:
                break;
        }
        _phase = Phase.Attack;
    }

    /// <summary>
    /// 勝利演出
    /// </summary>
    void VictoryProduction()
    {
        _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
        _gameVictoryObjects.SetActive(true);
        sequence = DOTween.Sequence();
        sequence.Append(_gameVictoryObjects.transform.DOScale(Vector3.one, _tweenSpeed))
                .AppendCallback(WaitingForGameEnd);

    }

    /// <summary>
    /// 敗北演出
    /// </summary>
    void LoseProduction()
    {
        _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
        _gameLoseObjects.SetActive(true);
        sequence = DOTween.Sequence();
        sequence.Append(_gameLoseObjects.transform.DOScale(Vector3.one, _tweenSpeed))
                .AppendCallback(WaitingForGameEnd);
    }

    void WaitingForGameEnd()
    {
        _nextButton.SetActive(true);
        _nextButton.transform.DOScale(Vector3.one, _tweenSpeed / 2);
        InputManager.ResetInputSettings();
        InputManager.InputEnter = GameEndProcess;
    }


    void GameEndProcess()
    {
        InputManager.ResetInputSettings();
        InputManager.setupCompleted = false;
        if (_fade.isActiveAndEnabled == true)
        {
            _soundManager.StopBgm();
            _fade.FadeIn(1.0f, BackToFrontScene);
        }
        else
        {
            _soundManager.StopBgm();
            BackToFrontScene();
        }

    }

    void BackToFrontScene()
    {
        SceneManager.LoadScene("Front");
    }
}

