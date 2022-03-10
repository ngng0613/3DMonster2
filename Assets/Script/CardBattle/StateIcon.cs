using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StateIcon : MonoBehaviour
{
    public StatusEffectBase Status;
    public Image IconImage;
    public TextMeshProUGUI Text;
    int _count = 0;
    [SerializeField] GameObject _iconTextParent;
    [SerializeField] TextMeshProUGUI _iconText;
    [SerializeField] float _tweenSpeed = 1.0f;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            Text.text = _count.ToString();
        }
    }
    public void Popup()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(IconImage.transform.DOScale(2, _tweenSpeed))
            .Append(IconImage.transform.DOScale(1, _tweenSpeed));
    }
}
