using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour
{
    //スキルID
    [SerializeReference] int _skillID;
    //スキル名
    [SerializeField] string _skillName;
    //スキルの威力
    [SerializeField] int _damage;

    //スキルの説明
    [SerializeField][TextArea] string _skillHelpText;

    //スキルのエフェクト
    [SerializeField] GameObject _effect;

    //スキル効果音
    [SerializeField] AudioClip _effectSound;

    //スキル説明文
    [SerializeField] Text _skillDescription;

    //使用時の消費MP
    [SerializeField] int _usedMp = 0;

    //使用時の反動（クールタイム）
    [SerializeField] int _addedCoolTime = 100;

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
    [SerializeField] SkillType _skillType;

    public SkillBase(string name, GameObject effect)
    {
        this._skillName = name;
        this._effect = effect;
    }

    public void SetOption(string name, GameObject effect)
    {
        this._skillName = name;
        this._effect = effect;

    }

    public string GetName()
    {
        return _skillName;
    }

    public int GetDamage()
    {
        return _damage;
    }

    public string GetHelptext()
    {
        return _skillHelpText;
    }

    public SkillType GetSkillType()
    {
        return _skillType;
    }

    public GameObject GetEffect()
    {
        return _effect;

    }

    public int GetCoolTime()
    {
        return _addedCoolTime;
    }
    public string GetDescription()
    {
        _skillDescription = gameObject.GetComponent<Text>();

        return _skillDescription.text;

    }
    public int GetUsedMp()
    {
        return _usedMp;
    }
    public AudioClip GetEffectSE()
    {
        return _effectSound;
    }
}
