using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillTags
{
    None = 0,
    Fire = 1,
    Ice = 2,
    Thunder = 3,
    Nature = 4,


}

public class SkillTag : MonoBehaviour
{
    public static string GetTagName(SkillTags tags)
    {
        string tagName;
        switch (tags)
        {
            case SkillTags.None:
                tagName = "無し";
                break;
            case SkillTags.Fire:
                tagName = "炎";
                break;
            case SkillTags.Ice:
                tagName = "氷";
                break;
            case SkillTags.Thunder:
                tagName = "雷";
                break;
            case SkillTags.Nature:
                tagName = "自然";
                break;
            default:
                tagName = "null";
                break;
        }

        return tagName;
    }
}
