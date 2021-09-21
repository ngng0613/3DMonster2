using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    static bool isFirst = false;
    public static int progress = 0;
    [SerializeField] SoundManager soundManager;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] MenuManager2 menuManager;
    [SerializeField] MessageBox messageBox;
    [SerializeField] StageSelectUI stageSelectUi;
    [SerializeField] PartyCreate partyCreate;
    [SerializeField] Fade fade;
    public static bool canTalk = true;

    [SerializeField] [TextArea] string[] winMessage;
    [SerializeField] [TextArea] string[] loseMessage;

    bool canOpenMenu = true;

    public void Start()
    {
        if (isFirst == false)
        {
            isFirst = true;
            monsterManager.Setup();
        }
        else
        {
            monsterManager.SetPossessionMonsterList(MonsterManager.partyMonsterList);
        }


        stageSelectUi.Setup(StartBattle, AfterClosedUi);

        SetFieldInput();

        if (fade.isActiveAndEnabled == true)
        {
            fade.FadeOut(1.0f);
        }
    }

    public void SetFieldInput()
    {
        InputManager.InputCancel = OpenPartyCreate;
        InputManager.setupCompleted = true;
    }

    public void StartBattle()
    {
        
        if (fade.isActiveAndEnabled)
        {
            fade.FadeIn(1.0f, () => SceneManager.LoadScene("Battle2"));
        }
        else
        {
            SceneManager.LoadScene("Battle2");
        }
    }

    public void OpenMenu()
    {
        menuManager.Setup(AfterClosedUi);
        menuManager.Activate();
        canTalk = false;
    }

    public void OpenPartyCreate()
    {
        partyCreate.Setup(AfterClosedUi);
        partyCreate.Activate();
        canTalk = false;
    }

    public void AfterClosedUi()
    {
        SetFieldInput();
        Player.canMove = true;
        canTalk = true;
    }

    public void BattleWin()
    {
        messageBox.Setup(winMessage, null);
        messageBox.Activate();
        progress++;

    }

    public void StartBgm()
    {

    }
}
