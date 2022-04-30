using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlinkImage : MonoBehaviour
{
    Image _myImage;
    Color _defaultColor;
    [SerializeField] float _speed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        _myImage = GetComponent<Image>();
        _defaultColor = _myImage.color; 
        _myImage.DOFade(0.2f, _speed).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    public void BlinkOnOff(bool isActive)
    {
        if (isActive)
        {
            _myImage.DOFade(0.2f, _speed).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            _myImage.color = _defaultColor;
            _myImage.DOKill();
        }

    }
}
