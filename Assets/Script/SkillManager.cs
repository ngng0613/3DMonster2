using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] List<SkillBase> skillList = new List<SkillBase>();

    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    [SerializeField] SkillBase skillTemp;
    [SerializeField] GameObject effectTemp;

    enum Skill
    {
        Skill_Null = 0,
        Hit1 = 1,


    }

    private void Start()
    {

        effectList.Add(effectTemp);


    }

    public SkillBase GetDefaultSkill()
    {
        return skillTemp;
    }
    /// <summary>
    /// 引数のIDをもとに、スキルリストから該当するスキルベースを探します。
    /// </summary>
    /// <param name="id">探したいスキルID</param>
    /// <returns>該当スキルベース</returns>
    public SkillBase SkillSearch(int id)
    {
        return skillList[id];

    }
}
