using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    public StatusEffectBase Guard;
    public StatusEffectBase Damage;

    public int Calculate(MonsterBase a, MonsterBase b, CardSpellBase spell)
    {
        int effectValue = 0;
        foreach (var state in b.StatusEffectList)
        {
            if (state.Name == Damage.Name)
            {
                effectValue += state.Count;
                Debug.Log("負傷している　負傷値：" + effectValue);
            }
            if (state.Name == Guard.Name)
            {
                int decreaseValue = spell.EffectValue + effectValue;
                effectValue += state.Count * -1;
                if (state.Count > 0)
                {
                    Debug.Log("ガードされている　ガード値：" + effectValue);
                    ///受けたダメージ分、ガード値を減らす
                    state.Count -= decreaseValue;
                    if (state.Count < 0)
                    {
                        state.Count = 0;
                    }
                }
            }

        }
        int damage = spell.EffectValue;

        damage += effectValue;
        if (damage <= 0)
        {
            damage = 0;
        }

        return damage;
    }
}
