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


    public void DisplayPasiveSkill(MonsterBase monster)
    {
        _viewNameText.text = monster.GetNickname();
        _viewImage.sprite = monster.Image;
        _statusView.SetActive(false);
        _passiveSkillView.SetActive(true);
        PassiveSkillBase pSkill = monster.GetPassiveSkill();
        _passiveSkillName.text = pSkill.pSkillName;
        _passiveSkillHelpText.text = pSkill.GetPSkillHelp();

    }
}
