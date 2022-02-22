using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class MapEvent : EventBase
{
    [SerializeField] protected bool _isActive = true;

    public Action EventAction;

    public delegate void MovePositionDelegate(Vector3 pos);
    public MovePositionDelegate MovePlayer;
    [SerializeField] protected bool _isSelect = false;
    protected Player _player;
    [SerializeField] protected Sprite _spriteSelected;
    [SerializeField] protected Sprite _spriteUnselected;
    [SerializeField] protected SpriteRenderer _backgroundSprite;
    [SerializeField] protected SpriteRenderer _mainSprite;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public virtual void StartEvent()
    {
        Debug.Log(transform.position + "の座標のイベントを開始します");
        EventAction.Invoke();
    }

    public void OnMouseOver()
    {
        _isSelect = true;
        _backgroundSprite.sprite = _spriteSelected;
    }

    public void OnMouseDown()
    {
        if (IsActive == false)
        {
            return;
        }
        MovePlayer.Invoke(this.transform.position);
    }

    public void OnMouseExit()
    {
        _isSelect = false;
        _backgroundSprite.sprite = _spriteUnselected;
    }
}
