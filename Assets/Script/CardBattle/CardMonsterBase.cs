using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMonsterBase : MonoBehaviour
{
    [SerializeField] string _name = default;
    [SerializeField] List<CardData> _cardSet = new List<CardData>();
    [SerializeField] int _currentHp = 100;
    [SerializeField] int _maxHp = 100;

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp < 0)
        {
            _currentHp = 0;
        }
    }

}
