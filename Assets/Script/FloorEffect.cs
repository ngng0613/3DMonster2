using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloorEffect : MonoBehaviour
{
    [SerializeField] Image myImage;
    [SerializeField] float tweenSpeed = 0.5f;
    [SerializeField] float maxSize = 5;
    // Start is called before the first frame update
    void Start()
    {
        EffectProcess();
    }

    void EffectProcess()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(myImage.transform.DOScale(maxSize, tweenSpeed))
            .Insert(0.0f, myImage.DOFade(0.0f, tweenSpeed * 0.8f))
            .AppendCallback(() => Destroy(this.gameObject));
    }
}
