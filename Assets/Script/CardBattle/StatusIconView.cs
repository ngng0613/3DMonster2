﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StatusIconView : MonoBehaviour
{
    [SerializeField] List<StatusEffectBase> StatusEffectBaseList;
    List<StateIcon> _stateIconList = new List<StateIcon>();
    [SerializeField] StateIcon _stateIconPrefab;
    public void UpdateView(List<StatusEffectBase> statusEffectBaseList)
    {
        StatusEffectBaseList = statusEffectBaseList;
        for (int i = 0; i < _stateIconList.Count; i++)
        {
            Destroy(_stateIconList[i].gameObject);
        }
        _stateIconList.Clear();
        for (int i = 0; i < StatusEffectBaseList.Count; i++)
        {
            StatusEffectBase statusEffect = StatusEffectBaseList[i];
            int tempCount = statusEffect.Count;
            if (tempCount <= 0)
            {
                StatusEffectBaseList.Remove(statusEffect);
                if (StatusEffectBaseList.Count > 0)
                {
                    i--;
                }
                continue;
            }
            StateIcon icon = Instantiate(_stateIconPrefab);
            icon.Status = statusEffect;
            icon.Count = tempCount;
            icon.transform.SetParent(this.gameObject.transform);
            icon.transform.localPosition = Vector3.zero;
            icon.IconImage.sprite = statusEffect.IconSprite;
            icon.transform.localEulerAngles = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            _stateIconList.Add(icon);
        }
    }

    public void IconPopup(StatusEffectBase status)
    {
        for (int i = 0; i < _stateIconList.Count; i++)
        {
            if (_stateIconList[i].Status.Name == status.Name)
            {
                _stateIconList[i].Popup();
            }
        }
    }
}

