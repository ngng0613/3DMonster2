using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    public StatusEffectBase Guard;

    public int Calculate(MonsterBase a, MonsterBase b, CardSpellBase spell)
    {
        int guardCount = 0;
        foreach (var state in b.StatusEffectList)
        {
            if (state.Name == Guard.Name)
            {
                guardCount = state.Count;
                Debug.Log("ガードされている　ガード値：" + guardCount);
                state.Count -= spell.EffectValue;
            }
            else
            {
                Debug.Log("ガードされていない");
            }
        }
        int damage = spell.EffectValue;

        damage -= guardCount;
        if (damage <= 0)
        {
            damage = 0;
        }

        return damage;
    }
}
