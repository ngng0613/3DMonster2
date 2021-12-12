using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloorEffect : MonoBehaviour
{
    [SerializeField] Image _myImage;
    [SerializeField] float _tweenSpeed = 0.5f;
    [SerializeField] float _maxSize = 5;
    // Start is called before the first frame update
    void Start()
    {
        EffectProcess();
    }

    void EffectProcess()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_myImage.transform.DOScale(_maxSize, _tweenSpeed))
            .Insert(0.0f, _myImage.DOFade(0.0f, _tweenSpeed * 0.8f))
            .AppendCallback(() => Destroy(this.gameObject));
    }
}
