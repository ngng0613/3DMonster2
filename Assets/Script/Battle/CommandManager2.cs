using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommandManager2 : MonoBehaviour
{

    //選択中モード
    enum CommandMode
    {
        Command = 1,
        Item = 2,
        Skill = 3,
        escape = 4,
    }

    [SerializeField] CommandMode commandMode;
    Sequence sequence;

    //コマンドボックスを代入し、動かす時の枠(中身は入れ替わる)
    public GameObject command1;
    public GameObject command2;
    public GameObject command3;
    GameObject commandTemp;

    public GameObject[] commandBoxObjects = new GameObject[3];
    /// <summary>
    /// コマンドボックスが画面外に移動する際に参照するY座標
    /// </summary>
    float offScreenPosY = -500;
    [SerializeField] float commandBoxMoveSpeed = 0.5f;

    /// <summary>
    /// 表示できるコマンドの数
    /// </summary>
    int maximumNumberOfCommand;

    List<CommandBox> commandBoxList = new List<CommandBox>();

    SEManager seManager;

    //コマンドが回転する際のZ軸移動量
    const float commandMoveZ = 50.0f;

    //選択中の戦闘コマンド
    [SerializeField] int selectedBattleCommand = 1;
    //最大戦闘コマンド数
    const int maxNumberOfBattleCommand = 3;

    //コマンドボックス
    [SerializeField] GameObject commandBoxPrefab;
    [SerializeField] Text charaDisplay_Text;
    [SerializeField] Image charaDisplay_Image;

    [SerializeField] GameObject canvas;
    [SerializeField] List<GameObject> battleCommandList;

    //コマンド説明文
    [SerializeField] Text explanationText;

    bool toNextCommand = false;
    bool toPreviousCommand = false;
    bool cancelCommand;

    [SerializeField] int cancelPhase = 1;

    public int moveTurn = 1;

    //コマンドボックス定位置

    Vector3 commandBoxFixPos = new Vector3(-500, 50, 0);

    const float commandFixPosX = -500;
    const float commandFixPosY = 50;
    const float commandFixPosZ = 0;

    //コマンドボックスのズレ
    Vector3 commandBoxPosShift = new Vector3(250, 15, 300);

    const float posXShift = 250;
    const float posYShift = 15;
    const float posZShift = 300;
    const float commandPosX_SkillMode = -1170;

    //コマンドベース周り
    [SerializeField] GameObject commandBase;
    const float commandBaseFixPosX = 753;
    public bool isCommandBaseOnTheLeft = true;

    //コマンドベースの移動スイッチ
    [SerializeField] bool moveCommandBase;
    //コマンドベース移動方向
    //（ 0: →　1:←）
    [SerializeField] bool commandBaseMoveDirection = true;

    /// <summary>
    /// メッセージ変更の権限
    /// </summary>
    bool messageSwitch = false;

    //参照先オブジェクト
    [SerializeField] BattleManager2 battleManager;


    //スキル・アイテムウインドウ関連

    [SerializeField] SkillManager skillManager;
    MonsterBase turnCharactor;

    [SerializeField] List<SkillBase> skillList;

    //Position軸の方のスキルベース
    [SerializeField] GameObject skillPositionBase;
    const float skillPosBase_defaultPos = 1200;

    int skillPhase = 1;

    SkillBase displaySkill1;
    SkillBase displaySkill2;
    SkillBase displaySkill3;

    GameObject skillObject1;
    GameObject skillObject2;
    GameObject skillObject3;

    Text skillName1;
    Text skillName2;
    Text skillName3;

    //画像ファイル
    [SerializeField] Image skillWindowImage;
    [SerializeField] GameObject skillWindowObject;

    //効果音ファイル

    [SerializeField] GameObject seObject;

    [SerializeField] AudioClip se_cursor;

    public enum CommandList
    {
        Attack,
        Skill,
        Item,
        Escape,
    }

    //前に選択したコマンドの記憶の枠
    GameObject command1BeforeBattleCommand;
    GameObject command2BeforeBattleCommand;
    GameObject command3BeforeBattleCommand;

    GameObject command1BeforeItemCommand;
    GameObject command2BeforeItemCommand;
    GameObject command3BeforeItemCommand;

    GameObject command1BeforeSkillCommand;
    GameObject command2BeforeSkillCommand;
    GameObject command3BeforeSkillCommand;

    bool tweenSwitch = true;

    private void Start()
    {

        for (int i = 0; i < commandBoxObjects.Length; i++)
        {
            commandBoxObjects[i] = Instantiate(commandBoxPrefab);
            commandBoxObjects[i].transform.SetParent(commandBase.transform);
            commandBoxObjects[i].transform.localScale = Vector3.one;
            commandBoxList.Add(commandBoxObjects[i].GetComponent<CommandBox>());
        }

        command1BeforeBattleCommand = command1;
        command2BeforeBattleCommand = command2;
        command3BeforeBattleCommand = command3;

        commandBoxObjects[0].GetComponent<RectTransform>().localPosition = commandBoxFixPos;
        commandBoxObjects[1].GetComponent<RectTransform>().localPosition = commandBoxFixPos + commandBoxPosShift;
        commandBoxObjects[2].GetComponent<RectTransform>().localPosition = commandBoxFixPos + commandBoxPosShift * 2;

        //commandBoxObjects[0].GetComponent<>();

        commandBoxList[0].BlinkOn();

        commandMode = CommandMode.Command;

    }


    /// <summary>
    /// キャンバスにコマンドボックスを生成
    /// </summary>
    /// <param name="commandBox"></param>
    /// <param name="canvas"></param>
    public void Setup()
    {
        messageSwitch = true;

        battleCommandList = new List<GameObject>();

        for (int i = 0; i < maximumNumberOfCommand; i++)
        {
            commandBoxObjects[0] = Instantiate(commandBoxPrefab);
            Text commandText = commandBoxObjects[0].GetComponent<CommandBox>().myText;
            switch (i)
            {
                case 1:
                    commandText.text = "攻撃";
                    break;

                case 2:
                    commandText.text = "スキル";
                    break;

                case 3:
                    commandText.text = "逃げる";
                    break;

                default:
                    break;
            }
        }
    }


    /// <summary>
    /// コマンド選択時処理 決定
    /// </summary>
    /// <returns></returns>
    public void ChoiceCommand()
    {
        if (commandMode == CommandMode.Command) //選択できる場合
        {
            if (selectedBattleCommand == 1) //通常攻撃選択時
            {
                battleManager.SetUseSkill(skillManager.GetDefaultSkill());

            }
            else if (selectedBattleCommand == 2)
            {

            }
            else if (selectedBattleCommand == 3)
            {

            }
            //選択できない場合
            else
            {

            }
        }
        else if (commandMode == CommandMode.Item)
        {

        }

    }

    public void CancelCommand()
    {
        if (commandMode == CommandMode.Item || commandMode == CommandMode.Skill)
        {
            //変更中
            cancelPhase = 1;
            seManager.PlaySoundDefault(SEManager.AudioType.CancelButton);

        }
    }

    //コマンドをまるごと隠す
    public void HideCommandBase()
    {
        //左にいるなら右移動
        moveCommandBase = true;
        commandBaseMoveDirection = true;
    }

    public void DisplayCommandBase()
    {
        //右にいるなら左移動
        moveCommandBase = true;
        commandBaseMoveDirection = false;
    }


    /// <summary>
    /// 次のコマンドへ
    /// </summary>
    /// <param name="battleManagerTemp"></param>
    public void ToNextCommand()
    {
        commandBoxList[0].BlinkOff();
        battleManager.keyReception = false;

        //現在のコマンドから1つ進む
        selectedBattleCommand++;
        //toNextCommand = true;

        //処理

        sequence = DOTween.Sequence();
        sequence.Append(commandBoxObjects[0].transform.DOLocalMoveY(offScreenPosY, commandBoxMoveSpeed))
                .Insert(commandBoxMoveSpeed / 2, commandBoxObjects[1].transform.DOLocalMove(commandBoxFixPos, commandBoxMoveSpeed))
                .Insert(commandBoxMoveSpeed / 2, commandBoxObjects[2].transform.DOLocalMove(commandBoxFixPos + commandBoxPosShift, commandBoxMoveSpeed))
                .Insert(commandBoxMoveSpeed, commandBoxObjects[0].transform.DOLocalMove(commandBoxFixPos + commandBoxPosShift * 2, commandBoxMoveSpeed))
                .AppendInterval(commandBoxMoveSpeed + 0.1f)
                .AppendCallback(() =>
                {
                    CommandBox commandBoxTemp = commandBoxList[0];
                    GameObject commandBoxObjectTemp = commandBoxObjects[0];
                    for (int i = 0; i < commandBoxObjects.Length - 1; i++)
                    {
                        commandBoxList[i] = commandBoxList[i + 1];
                        commandBoxObjects[i] = commandBoxObjects[i + 1];
                    }
                    commandBoxList[commandBoxList.Count - 1] = commandBoxTemp;
                    commandBoxObjects[commandBoxObjects.Length - 1] = commandBoxObjectTemp;
                    commandBoxList[0].BlinkOn();
                    battleManager.keyReception = true;
                });

        //最前で表示されているコマンドがListで最後のコマンドでないなら
        if (selectedBattleCommand > maxNumberOfBattleCommand)
        {
            selectedBattleCommand = 1;
            //SE再生
            //seManager.PlaySoundDefault(SEManager.AudioType.BattleCursor);
            moveTurn = 1;
        }


    }
    public void ToPreviousCommand()
    {
        commandBoxList[0].BlinkOff();
        battleManager.keyReception = false;

        //現在のコマンドから1つ進む
        selectedBattleCommand--;
        //toNextCommand = true;

        //処理

        sequence = DOTween.Sequence();
        sequence.Append(commandBoxObjects[2].transform.DOLocalMoveY(offScreenPosY, commandBoxMoveSpeed))
                .Insert(commandBoxMoveSpeed / 2, commandBoxObjects[1].transform.DOLocalMove(commandBoxFixPos + commandBoxPosShift * 2, commandBoxMoveSpeed))
                .Insert(commandBoxMoveSpeed / 2, commandBoxObjects[0].transform.DOLocalMove(commandBoxFixPos + commandBoxPosShift, commandBoxMoveSpeed))
                .Insert(commandBoxMoveSpeed, commandBoxObjects[2].transform.DOLocalMove(commandBoxFixPos, commandBoxMoveSpeed))
                .AppendInterval(commandBoxMoveSpeed + 0.1f)
                .AppendCallback(() =>
                {
                    CommandBox commandBoxTemp = commandBoxList[2];
                    GameObject commandBoxObjectTemp = commandBoxObjects[2];
                    for (int i = commandBoxObjects.Length - 2; i >= 0; i--)
                    {
                        commandBoxList[i + 1] = commandBoxList[i];
                        commandBoxObjects[i + 1] = commandBoxObjects[i];
                    }
                    commandBoxList[0] = commandBoxTemp;
                    commandBoxObjects[0] = commandBoxObjectTemp;
                    commandBoxList[0].BlinkOn();
                    battleManager.keyReception = true;
                });

        //最前で表示されているコマンドがListで最初のコマンドでないなら
        if (selectedBattleCommand == 1)
        {
            //現在のコマンドから1つ進む
            selectedBattleCommand = 3;

            //SE再生
            //seManager.PlaySoundDefault(SEManager.AudioType.BattleCursor);
        }

    }


    public void SetTurnCharactor(MonsterBase turnCharactor)
    {
        this.turnCharactor = turnCharactor;
        charaDisplay_Text.text = turnCharactor.GetNickname();
        charaDisplay_Image.sprite = turnCharactor.GetImage();
    }

    //メッセージオフ
    public void ChangeMessageSwitch(bool temp)
    {
        messageSwitch = temp;
    }

    public int GetCommandMode()
    {
        return (int)commandMode;
    }

    public SkillBase GetSellectSkill()
    {
        if (selectedBattleCommand == 1)
        {
            Debug.Log(skillObject1.GetComponent<CommandBox>().GetSkill());
            return skillObject1.GetComponent<CommandBox>().GetSkill();
        }
        else if (selectedBattleCommand == 2)
        {
            return skillObject2.GetComponent<CommandBox>().GetSkill();
        }
        else if (selectedBattleCommand == 3)
        {
            return skillObject3.GetComponent<CommandBox>().GetSkill();
        }
        else
        {
            return null;
        }


    }

    /// <summary>
    /// コマンドベースのリセット
    /// </summary>
    public void ResetCommandBase()
    {
        //スキルベースの子オブジェクトを全て削除
        foreach (Transform child in skillPositionBase.transform)
        {
            Destroy(child.gameObject);
        }

        command1 = command1BeforeBattleCommand;
        command2 = command2BeforeBattleCommand;
        command3 = command3BeforeBattleCommand;
        if (command2.transform.Find("Text").gameObject.GetComponent<Text>().text == "攻撃")
        {
            GameObject temp = command1;
            command1 = command2;
            command2 = command3;
            command3 = temp;
        }
        else if (command3.transform.Find("Text").gameObject.GetComponent<Text>().text == "攻撃")
        {
            //temp にスキルを代入
            GameObject temp = command1;
            //攻撃を1に
            command1 = command3;
            //逃げるを3に
            command3 = command2;
            command2 = temp;
        }

        Vector3 tempPos = command1.transform.position;
        tempPos.x = commandFixPosX;
        tempPos.y = commandFixPosY;
        tempPos.z = commandFixPosZ;
        command1.transform.localPosition = tempPos;

        tempPos = command2.transform.position;
        tempPos.x = commandFixPosX + posXShift;
        tempPos.y = commandFixPosY + posYShift;
        tempPos.z = commandFixPosZ + posZShift;
        command2.transform.localPosition = tempPos;

        tempPos = command3.transform.position;
        tempPos.x = commandFixPosX + posXShift * 2;
        tempPos.y = commandFixPosY + posYShift * 2;
        tempPos.z = commandFixPosZ + posZShift * 2;
        command3.transform.localPosition = tempPos;

        command1.GetComponent<Canvas>().sortingOrder = 5;
        command2.GetComponent<Canvas>().sortingOrder = 4;
        command3.GetComponent<Canvas>().sortingOrder = 3;
        selectedBattleCommand = 1;
        commandMode = CommandMode.Command;


    }
}
