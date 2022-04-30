using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] List<SkillBase> _skillList = new List<SkillBase>();

    [SerializeField] List<GameObject> _effectList = new List<GameObject>();

    [SerializeField] SkillBase _skillTemp;
    [SerializeField] GameObject _effectTemp;

    enum Skill
    {
        Skill_Null = 0,
        Hit1 = 1,


    }

    private void Start()
    {

        _effectList.Add(_effectTemp);


    }

    public SkillBase GetDefaultSkill()
    {
        return _skillTemp;
    }
    /// <summary>
    /// 引数のIDをもとに、スキルリストから該当するスキルベースを探します。
    /// </summary>
    /// <param name="id">探したいスキルID</param>
    /// <returns>該当スキルベース</returns>
    public SkillBase SkillSearch(int id)
    {
        return _skillList[id];

    }
}
