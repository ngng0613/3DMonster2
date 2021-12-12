using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public static bool IsFirst = false;
    public static int Progress = 0;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] MonsterManager _monsterManager;
    [SerializeField] MenuManager2 _menuManager;
    [SerializeField] MessageBox _messageBox;
    [SerializeField] StageSelectUI _stageSelectUi;
    [SerializeField] PartyCreate _partyCreate;
    [SerializeField] Fade _fade;
    public static bool CanTalk = true;

    [SerializeField] [TextArea] string[] _winMessage;
    [SerializeField] [TextArea] string[] _loseMessage;

    bool _canOpenMenu = true;

    public void Start()
    {
        if (IsFirst == false)
        {
            IsFirst = true;
            _monsterManager.Setup();
        }
        else
        {
            _monsterManager.SetPossessionMonsterList(MonsterManager.PartyMonsterList);
        }


        _stageSelectUi.Setup(StartBattle, AfterClosedUi);

        SetFieldInput();

        if (_fade.isActiveAndEnabled == true)
        {
            _fade.FadeOut(1.0f);
        }
    }

    public void SetFieldInput()
    {
        InputManager.InputCancel = OpenPartyCreate;
        InputManager.setupCompleted = true;
    }

    public void StartBattle()
    {
        
        if (_fade.isActiveAndEnabled)
        {
            _fade.FadeIn(1.0f, () => SceneManager.LoadScene("Battle2"));
        }
        else
        {
            SceneManager.LoadScene("Battle2");
        }
    }

    public void OpenMenu()
    {
        _menuManager.Setup(AfterClosedUi);
        _menuManager.Activate();
        CanTalk = false;
    }

    public void OpenPartyCreate()
    {
        _partyCreate.Setup(AfterClosedUi);
        _partyCreate.Activate();
        CanTalk = false;
    }

    public void AfterClosedUi()
    {
        SetFieldInput();
        Player.CanMove = true;
        CanTalk = true;
    }

    public void BattleWin()
    {
        _messageBox.Setup(_winMessage, null);
        _messageBox.Activate();
        Progress++;

    }

    public void StartBgm()
    {

    }
}
