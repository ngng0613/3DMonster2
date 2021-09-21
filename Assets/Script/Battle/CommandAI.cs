using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAI : MonoBehaviour
{
    //List<SkillBase> skillList = new List<SkillBase>();


    public SkillBase ChoiceSkill(List<SkillBase> skillList)
    {
        return skillList[Random.Range(0, skillList.Count)];

    }
}
