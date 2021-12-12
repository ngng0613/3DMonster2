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
    [SerializeField] GameObject _statusView;
    [SerializeField] GameObject _passiveSkillView;

    [SerializeField] Image _viewImage;
    [SerializeField] TextMeshProUGUI _viewNameText;
    [SerializeField] Image _viewIcon;

    [SerializeField] int _statusMaxValue = 200;
    [SerializeField] TextMeshProUGUI _hpValue;
    [SerializeField] Image _hpGraph;
    [SerializeField] TextMeshProUGUI _attackValue;
    [SerializeField] Image _attackGraph;
    [SerializeField] TextMeshProUGUI _defenceValue;
    [SerializeField] Image _defenceGraph;
    [SerializeField] TextMeshProUGUI _speedValue;
    [SerializeField] Image _speedGraph;
    [SerializeField] TextMeshProUGUI _spAttackValue;
    [SerializeField] Image _spAttackGraph;
    [SerializeField] TextMeshProUGUI _spDefenceValue;
    [SerializeField] Image _spDefenceGraph;

    [SerializeField] TextMeshProUGUI _passiveSkillName;
    [SerializeField] TextMeshProUGUI _passiveSkillHelpText;

    public IconManager IconManager;

    public void DisPlayStatusView(MonsterBase monster)
    {
        _statusView.SetActive(true);
        _passiveSkillView.SetActive(false);


        _viewNameText.text = monster.GetNickname();
        _viewImage.sprite = monster.GetImage();
        _viewIcon.sprite = IconManager.GetIconImage(monster.GetElement());
        _hpValue.text = monster.GetMaxHPValue().ToString();
        _hpGraph.transform.localScale = new Vector3((float)monster.GetMaxHPValue() / (_statusMaxValue * 2), 1, 1);
        _attackValue.text = monster.GetAttackValue().ToString();
        _attackGraph.transform.localScale = new Vector3((float)monster.GetAttackValue() / _statusMaxValue, 1, 1);
        _defenceValue.text = monster.GetDefenceValue().ToString();
        _defenceGraph.transform.localScale = new Vector3((float)monster.GetDefenceValue() / _statusMaxValue, 1, 1);
        _speedValue.text = monster.GetSpeedValue().ToString();
        _speedGraph.transform.localScale = new Vector3((float)monster.GetSpeedValue() / _statusMaxValue, 1, 1);
        _spAttackValue.text = monster.GetSpAttackValue().ToString();
        _spAttackGraph.transform.localScale = new Vector3((float)monster.GetSpAttackValue() / _statusMaxValue, 1, 1);
        _spDefenceValue.text = monster.GetSpDefenceValue().ToString();
        _spDefenceGraph.transform.localScale = new Vector3((float)monster.GetSpDefenceValue() / _statusMaxValue, 1, 1);
    }

    public void DisplayPasiveSkill(MonsterBase monster)
    {
        _viewNameText.text = monster.GetNickname();
        _viewImage.sprite = monster.GetImage();
        _viewIcon.sprite = IconManager.GetIconImage(monster.GetElement());
        _statusView.SetActive(false);
        _passiveSkillView.SetActive(true);
        PassiveSkillBase pSkill = monster.GetPassiveSkill();
        _passiveSkillName.text = pSkill.pSkillName;
        _passiveSkillHelpText.text = pSkill.GetPSkillHelp();

    }
}
