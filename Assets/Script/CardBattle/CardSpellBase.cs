using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum SpellType
{
    Attack,
    Guard,
    Buff,
    Debuff,
    Draw,
    DisCard,
}

public enum SpellTarget
{
    Enemy,
    Player,
}

[System.Serializable]
public class CardSpellBase
{
    public SpellType Type = SpellType.Attack;
    public SpellTarget Target;
    public int EffectValue = 0;
    public StatusEffectBase Status;
    public AudioClip SpellSound;
    
}
