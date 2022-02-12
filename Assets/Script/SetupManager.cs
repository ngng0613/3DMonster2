using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    [SerializeField] List<MonsterBase> _monsterList;

    void Awake()
    {
        GameManager.Instance.MonsterList = _monsterList;
    }

}
