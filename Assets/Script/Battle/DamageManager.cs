using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    float totalDamage;
    float attackValue;
    float defenceValue;

    public int DamageCalculator(MonsterBase actor, MonsterBase target, SkillBase skill, List<PassiveSkillBase> pSkillListPlayerSide, List<PassiveSkillBase> pSkillListEnemySide)
    {
        Debug.Log("Skill:" + skill);
        if (skill.GetSkillType() == SkillBase.SkillType.Magic)
        {
            attackValue = actor.GetSpAttackValue();
            defenceValue = target.GetSpDefenceValue();
        }
        else if (skill.GetSkillType() == SkillBase.SkillType.Physical)
        {
            attackValue = actor.GetAttackValue();
            defenceValue = target.GetDefenceValue();
        }

        float passivePower = 1;
        float passiveDefence = 1;

        //パッシブスキル補正
        if (skill.GetSkillType() == SkillBase.SkillType.Physical)
        {
            if (pSkillListPlayerSide[0] != null)
            {
                attackValue *= pSkillListPlayerSide[0].attack;
                passivePower *= pSkillListPlayerSide[0].attack;
            }
            if (pSkillListPlayerSide[1] != null)
            {
                attackValue *= pSkillListPlayerSide[1].attack;
                passivePower *= pSkillListPlayerSide[1].attack;
            }
            if (pSkillListPlayerSide[2] != null)
            {
                attackValue *= pSkillListPlayerSide[2].attack;
                passivePower *= pSkillListPlayerSide[2].attack;
            }
            if (pSkillListEnemySide[0] != null)
            {
                defenceValue *= pSkillListEnemySide[0].defence;
            }
            if (pSkillListEnemySide[1] != null)
            {
                defenceValue *= pSkillListEnemySide[1].defence;
            }
            if (pSkillListEnemySide[2] != null)
            {
                defenceValue *= pSkillListEnemySide[2].defence;
            }
        }
        else if (skill.GetSkillType() == SkillBase.SkillType.Magic)
        {
            if (pSkillListPlayerSide[0] != null)
            {
                attackValue *= pSkillListPlayerSide[0].spAttack;
            }
            if (pSkillListPlayerSide[1] != null)
            {
                attackValue *= pSkillListPlayerSide[1].spAttack;
            }
            if (pSkillListPlayerSide[2] != null)
            {
                attackValue *= pSkillListPlayerSide[2].spAttack;
            }
            if (pSkillListEnemySide[0] != null)
            {
                defenceValue *= pSkillListEnemySide[0].spDefence;
            }
            if (pSkillListEnemySide[1] != null)
            {
                defenceValue *= pSkillListEnemySide[1].spDefence;
            }
            if (pSkillListEnemySide[2] != null)
            {
                defenceValue *= pSkillListEnemySide[2].spDefence;
            }
        }
        Debug.Log("パッシヴ攻撃倍率:" + passivePower + "");

        DamageCalculatorDamageBase(attackValue, defenceValue, skill.GetDamage(), Element.CheckAdvantage(skill.GetElement(), target.GetElement()));

        if (target.status == MonsterBase.MonsterState.Guard)
        {
            totalDamage /= 2;
        }
        if (actor.status == MonsterBase.MonsterState.Charge)
        {
            Debug.Log("chargeAttack");
            totalDamage *= 1.5f;
        }


        if (totalDamage <= 0)
        {
            totalDamage = 1;
        }



        return (int)totalDamage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="skillDamage"></param>
    /// <param name="elementsAvarage">属性相性補正</param>
    /// <returns></returns>
    public int DamageCalculatorDamageBase(float a, float b, int skillDamage, float elementsAvarage)
    {
        float random = Random.Range(0.9f, 1.1f);


        //30はスキルダメージ倍率の基本値
        totalDamage = (int)((a * 14.0f / 20.0f) - (b * 1.0f / 20.0f)) * (skillDamage / 30.0f) * elementsAvarage * random;
        Debug.Log("a:" + a + " b;" + b + " ダメ" + totalDamage);
        return (int)totalDamage;
    }


}
