using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    [SerializeField] List<MonsterBase> _monsterList;
    [SerializeField] ShopManager _shopManager;

    void Awake()
    {
        if (GameManager.Instance.MonsterList.Count <= 0)
        {
            Debug.LogWarning("モンスターリストを初期化しました");
            GameManager.Instance.MonsterList = _monsterList;
            for (int i = 0; i < 3; i++)
            {
                if (i >= _monsterList.Count)
                {
                    continue;
                }
                _monsterList[i].InParty = true;
            }
            GameManager.Instance.MonsterParty.Add(_monsterList[0]);
            GameManager.Instance.MonsterParty.Add(_monsterList[1]);
            GameManager.Instance.MonsterParty.Add(_monsterList[2]);
        }
        else
        {
            foreach (var item in GameManager.Instance.MonsterList)
            {
                Debug.Log(item.NickName);
            }
            _monsterList = GameManager.Instance.MonsterList;
        }

        _shopManager.Setup();
    }
}
