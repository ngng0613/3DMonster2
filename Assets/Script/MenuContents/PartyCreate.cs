using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PartyCreate : MenuContents
{
    [SerializeField] GameObject partyCreateObjects;
    [SerializeField] SoundManager soundManager;
    [SerializeField] IconManager iconManager;

    [SerializeField] Image chooseRect;
    BlinkImage blinkImage;

    enum ViewMode
    {
        StatusView,
        PassiveSkillView,

    }
    [SerializeField] ViewMode viewMode = ViewMode.StatusView;

    [SerializeField] MonsterDataView monsterDataView;
    [SerializeField] MonsterManager monsterManager;

    public int choosePosition = 1;
    public int chooseIndex = 0;

    [SerializeField] Vector3[] rectPositonList = new Vector3[3];
    [SerializeField] List<MonsterBase> monsterPartyList;
    [SerializeField] List<MonsterBase> allMonsterList;

    [SerializeField] Image[] partyMonsterImageList = new Image[3];
    [SerializeField] TextMeshProUGUI[] partyMonsterNameList = new TextMeshProUGUI[3];
    [SerializeField] Image[] elementIconList = new Image[3];
 
    [SerializeField] CanvasGroup bottomContents;

    MenuContents.Func AfterClosed;

    public enum Direction
    {
        Right,
        Left,
    }

    public override void Setup(MenuContents.Func func)
    {
        this.AfterClosed = func;
        blinkImage = chooseRect.GetComponent<BlinkImage>();
        PhaseChoicePosition();
        InputManager.setupCompleted = true;
        allMonsterList = monsterManager.GetAllMonsterList();
        monsterPartyList = monsterManager.GetPossessionMonsterList().GetRange(0, GameManager2.progress + 3);
        UpdateParty();
        monsterDataView.DisPlayStatusView(allMonsterList[chooseIndex]);

    }



    public override void Activate()
    {
        partyCreateObjects.SetActive(true);
        soundManager.PlaySe(SoundManager.SeList.OpenMenu);
        Player.canMove = false;
    }

    public void Inactivate()
    {
        partyCreateObjects.SetActive(false);
        InputManager.ResetInputSettings();
        AfterClosed.Invoke();
    }

    public void PhaseChoicePosition()
    {
        bottomContents.alpha = 0.5f;
        blinkImage.BlinkOnOff(true);
        InputManager.ResetInputSettings();
        InputManager.InputEnter += PhaseChoiceMonster;
        InputManager.InputEnter += () => soundManager.PlaySe(SoundManager.SeList.Decided);
        InputManager.InputCancel += Inactivate;
        InputManager.InputCancel += () => soundManager.PlaySe(SoundManager.SeList.CloseMenu);
        InputManager.InputRight += () => MovePosition(Direction.Right);
        InputManager.InputRight += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputLeft += () => MovePosition(Direction.Left);
        InputManager.InputLeft += () => soundManager.PlaySe(SoundManager.SeList.Select);

    }
    public void PhaseChoiceMonster()
    {
        soundManager.PlaySe(SoundManager.SeList.Decided);
        bottomContents.alpha = 1;
        blinkImage.BlinkOnOff(false);
        InputManager.ResetInputSettings();
        InputManager.InputEnter += ChangeMonster;
        InputManager.InputEnter += PhaseChoicePosition;
        InputManager.InputEnter += UpdateParty;
        InputManager.InputEnter += () => soundManager.PlaySe(SoundManager.SeList.Decided);
        InputManager.InputCancel += PhaseChoicePosition;
        InputManager.InputCancel += () => soundManager.PlaySe(SoundManager.SeList.Cancel);
        InputManager.InputSubButton2 += ChangeView;
        InputManager.InputSubButton2 += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputRight += () => MoveList(Direction.Right);
        InputManager.InputRight += () => soundManager.PlaySe(SoundManager.SeList.Select);
        InputManager.InputLeft += () => MoveList(Direction.Left);
        InputManager.InputLeft += () => soundManager.PlaySe(SoundManager.SeList.Select);
    }

    public void EndProcess()
    {



    }

    public void MovePosition(Direction dir)
    {
        soundManager.PlaySe(SoundManager.SeList.Select);
        switch (dir)
        {
            case Direction.Right:
                choosePosition++;
                if (choosePosition > 2)
                {
                    choosePosition = 0;
                }
                break;
            case Direction.Left:
                choosePosition--;
                if (choosePosition < 0)
                {
                    choosePosition = 2;
                }
                break;
            default:
                break;
        }
        chooseRect.transform.localPosition = rectPositonList[choosePosition];
    }

    public void MoveList(Direction dir)
    {
        soundManager.PlaySe(SoundManager.SeList.Select);
        switch (dir)
        {
            case Direction.Right:
                chooseIndex++;
                if (chooseIndex >= allMonsterList.Count)
                {
                    chooseIndex = 0;
                }
                break;
            case Direction.Left:
                chooseIndex--;
                if (chooseIndex < 0)
                {
                    chooseIndex = allMonsterList.Count - 1;
                }
                break;
            default:
                break;
        }
        if (viewMode == ViewMode.StatusView)
        {
            monsterDataView.DisPlayStatusView(allMonsterList[chooseIndex]);
        }
        else if(viewMode == ViewMode.PassiveSkillView)
        {
            monsterDataView.DisplayPasiveSkill(allMonsterList[chooseIndex]);
        }
     
    }

    public void UpdateParty()
    {

        for (int i = 0; i < monsterPartyList.Count; i++)
        {
            partyMonsterImageList[i].sprite = monsterPartyList[i].GetImage();
            partyMonsterNameList[i].text = monsterPartyList[i].GetNickname();
            elementIconList[i].sprite = iconManager.GetIconImage(monsterPartyList[i].GetElement());
        }
        monsterManager.SetPossessionMonsterList(monsterPartyList);
    }

    public void ChangeView()
    {
        switch (viewMode)
        {
            case ViewMode.StatusView:
                monsterDataView.DisplayPasiveSkill(allMonsterList[chooseIndex]);
                viewMode = ViewMode.PassiveSkillView;
                break;

            case ViewMode.PassiveSkillView:
                monsterDataView.DisPlayStatusView(allMonsterList[chooseIndex]);
                viewMode = ViewMode.StatusView;

                break;
            default:
                break;
        }

    }

    public void ChangeMonster()
    {
        monsterPartyList[choosePosition] = allMonsterList[chooseIndex];

    }


}