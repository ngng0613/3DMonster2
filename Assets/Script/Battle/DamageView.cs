using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class DamageView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Canvas _canvas;
    [SerializeField] Camera _useCamera;
    int _damageValue = 100;
    int _displayValue;
    [SerializeField] float _tweenTime = 0.3f;
    string _damageMessage = "";

    public bool IsActive { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (IsActive == true)
        {
            DOTween.To(() => _displayValue, (x) => _displayValue = x, _damageValue, _tweenTime).SetEase(Ease.InSine);
            if (_damageMessage != "")
            {
                _text.text = _damageMessage + "\n" + _displayValue;
            }
            else
            {
                _text.text = _displayValue + "";
            }

        }

    }

    public void Setup(int damageValue, string damageMessage, Color textColor, Camera camera)
    {

        _useCamera = camera;
        _canvas.worldCamera = _useCamera;
        this._damageValue = damageValue;
        if (damageMessage != "")
        {
            this._damageMessage = damageMessage;
            _text.text = damageMessage + "\n" + 0;
            _text.color = textColor;
        }
        else
        {
            damageMessage = "";
            _text.text = 0 + "";
            _text.color = textColor;
        }
        _text.transform.localScale = Vector3.one;

    }

}
