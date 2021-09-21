using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillBase : MonoBehaviour
{
    public enum PSkillType
    {
        Power = 10,
        Defence = 11,

        Red = 50,
        Blue = 51,
        Yellow = 52,
        Green = 53,

    }

    public string pSkillName;
    [SerializeField] [TextArea] string pSkillHelpText;
    public float attack = 1;
    public float defence = 1;
    public float spAttack = 1;
    public float spDefence = 1;
    public float speed = 1;

    public float elementalBuff = 1;
    public Element.BattleElement elementalType;

    public string GetPSkillHelp()
    {
        return pSkillHelpText;
    }

}
