using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour
{
    //スキルID
    [SerializeReference] int skillID;
    //スキル名
    [SerializeField] string skillName;
    //スキルの威力
    [SerializeField] int damage;

    //スキルの説明
    [SerializeField][TextArea] string skillHelpText;

    //スキルのエフェクト
    [SerializeField] GameObject effect;

    //スキル効果音
    [SerializeField] AudioClip effectSound;

    //スキル説明文
    [SerializeField] Text skillDescription;

    //使用時の消費MP
    [SerializeField] int usedMp = 0;

    //使用時の反動（クールタイム）
    [SerializeField] int addedCoolTime = 100;

    //スキルタイプ
    public enum SkillType
    {
        Physical = 1,
        Magic = 2,
        Guard = 3,
        Buff = 4,
        Debuff = 5,
        Heal = 6,
        Charge = 7,


    }
    [SerializeField] SkillType skillType;
    [SerializeField] Element.BattleElement skillElement;

    public SkillBase(string name, GameObject effect)
    {
        this.skillName = name;
        this.effect = effect;
    }

    public void SetOption(string name, GameObject effect)
    {
        this.skillName = name;
        this.effect = effect;

    }

    public string GetName()
    {
        return skillName;
    }

    public int GetDamage()
    {
        return damage;
    }

    public string GetHelptext()
    {
        return skillHelpText;
    }

    public SkillType GetSkillType()
    {
        return skillType;
    }

    public GameObject GetEffect()
    {
        return effect;

    }

    public int GetCoolTime()
    {
        return addedCoolTime;
    }
    public string GetDescription()
    {
        skillDescription = gameObject.GetComponent<Text>();

        return skillDescription.text;

    }
    public int GetUsedMp()
    {
        return usedMp;
    }
    public AudioClip GetEffectSE()
    {
        return effectSound;
    }
    public Element.BattleElement GetElement()
    {
        return skillElement;
    }
}
