using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckComposition : MonoBehaviour
{
    [SerializeField] PartOfMonsterList _partOfMonsterListPrefab;
    [SerializeField] GameObject _lane;

    [SerializeField] CardObject[][] _cardList = new CardObject[3][];

    private void Start()
    {
        PartOfMonsterList part = Instantiate(_partOfMonsterListPrefab,_lane.transform);
        part.SetMonsterSlot = SetMonsterSlot;
    }

    public void SetMonsterSlot(MonsterBase monster, int slotId)
    {
        
    
    }


}
