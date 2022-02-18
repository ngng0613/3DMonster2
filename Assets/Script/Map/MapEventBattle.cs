using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEventBattle : MapEvent
{
    [SerializeField] StageData _stage;

    private void Start()
    {
        if (_mainSprite != null && _stage != null)
        {
            _mainSprite.sprite = _stage.EnemyMonster.Image;
        }
    }

    public override void StartEvent()
    {
        GameManager.Instance.NextBattleStage = _stage;
        base.StartEvent();
    }

}
