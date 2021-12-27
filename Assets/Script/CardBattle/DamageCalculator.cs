using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    public static DamageCalculator Instance;
    [SerializeField] StatusEffectBase _guard ;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = new DamageCalculator();
        }
    }

    public int Calculate(MonsterBase a, MonsterBase b, CardSpellBase spell)
    {
        int guardCount = 0;
        foreach (var state in b.StatusEffectList)
        {
            if (state == _guard)
            {
                guardCount = state.Count;
            }
        }
        int damage = spell.EffectValue;

        damage -= guardCount;

        return damage;
    }
}
