using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardData : ScriptableObject
{
    public string _cardName;
    public Sprite _mainImage;
    public int _cost;
    public SkillBase _skill;
}
