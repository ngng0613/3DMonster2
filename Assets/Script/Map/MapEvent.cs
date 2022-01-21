using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class MapEvent : EventBase
{
    public Action EventAction;

    public delegate void MovePositionDelegate(Vector3 pos);
    public MovePositionDelegate MovePlayer;
    [SerializeField] bool _isSelect = false;
    Player _player;
    [SerializeField] Sprite _spriteSelected;
    [SerializeField] Sprite _spriteUnselected;
    [SerializeField] SpriteRenderer _spriteRenderer;

    public void OnMouseOver()
    {
        _isSelect = true;
        _spriteRenderer.sprite = _spriteSelected;
    }

    public void OnMouseDown()
    {
        MovePlayer.Invoke(this.transform.position);
    }

    public void OnMouseExit()
    {
        _isSelect = false;
        _spriteRenderer.sprite = _spriteUnselected;
     
    }
}
