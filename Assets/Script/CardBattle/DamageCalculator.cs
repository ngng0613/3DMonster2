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
            Debug.Log("stateName : " + state.name + state.Count);
            Debug.Log(Guard);
            if (state.Name == Guard.Name)
            {
                Debug.Log("ガードされている");
                guardCount = state.Count;
            }
            else
            {
                Debug.Log("ガードされていない");
            }
        }
        int damage = spell.EffectValue;

        damage -= guardCount;

        return damage;
    }
}
