using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager2 : MonoBehaviour
{
    Sequence sequence;
    public static StageData stageData;
    [SerializeField] StageData stageDataForDebug;
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
    [SerializeField] Phase phase = Phase.start;
    SkillBase defaultSkill;
    public bool keyReception { get; set; } = false;
    int maximumNumberOfMonster = 3;
    int numberOfPossessionMonster = 0;

    float cameraMovementTime = 1.0f;
    [SerializeField] float tweenSpeed = 1.0f;


    BattleMonsterTag.CharactorTag thisTurnActor;
    BattleMonsterTag.CharactorTag thisTurnTarget;
    MonsterBase thisTurnActorMonsterBase;
    MonsterBase thisTurnTargetMonsterBase;


    List<BattleMonsterTag.CharactorTag> turnListTurnCharactor = new List<BattleMonsterTag.CharactorTag>();
    List<MonsterBase> turnOrderListMonsterBase = new List<MonsterBase>();

    /// <summary>
    /// 攻撃待機しているキャラクター（同時に攻撃順が回った際に使う）
    /// </summary>
    List<MonsterBase> attackWaitingList;
    /// <summary>
    /// 攻撃順
    /// </summary>
    int turnOrder = 0;

    //スキル
    SkillBase useSkill;
    [SerializeField] SkillBase guardSkill;

    /*
     * マネージャー宣言
     */
    [SerializeField] SoundManager soundManager;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] SkillManager skillManager;
    [SerializeField] CommandManager3 commandManager3;
    [SerializeField] CommandBoxManager commandBoxManager;
    [SerializeField] BattleCamera battleCamera;
    [SerializeField] SEManager seManager;
    [SerializeField] TargetView targetView;
    [SerializeField] EffectManager effectManager;
    [SerializeField] TurnManager turnManager;
    [SerializeField] TimelineManager timelineManager;
    MonsterSort monsterSort;
    [SerializeField] SkillView skillView;
    [SerializeField] DamageManager damageManager;
    [SerializeField] HpGauge enemyStatusPrefab;
    [SerializeField] List<HpGauge> playerHpList = new List<HpGauge>();
    [SerializeField] List<HpGauge> playerHpListInWorld = new List<HpGauge>();
    [SerializeField] List<HpGauge> enemyHpListInWorld = new List<HpGauge>();
    [SerializeField] TurnDisplay turnDisplay;
    [SerializeField] IconManager iconManager;


    /*
     * オブジェクト宣言
     */
    [SerializeField] List<GameObject> playerMonsterObjectsList = new List<GameObject>();
    [SerializeField] List<GameObject> enemyMonsterObjectsList = new List<GameObject>();
    [SerializeField] List<Transform> playerMonstersPositionList = new List<Transform>();
    [SerializeField] List<Transform> enemyMonsterPositionList = new List<Transform>();
    [SerializeField] List<MonsterBase> playerMonsterBaseList = new List<MonsterBase>();
    [SerializeField] List<MonsterBase> enemyMonsterBaseList = new List<MonsterBase>();
    [SerializeField] List<Transform> playerMonsterLookPositionList = new List<Transform>();
    [SerializeField] List<Transform> enemyMonsterLookPositionList = new List<Transform>();
    [SerializeField] BattleMessage battleMessage;
    DamageView damageView;
    [SerializeField] DamageView damageViewPrefab;
    [SerializeField] StatusView2[] statusListPlayerSide = new StatusView2[3];
    Camera cameraComponent;
    [SerializeField] Color damageWeakColor;
    [SerializeField] Color damageResistColor;
    [SerializeField] Fade fade;
    [SerializeField] GameObject gameVictoryObjects;
    [SerializeField] GameObject gameLoseObjects;
    [SerializeField] GameObject nextButton;
    [SerializeField] CanvasGroup startAnimationCanvasGroup;

    UnityEngine.Random random = new UnityEngine.Random();
    delegate void Func();

    void Start()
    {
        Application.targetFrameRate = 60;

        //初期化
        turnOrderListMonsterBase = new List<MonsterBase>();
        monsterSort = new MonsterSort();
        cameraComponent = battleCamera.GetComponent<Camera>();

        //ステージデータ読み込み
        if (stageData != null)
        {
            for (int i = 0; i < stageData.EnemyData.Count; i++)
            {
                SetEnemyMonsterBase(enemyMonsterObjectsList[i], i, enemyMonsterPositionList[i], enemyMonsterBaseList[i]);
            }
        }
        else
        {
            Debug.LogWarning("ステージデータがありませんでした.デバッグ用のステージデータを読み込みます");

            stageData = stageDataForDebug;

            //敵情報のセット
            for (int i = 0; i < enemyMonsterObjectsList.Count; i++)
            {
                if (enemyMonsterObjectsList[i])
                {
                    SetEnemyMonsterBase(enemyMonsterObjectsList[i], i, enemyMonsterPositionList[i], enemyMonsterBaseList[i]);

                    //turnOrderListMonsterBase.Add(enemyMonsterBaseList[i]);
                }
            }
        }

        playerMonsterBaseList = MonsterManager.PartyMonsterList;
        //味方情報のセット
        for (int i = 0; i < playerMonsterObjectsList.Count; i++)
        {
            Debug.Log(playerMonsterObjectsList[i] + "   " + playerMonstersPositionList[i]);
            SetPlayerMonsterBase(playerMonsterObjectsList[i], i, playerMonstersPositionList[i], playerMonsterBaseList[i]);

        }
        //マネージャーのセット
        numberOfPossessionMonster = monsterManager.NumberOfPossessionMonster;
        targetView.Setup(playerMonsterBaseList, enemyMonsterBaseList);
        targetView.maximumNumberOfMonster = this.maximumNumberOfMonster;

        //デフォルトスキルの設定(通常攻撃に当たるスキル)
        defaultSkill = skillManager.GetDefaultSkill();


        //バトルスタートの効果音
        //seManager.PlaySoundDefault(SEManager.AudioType.BattleStart);

        for (int i = 0; i < statusListPlayerSide.Length; i++)
        {
            statusListPlayerSide[i].Setup(playerMonsterBaseList[i]);
        }

        //前の処理に戻るときの、戻り先設定
        skillView.BackToPhaseForBattleManager = BackToBeforePhase;
        targetView.BackToBeforePhaseForBattleManager = BackToBeforePhase;

        /*
        //フェードアウト
        if (fade.isActiveAndEnabled == true)
        {
            fade.FadeOut(1.0f, () => PhasePreparation());
        }
        else
        {
            PhasePreparation();
        }
        */
        PhaseStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == Phase.Wait)
        {
            //if (attackWaitingList == null)
            //{
            //    attackWaitingList = timelineManager.UpdateTimeline(true);
            //}
            if (attackWaitingList != null)
            {
                if (attackWaitingList.Count > 1) //2体より多い場合はソートする
                {
                    monsterSort.turnListMonsterBase = attackWaitingList;
                    monsterSort.QuickSort(0, attackWaitingList.Count - 1);
                    attackWaitingList = monsterSort.turnListMonsterBase;
                }
                if (attackWaitingList.Count >= 1)
                {
                    thisTurnActorMonsterBase = attackWaitingList[0];
                    thisTurnActor = attackWaitingList[0].charactorTag;
                    timelineManager.MoveImageToFront(thisTurnActorMonsterBase);

                    if ((int)attackWaitingList[0].charactorTag >= 10) //敵ならば10番以降を使用しているためこれで判別する。
                    {
                        PhaseAI(attackWaitingList[0]);
                    }
                    else //味方ならばコマンド選択の開始
                    {
                        thisTurnActor = attackWaitingList[0].charactorTag;

                        PhaseCommand();
                    }
                }
                else
                {
                    attackWaitingList = null;
                }
            }
        }


    }

    IEnumerator WaitForAttackList(bool advanceTime)
    {
        while (true)
        {
            attackWaitingList = timelineManager.UpdateTimeline(advanceTime);
            if (attackWaitingList == null)
            {
                Debug.Log("hey");
                yield return null;
            }
            else
            {
                break;
            }
        }
        
    }

    public void WriteMessage(string message)
    {
        battleMessage.UpdateMessage(message);
    }

    void PhaseStart()
    {
        sequence = DOTween.Sequence();
        sequence
        .Append(startAnimationCanvasGroup.DOFade(1.0f, 1.0f))
        .AppendInterval(2.0f)
        .AppendCallback(() => soundManager.PlaySe(SoundManager.SeList.BattleStart))
        .Append(startAnimationCanvasGroup.gameObject.transform.DOScale(10.0f, 1.0f))
        .Insert(3.0f, startAnimationCanvasGroup.DOFade(0.0f, 0.5f))
        .InsertCallback(3.0f, () =>
         {
             //フェードアウト
             if (fade.isActiveAndEnabled == true)
             {
                 fade.FadeOut(1.0f, () => PhasePreparation());
             }
             else
             {
                 PhasePreparation();
             }
         });
    }

    void PhasePreparation()
    {
        phase = Phase.Preparation;

        soundManager.PlayBgm();


        List<MonsterBase> allMonsterBaseList = new List<MonsterBase>();
        allMonsterBaseList.AddRange(playerMonsterBaseList);
        allMonsterBaseList.AddRange(enemyMonsterBaseList);

        timelineManager.Setup(allMonsterBaseList);


        //敵全体をカメラで映す演出
        //battleCamera.AllEnemyView(() => PhaseEnemyView());
        turnOrder = 0;
        phase = Phase.Wait;
        StartCoroutine(WaitForAttackList(true));


    }

    public void PhaseEnemyView()
    {
        sequence.AppendInterval(1.0f)
                .AppendCallback(() =>
                {

                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);

                    turnOrder = 0;
                    phase = Phase.Wait;
                });
    }


    void PhaseCommand()
    {
        phase = Phase.Command;
        turnDisplay.DisplayImage(TurnDisplay.DisplayPattern.PlayerTurn);
        switch (thisTurnActor)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                statusListPlayerSide[0].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                statusListPlayerSide[1].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                statusListPlayerSide[2].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                break;
            default:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
                break;
        }

        if (thisTurnActorMonsterBase.status == MonsterBase.MonsterState.Guard)
        {
            thisTurnActorMonsterBase.status = MonsterBase.MonsterState.Normal;
        }

        phase = Phase.Command;
        commandBoxManager.Setup(SetUseSkill);
        commandBoxManager.PhaseSkill = PhaseChooseSkill;

        keyReception = true;
    }

    public void PhaseChooseSkill()
    {
        phase = Phase.ChooseSkill;
        skillView.SetUseSkill = this.SetUseSkill;
        switch (thisTurnActor)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                statusListPlayerSide[0].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                statusListPlayerSide[1].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                statusListPlayerSide[2].ChangeState(true);
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                break;
            default:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
                break;
        }

        int actorNumber = (int)thisTurnActor;
        skillView.Setup(thisTurnActorMonsterBase.GetSkillList());

        skillView.Display(true);
        skillView.SetInput();

        keyReception = true;
    }

    void PhaseTarget()
    {
        phase = Phase.Target;
        commandBoxManager.DisplayCommand(false);
        List<bool> targetList = new List<bool>();
        for (int i = 0; i < maximumNumberOfMonster; i++)
        {
            if (enemyMonsterBaseList[i])
            {
                targetList.Add(CheckIfAlived(enemyMonsterBaseList[i]));
            }
        }
        targetView.TargetPreparation(TargetView.TargetType.EnemySide, targetList.ToArray(), SetTarget);
    }

    void PhaseAI(MonsterBase monster)
    {
        phase = Phase.Enemy;
        //モンスターに使用スキルを考えさせる
        useSkill = monster.ThinkOfASkill();
        thisTurnActor = monster.charactorTag;

        if ((int)monster.charactorTag >= 10) //敵側のモンスターなら
        {
            int count = playerMonsterBaseList.Count * 100;
            turnDisplay.DisplayImage(TurnDisplay.DisplayPattern.EnemyTurn);
            int targetNumber = UnityEngine.Random.Range(0, playerMonsterBaseList.Count);
            while (true)
            {
                if (CheckIfAlived(playerMonsterBaseList[targetNumber]) == true)
                {
                    thisTurnTarget = (BattleMonsterTag.CharactorTag)targetNumber;
                    switch (thisTurnTarget)
                    {
                        case BattleMonsterTag.CharactorTag.Player1:
                            thisTurnTargetMonsterBase = playerMonsterBaseList[0];
                            break;
                        case BattleMonsterTag.CharactorTag.Player2:
                            thisTurnTargetMonsterBase = playerMonsterBaseList[1];
                            break;
                        case BattleMonsterTag.CharactorTag.Player3:
                            thisTurnTargetMonsterBase = playerMonsterBaseList[2];
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
                if (targetNumber >= playerMonsterBaseList.Count)
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
        int actorNumber = (int)thisTurnActor;

        WriteMessage(useSkill.GetName());

        if (useSkill.GetSkillType() == SkillBase.SkillType.Physical || useSkill.GetSkillType() == SkillBase.SkillType.Magic)
        {
            if (actorNumber < 10)
            {
                if (actorNumber == 0)
                {
                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                }
                else if (actorNumber == 1)
                {
                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                }
                else if (actorNumber == 2)
                {
                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                }
                playerMonsterBaseList[actorNumber].AfterAction = PhaseAttack2;
                playerMonsterBaseList[actorNumber].CheckEndOfAnimation();
                playerMonsterBaseList[actorNumber].MotionAttack();

            }
            else if (actorNumber >= 10)
            {
                if (actorNumber == 10)
                {
                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1);
                }
                else if (actorNumber == 11)
                {
                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2);
                }
                else if (actorNumber == 12)
                {
                    battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3);
                }

                enemyMonsterBaseList[actorNumber - 10].AfterAction = PhaseAttack2;
                enemyMonsterBaseList[actorNumber - 10].CheckEndOfAnimation();
                enemyMonsterBaseList[actorNumber - 10].MotionAttack();
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
        int targetNumber = (int)thisTurnTarget;
        //エフェクト再生位置を表す変数
        Vector3 effectProducePos = new Vector3();
        //味方か敵かどちらの側に再生するかを表す変数(暫定的に敵側で初期化)
        EffectManager.DirectionToProduce direction = EffectManager.DirectionToProduce.EnemySide;
        if (targetNumber < 10)
        {
            if (targetNumber == 0)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
            }
            else if (targetNumber == 1)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
            }
            else if (targetNumber == 2)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
            }

            if (useSkill.GetSkillType() == SkillBase.SkillType.Physical || useSkill.GetSkillType() == SkillBase.SkillType.Magic)
            {
                playerMonsterBaseList[targetNumber].MotionTakeDamege();
            }
            else
            {
                playerMonsterBaseList[targetNumber].MotionAttack();
                if (useSkill.GetSkillType() == SkillBase.SkillType.Guard) //ガード・防御時処理
                {
                    playerMonsterBaseList[targetNumber].status = MonsterBase.MonsterState.Guard;
                }
                if (useSkill.GetSkillType() == SkillBase.SkillType.Charge) //チャージ時処理
                {
                    playerMonsterBaseList[targetNumber].status = MonsterBase.MonsterState.Charge;
                }
            }

            playerMonsterBaseList[targetNumber].AfterAction = PhaseAfterAttack;
            playerMonsterBaseList[targetNumber].CheckEndOfAnimation();
            effectProducePos = playerMonsterBaseList[targetNumber].transform.position;
            effectProducePos.z += 1.0f;
            direction = EffectManager.DirectionToProduce.PlayerSide;

            effectProducePos = playerMonsterBaseList[targetNumber].transform.position;

            damageGeneratePos = playerMonsterBaseList[targetNumber].transform.position;
            damageGeneratePos += new Vector3(1f, 0, 1.5f);

        }
        else if (targetNumber >= 10)
        {
            if (targetNumber == 10)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1);
            }
            else if (targetNumber == 11)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2);
            }
            else if (targetNumber == 12)
            {
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3);
            }
            if (useSkill.GetSkillType() == SkillBase.SkillType.Physical || useSkill.GetSkillType() == SkillBase.SkillType.Magic)
            {
                enemyMonsterBaseList[targetNumber - 10].MotionTakeDamege();
            }
            else
            {
                enemyMonsterBaseList[targetNumber - 10].MotionAttack();
            }

            enemyMonsterBaseList[targetNumber - 10].AfterAction = PhaseAfterAttack;
            enemyMonsterBaseList[targetNumber - 10].CheckEndOfAnimation();
            effectProducePos = enemyMonsterBaseList[targetNumber - 10].transform.position;
            effectProducePos.z -= 1.0f;
            direction = EffectManager.DirectionToProduce.EnemySide;

            damageGeneratePos = enemyMonsterBaseList[targetNumber - 10].transform.position;
            damageGeneratePos += new Vector3(1f, 0, -1.5f);
        }
        effectManager.ProduceEffect(useSkill.GetEffect(), effectProducePos, direction);


        if (useSkill.GetDamage() > 0)
        {
            //パッシブスキルの整理
            List<PassiveSkillBase> pSkillListPlayerSide = new List<PassiveSkillBase>();
            List<PassiveSkillBase> pSkillListEnemySide = new List<PassiveSkillBase>();
            for (int i = 0; i < playerMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(playerMonsterBaseList[0]) == true)
                {
                    pSkillListPlayerSide.Add(playerMonsterBaseList[i].GetPassiveSkill());
                }
                else
                {
                    pSkillListPlayerSide.Add(null);
                }
            }
            for (int i = 0; i < enemyMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(enemyMonsterBaseList[0]) == true)
                {
                    pSkillListEnemySide.Add(enemyMonsterBaseList[i].GetPassiveSkill());
                }
                else
                {
                    pSkillListEnemySide.Add(null);
                }
            }


            //ダメージ計算とHP更新
            int damage = damageManager.DamageCalculator(thisTurnActorMonsterBase, thisTurnTargetMonsterBase, useSkill, pSkillListPlayerSide, pSkillListEnemySide);

            thisTurnTargetMonsterBase.TakeDamage(damage);

            //ダメージ表示(ガードの場合は表示しない)
            if (useSkill.GetSkillType() != SkillBase.SkillType.Guard)
            {
                string damageMessage = "";
                Color damageViewColor = Color.white;
                damageView = Instantiate(damageViewPrefab);
                if (Element.CheckAdvantage(useSkill.GetElement(), thisTurnTargetMonsterBase.GetElement()) > 1)
                {
                    damageViewColor = damageWeakColor;
                    Debug.Log("WEAK!!");
                }
                else if (Element.CheckAdvantage(useSkill.GetElement(), thisTurnTargetMonsterBase.GetElement()) < 1)
                {
                    damageViewColor = damageResistColor;
                }
                damageView.Setup(damage, damageMessage, damageViewColor, cameraComponent);
            }
        }

        //味方ステータス表示の更新
        switch (thisTurnTarget)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                if (playerHpList[0])
                {
                    if (playerHpList[0].isActive)
                    {
                        playerHpList[0].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }
                if (playerHpListInWorld[0])
                {
                    if (playerHpListInWorld[0].isActive)
                    {
                        playerHpListInWorld[0].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }

                break;
            case BattleMonsterTag.CharactorTag.Player2:
                if (playerHpList.Count >= 2)
                {
                    if (playerHpList[1])
                    {
                        if (playerHpList[1].isActive)
                        {
                            playerHpList[1].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                        }
                    }
                }
                if (playerHpListInWorld[1])
                {
                    if (playerHpListInWorld[1].isActive)
                    {
                        playerHpListInWorld[1].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }

                break;
            case BattleMonsterTag.CharactorTag.Player3:
                if (playerHpList.Count >= 3)
                {
                    if (playerHpList[2])
                    {
                        if (playerHpList[2].isActive)
                        {
                            playerHpList[2].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                        }
                    }
                }
                if (playerHpListInWorld[2])
                {
                    if (playerHpListInWorld[2].isActive)
                    {
                        playerHpListInWorld[2].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                    }
                }

                break;


            case BattleMonsterTag.CharactorTag.Enemy1:
                enemyHpListInWorld[0].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                enemyHpListInWorld[1].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                enemyHpListInWorld[2].UpdateStatus(thisTurnTargetMonsterBase.GetCurrentHPValue());
                break;
            default:
                break;
        }

        if (useSkill.GetDamage() > 0)
        {
            //高さ調整
            damageGeneratePos += new Vector3(0, 1f, 0);
            damageView.transform.position = damageGeneratePos;
            //カメラの方向を向く    
            //damageView.transform.LookAt(battleCamera.transform);
            damageView.transform.rotation = battleCamera.transform.rotation;
            sequence = DOTween.Sequence();
            sequence.Append(damageView.transform.DOJump((damageGeneratePos + new Vector3(0, 0.05f, 0)), 1, 1, 0.3f).SetEase(Ease.InBounce))
                    .Append(damageView.transform.DOMove(battleCamera.transform.position, 20f));

            damageView.isActive = true;

        }

    }



    public void PhaseAfterAttack()
    {
        phase = Phase.AfterAttack;

        battleMessage.CloseMessage();
        effectManager.DestroyObject();
        turnDisplay.DisplayImage(TurnDisplay.DisplayPattern.None);
        sequence.Kill();
        if (damageView)
        {
            Destroy(damageView.gameObject);
        }
        Debug.Log("使用スキル：" + useSkill);
        if (thisTurnActorMonsterBase.status == MonsterBase.MonsterState.Charge && useSkill.GetSkillType() != SkillBase.SkillType.Charge)
        {
            Debug.Log("チャージ解除");
            thisTurnActorMonsterBase.status = MonsterBase.MonsterState.Normal;
        }



        if ((int)thisTurnActor < 10)
        {
            playerMonsterBaseList[(int)thisTurnActor].coolTime = useSkill.GetCoolTime();
        }
        else
        {
            enemyMonsterBaseList[((int)thisTurnActor) - 10].coolTime = useSkill.GetCoolTime();
        }
        attackWaitingList.RemoveAt(0);
        timelineManager.UpdateTimeline(false);
        if (!CheckIfAlived(thisTurnTargetMonsterBase))
        {
            PhaseDead();
            return;
        }

        for (int i = 0; i < statusListPlayerSide.Length; i++)
        {
            statusListPlayerSide[i].ChangeState(false);
        }

        if (attackWaitingList.Count == 0)
        {
            battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
            attackWaitingList = null;
            StartCoroutine(WaitForAttackList(true));
            phase = Phase.Wait;
        }
        else
        {
            StartCoroutine(WaitForAttackList(false));
            phase = Phase.Wait;
        }
    }

    public void PhaseDead()
    {
        switch (thisTurnTarget)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player2);
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player3);
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy1);
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy2);
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Enemy3);
                break;
            default:
                break;
        }
        thisTurnTargetMonsterBase.AfterDead = PhaseDead2;
        thisTurnTargetMonsterBase.CheckEndOfDeadAnimation();
        thisTurnTargetMonsterBase.MotionDead();
    }

    public void PhaseDead2()
    {
        timelineManager.DeleteMonster(thisTurnTargetMonsterBase);
        for (int i = 0; i < attackWaitingList.Count; i++)
        {
            if (attackWaitingList[i] == thisTurnTargetMonsterBase)
            {
                attackWaitingList.Remove(thisTurnTargetMonsterBase);
            }
        }

        for (int i = 0; i < statusListPlayerSide.Length; i++)
        {
            statusListPlayerSide[i].ChangeState(false);
        }

        //全滅チェック
        BattleMonsterTag.CharactorTag targetTag = thisTurnTargetMonsterBase.charactorTag;
        if (targetTag == BattleMonsterTag.CharactorTag.Enemy1 || targetTag == BattleMonsterTag.CharactorTag.Enemy2 || targetTag == BattleMonsterTag.CharactorTag.Enemy3)
        {
            bool allDeadFlag = true;
            for (int i = 0; i < enemyMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(enemyMonsterBaseList[i]))
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
            for (int i = 0; i < playerMonsterBaseList.Count; i++)
            {
                if (CheckIfAlived(playerMonsterBaseList[i]))
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


        if (attackWaitingList.Count == 0)
        {
            battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
            attackWaitingList = null;
            StartCoroutine(WaitForAttackList(true));
            phase = Phase.Wait;
        }
        else
        {
            StartCoroutine(WaitForAttackList(false));
            phase = Phase.Wait;
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

            battleCamera.SetCameraPosition(BattleCamera.CameraPosition.Player1);
        }
    }

    /// <summary>
    /// 前のフェイズに戻る処理
    /// </summary>
    public void BackToBeforePhase()
    {
        switch (phase)
        {
            case Phase.Preparation:
                break;
            case Phase.Command:
                break;
            case Phase.ChooseSkill:

                PhaseCommand();

                break;
            case Phase.Target:

                if (useSkill == defaultSkill)
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
    /// 味方モンスターデータの設定
    /// </summary>
    /// <param name="playerObject">生成した味方オブジェクトの保存先</param>
    /// <param name="playerNumber">味方番号</param>
    /// <param name="playerPos">味方のオブジェクトの配置箇所</param>
    /// <param name="playerMonsterBase">味方の情報</param>
    void SetPlayerMonsterBase(GameObject playerObject, int playerNumber, Transform playerPos, MonsterBase playerMonsterBase)
    {
        //味方情報の更新
        playerObject = Instantiate(monsterManager.GetMonsterPrefab(playerMonsterBaseList[playerNumber].GetId()));
        //複製したモンスターオブジェクトの親に、既に存在しているポジションオブジェクトを設定
        playerObject.transform.SetParent(playerPos);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerMonsterBase = playerObject.GetComponent<MonsterBase>();
        playerMonsterBase.charactorTag = (BattleMonsterTag.CharactorTag)playerNumber;
        playerMonsterBaseList[playerNumber] = playerMonsterBase;

        //味方ステータスの表示
        HpGauge newHpView = Instantiate(enemyStatusPrefab);
        Sprite icon = iconManager.GetIconImage(playerMonsterBase.GetElement());
        newHpView.Setup(playerMonsterBase.GetNickname(), playerMonsterBase.GetMaxHPValue(), playerMonsterBase.GetCurrentHPValue(), cameraComponent, icon);
        newHpView.transform.SetParent(playerPos);
        newHpView.transform.position = playerPos.position + new Vector3(0, 2.5f, 0);
        playerHpListInWorld.Add(newHpView);

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
        enemyObject = Instantiate(monsterManager.GetMonsterPrefab(stageData.EnemyData[enemyNumber].GetId()), enemyPos.transform.position, Quaternion.identity);

        Vector3 tempDir = enemyObject.transform.localEulerAngles;
        tempDir.y += 180;
        enemyObject.transform.localEulerAngles = tempDir;
        enemyObject.transform.SetParent(enemyPos);
        enemyObject.transform.localPosition = new Vector3(0, 0, 0);
        enemyMonsterBase = enemyObject.GetComponent<MonsterBase>();
        enemyMonsterBase.charactorTag = (BattleMonsterTag.CharactorTag)enemyNumber + 10;
        enemyMonsterBaseList[enemyNumber] = enemyMonsterBase;
        //敵ステータスの表示
        HpGauge newHpView = Instantiate(enemyStatusPrefab);
        Sprite icon = iconManager.GetIconImage(enemyMonsterBase.GetElement());
        newHpView.Setup(enemyMonsterBase.GetNickname(), enemyMonsterBase.GetMaxHPValue(), enemyMonsterBase.GetCurrentHPValue(), cameraComponent, icon);
        newHpView.transform.SetParent(enemyPos);
        newHpView.transform.position = enemyPos.position + new Vector3(0, 2.5f, 0);
        enemyHpListInWorld.Add(newHpView);
    }
    /// <summary>
    /// キャラクターが存在しているか、生きているかを確認し、
    /// 生きているキャラの中で素早さ計算を行う。
    /// </summary>
    /// 
    void CreateTurnOrder()
    {
        for (int i = 0; i < playerMonsterBaseList.Count; i++)
        {
            if (playerMonsterBaseList[i])
            {
                if (CheckIfAlived(playerMonsterBaseList[i]))
                {
                    switch (i)
                    {
                        case 1:
                            playerMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Player1;
                            break;

                        case 2:
                            playerMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Player2;
                            break;

                        case 3:
                            playerMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Player3;
                            break;

                        default:
                            break;
                    }
                    turnOrderListMonsterBase.Add(playerMonsterBaseList[i]);
                };
            }
        }
        for (int i = 0; i < enemyMonsterBaseList.Count; i++)
        {
            if (enemyMonsterBaseList[i])
            {
                if (CheckIfAlived(enemyMonsterBaseList[i]))
                {
                    switch (i)
                    {
                        case 1:
                            enemyMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Enemy1;
                            break;

                        case 2:
                            enemyMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Enemy2;
                            break;

                        case 3:
                            enemyMonsterBaseList[i].charactorTag = BattleMonsterTag.CharactorTag.Enemy3;
                            break;

                        default:
                            break;
                    }
                    turnOrderListMonsterBase.Add(enemyMonsterBaseList[i]);
                };
            }
        }
        turnManager.CreateTurn(turnOrderListMonsterBase);
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
        useSkill = skill;
        if (useSkill.GetSkillType() == SkillBase.SkillType.Guard || useSkill.GetSkillType() == SkillBase.SkillType.Charge) //防御,チャージ時
        {
            thisTurnTarget = thisTurnActor;
            thisTurnTargetMonsterBase = thisTurnActorMonsterBase;
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
        thisTurnTarget = charactor;
        switch (thisTurnTarget)
        {
            case BattleMonsterTag.CharactorTag.Player1:
                thisTurnTargetMonsterBase = playerMonsterBaseList[0];
                break;
            case BattleMonsterTag.CharactorTag.Player2:
                thisTurnTargetMonsterBase = playerMonsterBaseList[1];
                break;
            case BattleMonsterTag.CharactorTag.Player3:
                thisTurnTargetMonsterBase = playerMonsterBaseList[2];
                break;
            case BattleMonsterTag.CharactorTag.Enemy1:
                thisTurnTargetMonsterBase = enemyMonsterBaseList[0];
                break;
            case BattleMonsterTag.CharactorTag.Enemy2:
                thisTurnTargetMonsterBase = enemyMonsterBaseList[1];
                break;
            case BattleMonsterTag.CharactorTag.Enemy3:
                thisTurnTargetMonsterBase = enemyMonsterBaseList[2];
                break;
            default:
                break;
        }
        phase = Phase.Attack;
        PhaseAttack1();
    }

    /// <summary>
    /// 勝利演出
    /// </summary>
    void VictoryProduction()
    {
        battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
        gameVictoryObjects.SetActive(true);
        sequence = DOTween.Sequence();
        sequence.Append(gameVictoryObjects.transform.DOScale(Vector3.one, tweenSpeed))
                .AppendCallback(WaitingForGameEnd);

    }

    /// <summary>
    /// 敗北演出
    /// </summary>
    void LoseProduction()
    {
        battleCamera.SetCameraPosition(BattleCamera.CameraPosition.DefaultPositon);
        gameLoseObjects.SetActive(true);
        sequence = DOTween.Sequence();
        sequence.Append(gameLoseObjects.transform.DOScale(Vector3.one, tweenSpeed))
                .AppendCallback(WaitingForGameEnd);
    }

    void WaitingForGameEnd()
    {
        nextButton.SetActive(true);
        nextButton.transform.DOScale(Vector3.one, tweenSpeed / 2);
        InputManager.ResetInputSettings();
        InputManager.InputEnter = GameEndProcess;
    }


    void GameEndProcess()
    {
        InputManager.ResetInputSettings();
        InputManager.setupCompleted = false;
        if (fade.isActiveAndEnabled == true)
        {
            soundManager.StopBgm();
            fade.FadeIn(1.0f, BackToFrontScene);
        }
        else
        {
            soundManager.StopBgm();
            BackToFrontScene();
        }

    }

    void BackToFrontScene()
    {
        SceneManager.LoadScene("Front");
    }
}
