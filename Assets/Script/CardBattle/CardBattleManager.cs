using System;
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
     * マネージャー宣言
     */
    [SerializeField] SoundManager _soundManager;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] SkillManager _skillManager;
    [SerializeField] Hand _hand;
    [SerializeField] BattleCamera _battleCamera;
    [SerializeField] SEManager _seManager;
    [SerializeField] TargetView _targetView;
    [SerializeField] EffectManager _effectManager;
    [SerializeField] TurnManager _turnManager;
    [SerializeField] TimelineManager _timelineManager;
    MonsterSort _monsterSort;
    [SerializeField] SkillView _skillView;
    [SerializeField] DamageManager _damageManager;
    [SerializeField] HpGauge _enemyStatusPrefab;
    [SerializeField] List<HpGauge> _playerHpList = new List<HpGauge>();
    [SerializeField] List<HpGauge> _playerHpListInWorld = new List<HpGauge>();
    [SerializeField] List<HpGauge> _enemyHpListInWorld = new List<HpGauge>();
    [SerializeField] TurnDisplay _turnDisplay;
    [SerializeField] IconManager _iconManager;


    /*
     * オブジェクト宣言
     */
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

    UnityEngine.Random _random = new UnityEngine.Random();
    delegate void Func();


    void Start()
    {
        Application.targetFrameRate = 60;

        //初期化
        _turnOrderListMonsterBase = new List<MonsterBase>();
        _monsterSort = new MonsterSort();
        _cameraComponent = _battleCamera.GetComponent<Camera>();

        //カード使用時の処理の追加
        _hand.Setup(PlayCard);

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

                    //turnOrderListMonsterBase.Add(enemyMonsterBaseList[i]);
                }
            }
        }
        if (MonsterManager.PartyMonsterList == null)
        {
            monsterManager.SetDebugParty();

        }
        _playerMonsterBaseList = MonsterManager.PartyMonsterList;
        //味方情報のセット
        for (int i = 0; i < _playerMonsterObjectsList.Count; i++)
        {
            Debug.Log(_playerMonsterObjectsList[i] + "   " + _playerMonstersPositionList[i]);
            SetPlayerMonsterBase(_playerMonsterObjectsList[i], i, _playerMonstersPositionList[i], _playerMonsterBaseList[i]);
        }
        //マネージャーのセット
        _numberOfPossessionMonster = monsterManager.NumberOfPossessionMonster;
        _targetView.Setup(_playerMonsterBaseList, _enemyMonsterBaseList);
        _targetView.maximumNumberOfMonster = this._maximumNumberOfMonster;

        //デフォルトスキルの設定(通常攻撃に当たるスキル)
        _defaultSkill = _skillManager.GetDefaultSkill();

        //for (int i = 0; i < _statusListPlayerSide.Length; i++)
        //{
        //    _statusListPlayerSide[i].Setup(_playerMonsterBaseList[i]);
        //}

        //前の処理に戻るときの、戻り先設定
        _skillView.BackToPhaseForBattleManager = BackToBeforePhase;
        _targetView.BackToBeforePhaseForBattleManager = BackToBeforePhase;

        PhaseStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (_phase == Phase.Wait)
        {
            if (_attackWaitingList == null)
            {
                _attackWaitingList = _timelineManager.UpdateTimeline(true);
            }
            if (_attackWaitingList != null)
            {
                if (_attackWaitingList.Count > 1) //2体より多い場合はソートする
                {
                    _monsterSort.turnListMonsterBase = _attackWaitingList;
                    _monsterSort.QuickSort(0, _attackWaitingList.Count - 1);
                    _attackWaitingList = _monsterSort.turnListMonsterBase;
                }
                if (_attackWaitingList.Count >= 1)
                {
                    _thisTurnActorMonsterBase = _attackWaitingList[0];
                    _thisTurnActor = _attackWaitingList[0].charactorTag;
                    _timelineManager.MoveImageToFront(_thisTurnActorMonsterBase);

                    if ((int)_attackWaitingList[0].charactorTag >= 10) //敵ならば10番以降を使用しているためこれで判別する。
                    {
                        PhaseAI(_attackWaitingList[0]);
                    }
                    else //味方ならばコマンド選択の開始
                    {
                        _thisTurnActor = _attackWaitingList[0].charactorTag;

                        PhaseCommand();
                    }
                }
                else
                {
                    _attackWaitingList = null;
                }
            }
        }
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

    void PhasePreparation()
    {
        _phase = Phase.Preparation;

        _soundManager.PlayBgm();


        List<MonsterBase> allMonsterBaseList = new List<MonsterBase>();
        allMonsterBaseList.AddRange(_playerMonsterBaseList);
        allMonsterBaseList.AddRange(_enemyMonsterBaseList);

        _timelineManager.Setup(allMonsterBaseList);


        //敵全体をカメラで映す演出
        //battleCamera.AllEnemyView(() => PhaseEnemyView());
        _turnOrder = 0;
        _phase = Phase.Wait;
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

    void PhaseCommand()
    {
        _phase = Phase.Command;
        _turnDisplay.DisplayImage(TurnDisplay.DisplayPattern.PlayerTurn);
        switch (_thisTurnActor)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                //battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                _statusListPlayerSide[0].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                //battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                _statusListPlayerSide[1].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                //battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                _statusListPlayerSide[2].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                break;
            default:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
                break;
        }

        if (_thisTurnActorMonsterBase.status == MonsterBase.MonsterState.Guard)
        {
            _thisTurnActorMonsterBase.status = MonsterBase.MonsterState.Normal;
        }

        _phase = Phase.Command;
        //commandBoxManager.Setup(SetUseSkill);
        //commandBoxManager.PhaseSkill = PhaseChooseSkill;

        KeyReception = true;
    }

    public void PhaseChooseSkill()
    {
        _phase = Phase.ChooseSkill;
        _skillView.SetUseSkill = this.SetUseSkill;
        switch (_thisTurnActor)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                _statusListPlayerSide[0].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                _statusListPlayerSide[1].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                _statusListPlayerSide[2].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                break;
            default:
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
                break;
        }

        int actorNumber = (int)_thisTurnActor;
        _skillView.Setup(_thisTurnActorMonsterBase.GetSkillList());

        _skillView.Display(true);
        _skillView.SetInput();

        KeyReception = true;
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

    void PhaseAI(MonsterBase monster)
    {
        _phase = Phase.Enemy;
        //モンスターに使用スキルを考えさせる
        _useSkill = monster.ThinkOfASkill();
        _thisTurnActor = monster.charactorTag;

        if ((int)monster.charactorTag >= 10) //敵側のモンスターなら
        {
            int count = _playerMonsterBaseList.Count * 100;
            _turnDisplay.DisplayImage(TurnDisplay.DisplayPattern.EnemyTurn);
            int targetNumber = UnityEngine.Random.Range(0, _playerMonsterBaseList.Count);
            while (true)
            {
                if (CheckIfAlived(_playerMonsterBaseList[targetNumber]) == true)
                {
                    _thisTurnTarget = (BattleMonsterTag.CharactorTag)targetNumber;
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
                            break;
                        case BattleMonsterTag.CharactorTag.Enemy2:
                            break;
                        case BattleMonsterTag.CharactorTag.Enemy3:
                            break;
                        default:
                            break;
                    }
                    break;
                }
                count--;
                targetNumber++;
                if (targetNumber >= _playerMonsterBaseList.Count)
                {
                    targetNumber = 0;
                }
            }

            PhaseAttack1();
        }
    }

    void PhaseAttack1()
    {
        InputManager.ResetInputSettings();
        int actorNumber = (int)_thisTurnActor;

        WriteMessage(_useSkill.GetName());

        if (_useSkill.GetSkillType() == SkillBase.SkillType.Physical || _useSkill.GetSkillType() == SkillBase.SkillType.Magic)
        {
            if (actorNumber < 10)
            {
                if (actorNumber == 0)
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                }
                else if (actorNumber == 1)
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                }
                else if (actorNumber == 2)
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                }
                _playerMonsterBaseList[actorNumber].AfterAction = PhaseAttack2;
                _playerMonsterBaseList[actorNumber].CheckEndOfAnimation();
                _playerMonsterBaseList[actorNumber].MotionAttack();

            }
            else if (actorNumber >= 10)
            {
                if (actorNumber == 10)
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1);
                }
                else if (actorNumber == 11)
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2);
                }
                else if (actorNumber == 12)
                {
                    _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3);
                }

                _enemyMonsterBaseList[actorNumber - 10].AfterAction = PhaseAttack2;
                _enemyMonsterBaseList[actorNumber - 10].CheckEndOfAnimation();
                _enemyMonsterBaseList[actorNumber - 10].MotionAttack();
            }
        }
        else
        {
            PhaseAttack2();
        }

    }

    /// <summary>
    /// 被ダメキャラの方向を向いて、エフェクトを再生する
    /// </summary>
    void PhaseAttack2()
    {
        InputManager.ResetInputSettings();
        Vector3 damageGeneratePos = new Vector3();

        //配列を参照する際のターゲットの番号
        int targetNumber = (int)_thisTurnTarget;
        //エフェクト再生位置を表す変数
        Vector3 effectProducePos = new Vector3();
        //味方か敵かどちらの側に再生するかを表す変数(暫定的に敵側で初期化)
        EffectManager.DirectionToProduce direction = EffectManager.DirectionToProduce.EnemySide;
        if (targetNumber < 10)
        {
            if (targetNumber == 0)
            {
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
            }
            else if (targetNumber == 1)
            {
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
            }
            else if (targetNumber == 2)
            {
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
            }

            if (_useSkill.GetSkillType() == SkillBase.SkillType.Physical || _useSkill.GetSkillType() == SkillBase.SkillType.Magic)
            {
                _playerMonsterBaseList[targetNumber].MotionTakeDamege();
            }
            else
            {
                _playerMonsterBaseList[targetNumber].MotionAttack();
                if (_useSkill.GetSkillType() == SkillBase.SkillType.Guard) //ガード・防御時処理
                {
                    _playerMonsterBaseList[targetNumber].status = MonsterBase.MonsterState.Guard;
                }
                if (_useSkill.GetSkillType() == SkillBase.SkillType.Charge) //チャージ時処理
                {
                    _playerMonsterBaseList[targetNumber].status = MonsterBase.MonsterState.Charge;
                }
            }

            _playerMonsterBaseList[targetNumber].AfterAction = PhaseAfterAttack;
            _playerMonsterBaseList[targetNumber].CheckEndOfAnimation();
            effectProducePos = _playerMonsterBaseList[targetNumber].transform.position;
            effectProducePos.z += 1.0f;
            direction = EffectManager.DirectionToProduce.PlayerSide;

            effectProducePos = _playerMonsterBaseList[targetNumber].transform.position;

            damageGeneratePos = _playerMonsterBaseList[targetNumber].transform.position;
            damageGeneratePos += new Vector3(1f, 0, 1.5f);

        }
        else if (targetNumber >= 10)
        {
            if (targetNumber == 10)
            {
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1);
            }
            else if (targetNumber == 11)
            {
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2);
            }
            else if (targetNumber == 12)
            {
                _battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3);
            }
            if (_useSkill.GetSkillType() == SkillBase.SkillType.Physical || _useSkill.GetSkillType() == SkillBase.SkillType.Magic)
            {
                _enemyMonsterBaseList[targetNumber - 10].MotionTakeDamege();
            }
            else
            {
                _enemyMonsterBaseList[targetNumber - 10].MotionAttack();
            }

            _enemyMonsterBaseList[targetNumber - 10].AfterAction = PhaseAfterAttack;
            _enemyMonsterBaseList[targetNumber - 10].CheckEndOfAnimation();
            effectProducePos = _enemyMonsterBaseList[targetNumber - 10].transform.position;
            effectProducePos.z -= 1.0f;
            direction = EffectManager.DirectionToProduce.EnemySide;

            damageGeneratePos = _enemyMonsterBaseList[targetNumber - 10].transform.position;
            damageGeneratePos += new Vector3(1f, 0, -1.5f);
        }
        _effectManager.ProduceEffect(_useSkill.GetEffect(), effectProducePos, direction);


        if (_useSkill.GetDamage() > 0)
        {
            //パッシブスキルの整理
            List<PassiveSkillBase> pSkillListPlayerSide = new List<PassiveSkillBase>();
            List<PassiveSkillBase> pSkillListEnemySide = new List<PassiveSkillBase>();
            for (int i = 0; i < _playerMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(_playerMonsterBaseList[0]) == true)
                {
                    pSkillListPlayerSide.Add(_playerMonsterBaseList[i].GetPassiveSkill());
                }
                else
                {
                    pSkillListPlayerSide.Add(null);
                }
            }
            for (int i = 0; i < _enemyMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(_enemyMonsterBaseList[0]) == true)
                {
                    pSkillListEnemySide.Add(_enemyMonsterBaseList[i].GetPassiveSkill());
                }
                else
                {
                    pSkillListEnemySide.Add(null);
                }
            }


            //ダメージ計算とHP更新
            int damage = _damageManager.DamageCalculator(_thisTurnActorMonsterBase, _thisTurnTargetMonsterBase, _useSkill, pSkillListPlayerSide, pSkillListEnemySide);

            _thisTurnTargetMonsterBase.TakeDamage(damage);

            //ダメージ表示(ガードの場合は表示しない)
            if (_useSkill.GetSkillType() != SkillBase.SkillType.Guard)
            {
                string damageMessage = "";
                Color damageViewColor = Color.white;
                _damageView = Instantiate(_damageViewPrefab);
                if (Element.CheckAdvantage(_useSkill.GetElement(), _thisTurnTargetMonsterBase.GetElement()) > 1)
                {
                    damageViewColor = _damageWeakColor;
                }
                else if (Element.CheckAdvantage(_useSkill.GetElement(), _thisTurnTargetMonsterBase.GetElement()) < 1)
                {
                    damageViewColor = _damageResistColor;
                }
                _damageView.Setup(damage, damageMessage, damageViewColor, _cameraComponent);
            }
        }

        //味方ステータス表示の更新
        switch (_thisTurnTarget)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                if (_playerHpList[0])
                {
                    if (_playerHpList[0].isActive)
                    {
                        _playerHpList[0].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }
                if (_playerHpListInWorld[0])
                {
                    if (_playerHpListInWorld[0].isActive)
                    {
                        _playerHpListInWorld[0].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }

                break;
            case BattleMonsterTag.CharactorTag.Player2:
                if (_playerHpList.Count >= 2)
                {
                    if (_playerHpList[1])
                    {
                        if (_playerHpList[1].isActive)
                        {
                            _playerHpList[1].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                        }
                    }
                }
                if (_playerHpListInWorld[1])
                {
                    if (_playerHpListInWorld[1].isActive)
                    {
                        _playerHpListInWorld[1].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }

                break;
            case BattleMonsterTag.CharactorTag.Player3:
                if (_playerHpList.Count >= 3)
                {
                    if (_playerHpList[2])
                    {
                        if (_playerHpList[2].isActive)
                        {
                            _playerHpList[2].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                        }
                    }
                }
                if (_playerHpListInWorld[2])
                {
                    if (_playerHpListInWorld[2].isActive)
                    {
                        _playerHpListInWorld[2].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }

                break;


            case BattleMonsterTag.CharactorTag.Enemy1:
                _enemyHpListInWorld[0].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                _enemyHpListInWorld[1].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                _enemyHpListInWorld[2].UpdateStatus(_thisTurnTargetMonsterBase.GetCurrentHPValue());
                break;
            default:
                break;
        }

        if (_useSkill.GetDamage() > 0)
        {
            //高さ調整
            damageGeneratePos += new Vector3(0, 1f, 0);
            _damageView.transform.position = damageGeneratePos;
            //カメラの方向を向く    
            //damageView.transform.LookAt(battleCamera.transform);
            _damageView.transform.rotation = _battleCamera.transform.rotation;
            sequence = DOTween.Sequence();
            sequence.Append(_damageView.transform.DOJump((damageGeneratePos + new Vector3(0, 0.05f, 0)), 1, 1, 0.3f).SetEase(Ease.InBounce))
                    .Append(_damageView.transform.DOMove(_battleCamera.transform.position, 20f));

            _damageView.isActive = true;

        }

    }



    public void PhaseAfterAttack()
    {
        _phase = Phase.AfterAttack;

        _battleMessage.CloseMessage();
        _effectManager.DestroyObject();
        _turnDisplay.DisplayImage(TurnDisplay.DisplayPattern.None);
        sequence.Kill();
        if (_damageView)
        {
            Destroy(_damageView.gameObject);
        }
        Debug.Log("使用スキル：" + _useSkill);
        if (_thisTurnActorMonsterBase.status == MonsterBase.MonsterState.Charge && _useSkill.GetSkillType() != SkillBase.SkillType.Charge)
        {
            Debug.Log("チャージ解除");
            _thisTurnActorMonsterBase.status = MonsterBase.MonsterState.Normal;
        }



        if ((int)_thisTurnActor < 10)
        {
            _playerMonsterBaseList[(int)_thisTurnActor].coolTime = _useSkill.GetCoolTime();
        }
        else
        {
            _enemyMonsterBaseList[((int)_thisTurnActor) - 10].coolTime = _useSkill.GetCoolTime();
        }
        _attackWaitingList.RemoveAt(0);
        _timelineManager.UpdateTimeline(false);
        if (!CheckIfAlived(_thisTurnTargetMonsterBase))
        {
            PhaseDead();
            return;
        }

        for (int i = 0; i < _statusListPlayerSide.Length; i++)
        {
            _statusListPlayerSide[i].ChangeState(false);
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
        _timelineManager.DeleteMonster(_thisTurnTargetMonsterBase);
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
        BattleMonsterTag.CharactorTag targetTag = _thisTurnTargetMonsterBase.charactorTag;
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
    /// 前のフェイズに戻る処理
    /// </summary>
    public void BackToBeforePhase()
    {
        switch (_phase)
        {
            case Phase.Preparation:
                break;
            case Phase.Command:
                break;
            case Phase.ChooseSkill:

                PhaseCommand();

                break;
            case Phase.Target:

                if (_useSkill == _defaultSkill)
                {
                    PhaseCommand();
                }
                else
                {
                    PhaseChooseSkill();
                }

                break;
            case Phase.Attack:
                break;
            case Phase.Wait:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// スキルを使用する
    /// </summary>
    public void PlayCard(CardData card)
    {
        _useSkill = card._skill;

        //アニメーションの再生

        //ダメージ処理
        Debug.Log(" かず" + _enemyMonsterPositionList[0]);
        DamageView damageView = Instantiate(_damageViewPrefab, _enemyMonsterPositionList[0].transform.position, Quaternion.identity);
        damageView.Setup(10, "", Color.white, _cameraComponent);
        damageView.transform.position += new Vector3(0, 1, -10);
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
        playerObject = Instantiate(monsterManager.GetMonsterPrefab(_playerMonsterBaseList[playerNumber].GetId()));
        //複製したモンスターオブジェクトの親に、既に存在しているポジションオブジェクトを設定
        playerObject.transform.SetParent(playerPos);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerMonsterBase = playerObject.GetComponent<MonsterBase>();
        playerMonsterBase.charactorTag = (BattleMonsterTag.CharactorTag)playerNumber;
        _playerMonsterBaseList[playerNumber] = playerMonsterBase;

        //味方ステータスの表示
        HpGauge newHpView = Instantiate(_enemyStatusPrefab);
        Sprite icon = _iconManager.GetIconImage(playerMonsterBase.GetElement());
        newHpView.Setup(playerMonsterBase.GetNickname(), playerMonsterBase.GetMaxHPValue(), playerMonsterBase.GetCurrentHPValue(), _cameraComponent, icon);
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
        enemyObject = Instantiate(monsterManager.GetMonsterPrefab(StageData.EnemyData[enemyNumber].GetId()), enemyPos.transform.position, Quaternion.identity);

        Vector3 tempDir = enemyObject.transform.localEulerAngles;
        tempDir.y += 180;
        enemyObject.transform.localEulerAngles = tempDir;
        enemyObject.transform.SetParent(enemyPos);
        enemyObject.transform.localPosition = new Vector3(0, 0, 0);
        enemyMonsterBase = enemyObject.GetComponent<MonsterBase>();
        enemyMonsterBase.charactorTag = (BattleMonsterTag.CharactorTag)enemyNumber + 10;
        _enemyMonsterBaseList[enemyNumber] = enemyMonsterBase;
        //敵ステータスの表示
        HpGauge newHpView = Instantiate(_enemyStatusPrefab);
        Sprite icon = _iconManager.GetIconImage(enemyMonsterBase.GetElement());
        newHpView.Setup(enemyMonsterBase.GetNickname(), enemyMonsterBase.GetMaxHPValue(), enemyMonsterBase.GetCurrentHPValue(), _cameraComponent, icon);
        newHpView.transform.SetParent(enemyPos);
        newHpView.transform.position = enemyPos.position + new Vector3(0, 2.5f, 0);
        _enemyHpListInWorld.Add(newHpView);
    }
    /// <summary>
    /// キャラクターが存在しているか、生きているかを確認し、
    /// 生きているキャラの中で素早さ計算を行う。
    /// </summary>
    /// 
    void CreateTurnOrder()
    {
        for (int i = 0; i < _playerMonsterBaseList.Count; i++)
        {
            if (_playerMonsterBaseList[i])
            {
                if (CheckIfAlived(_playerMonsterBaseList[i]))
                {
                    switch (i)
                    {
                        case 1:
                            _playerMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Player1;
                            break;

                        case 2:
                            _playerMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Player2;
                            break;

                        case 3:
                            _playerMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Player3;
                            break;

                        default:
                            break;
                    }
                    _turnOrderListMonsterBase.Add(_playerMonsterBaseList[i]);
                };
            }
        }
        for (int i = 0; i < _enemyMonsterBaseList.Count; i++)
        {
            if (_enemyMonsterBaseList[i])
            {
                if (CheckIfAlived(_enemyMonsterBaseList[i]))
                {
                    switch (i)
                    {
                        case 1:
                            _enemyMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Enemy1;
                            break;

                        case 2:
                            _enemyMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Enemy2;
                            break;

                        case 3:
                            _enemyMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Enemy3;
                            break;

                        default:
                            break;
                    }
                    _turnOrderListMonsterBase.Add(_enemyMonsterBaseList[i]);
                };
            }
        }
        _turnManager.CreateTurn(_turnOrderListMonsterBase);
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
        if (monster.GetCurrentHPValue() > 0)
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
            PhaseAttack1();
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
        PhaseAttack1();
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

