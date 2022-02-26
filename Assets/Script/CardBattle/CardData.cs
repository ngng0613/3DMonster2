using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardData : ScriptableObject
{
    [SerializeField] string _cardName;
    [SerializeField] Sprite _mainImage;
    [SerializeField] int _cost;
    [TextArea, SerializeField] string _flavourText; 
    [SerializeField] List<CardSpellBase> _cardSpellBases;

    public string CardName { get => _cardName; set => _cardName = value; }
    public Sprite MainImage { get => _mainImage; set => _mainImage = value; }
    public int Cost { get => _cost; set => _cost = value; }
    public List<CardSpellBase> CardSpellBases { get => _cardSpellBases; set => _cardSpellBases = value; }
    public string FlavourText { get => _flavourText; set => _flavourText = value; }
}

