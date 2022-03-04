using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSub : MonoBehaviour
{
    [SerializeField] List<MonsterBase> _monsterParty;
    [SerializeField] List<int> _monsterInParty;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _monsterParty = GameManager.Instance.MonsterParty;
        _monsterInParty = GameManager.Instance.MonsterPartyIdList;
    }
}
