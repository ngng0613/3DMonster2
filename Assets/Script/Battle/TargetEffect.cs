using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TargetEffect : MonoBehaviour
{
    [SerializeField] Image circleImage;
    Sequence sequence;
    [SerializeField] Quaternion dir;

    private void Start()
    {
        sequence = DOTween.Sequence();
        circleImage.transform.DOLocalRotate(new Vector3(0, 0, 360), 6f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
