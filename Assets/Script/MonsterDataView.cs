using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// モンスターの詳細情報を表示するクラス
/// </summary>
public class MonsterDataView : MonoBehaviour
{
    [SerializeField] GameObject statusView;
    [SerializeField] GameObject passiveSkillView;

    [SerializeField] Image viewImage;
    [SerializeField] TextMeshProUGUI viewNameText;
    [SerializeField] Image viewIcon;

    [SerializeField] int statusMaxValue = 200;
    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] Image hpGraph;
    [SerializeField] TextMeshProUGUI attackValue;
    [SerializeField] Image attackGraph;
    [SerializeField] TextMeshProUGUI defenceValue;
    [SerializeField] Image defenceGraph;
    [SerializeField] TextMeshProUGUI speedValue;
    [SerializeField] Image speedGraph;
    [SerializeField] TextMeshProUGUI spAttackValue;
    [SerializeField] Image spAttackGraph;
    [SerializeField] TextMeshProUGUI spDefenceValue;
    [SerializeField] Image spDefenceGraph;

    [SerializeField] TextMeshProUGUI passiveSkillName;
    [SerializeField] TextMeshProUGUI passiveSkillHelpText;

    public IconManager iconManager;

    public void DisPlayStatusView(MonsterBase monster)
    {
        statusView.SetActive(true);
        passiveSkillView.SetActive(false);


        viewNameText.text = monster.GetNickname();
        viewImage.sprite = monster.GetImage();
        viewIcon.sprite = iconManager.GetIconImage(monster.GetElement());
        hpValue.text = monster.GetMaxHPValue().ToString();
        hpGraph.transform.localScale = new Vector3((float)monster.GetMaxHPValue() / (statusMaxValue * 2), 1, 1);
        attackValue.text = monster.GetAttackValue().ToString();
        attackGraph.transform.localScale = new Vector3((float)monster.GetAttackValue() / statusMaxValue, 1, 1);
        defenceValue.text = monster.GetDefenceValue().ToString();
        defenceGraph.transform.localScale = new Vector3((float)monster.GetDefenceValue() / statusMaxValue, 1, 1);
        speedValue.text = monster.GetSpeedValue().ToString();
        speedGraph.transform.localScale = new Vector3((float)monster.GetSpeedValue() / statusMaxValue, 1, 1);
        spAttackValue.text = monster.GetSpAttackValue().ToString();
        spAttackGraph.transform.localScale = new Vector3((float)monster.GetSpAttackValue() / statusMaxValue, 1, 1);
        spDefenceValue.text = monster.GetSpDefenceValue().ToString();
        spDefenceGraph.transform.localScale = new Vector3((float)monster.GetSpDefenceValue() / statusMaxValue, 1, 1);
    }

    public void DisplayPasiveSkill(MonsterBase monster)
    {
        viewNameText.text = monster.GetNickname();
        viewImage.sprite = monster.GetImage();
        viewIcon.sprite = iconManager.GetIconImage(monster.GetElement());
        statusView.SetActive(false);
        passiveSkillView.SetActive(true);
        PassiveSkillBase pSkill = monster.GetPassiveSkill();
        passiveSkillName.text = pSkill.pSkillName;
        passiveSkillHelpText.text = pSkill.GetPSkillHelp();

    }
}
