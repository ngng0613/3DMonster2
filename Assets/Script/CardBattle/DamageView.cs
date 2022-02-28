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
    [SerializeField] float _moveXRange;
    int _damageValue = 0;
    int _displayValue = 0;
    [SerializeField] float _tweenTime = 0.3f;
    string _damageMessage = "";

    Vector3 _randomPos;
    [SerializeField] float _jumpPower = 1.0f;
    Sequence _sequence;

    public bool IsActive { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (IsActive == true)
        {
            string damageString = "";
            if (_damageValue > 0)
            {
                damageString = _damageValue.ToString();
            }
            DOTween.To(() => _displayValue, (x) => { _displayValue = x; Debug.Log(x); }, _damageValue, _tweenTime).SetEase(Ease.InSine);
            if (_damageMessage != "")
            {
                _text.text = _damageMessage + "\n" + damageString;
            }
            else
            {
                _text.text = damageString + "";
            }
        }

    }

    public void Setup(int damageValue, string damageMessage, Color textColor, Camera camera)
    {
        _sequence = DOTween.Sequence();
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        _randomPos = this.gameObject.transform.position;
        Debug.Log("Pos" + _randomPos);
        _randomPos.x += UnityEngine.Random.Range(-1 * _moveXRange, _moveXRange);
        _useCamera = camera;
        _canvas.worldCamera = _useCamera;
        this._damageValue = damageValue;

        if (damageMessage != "")
        {
            this._damageMessage = damageMessage;

            _text.color = textColor;
        }
        else
        {
            damageMessage = "";

            _text.color = textColor;
        }
        _text.transform.localScale = Vector3.one;

    }

    public void Activate()
    {

        _sequence.Append(
        DOTween.To(() => _displayValue, (x) =>
            {
                _displayValue = x;
                string damageString = "";
                if (_displayValue > 0)
                {
                    damageString = _displayValue.ToString();
                }
                if (_damageMessage != "")
                {
                    _text.text = _damageMessage + "\n" + damageString;

                }
                else
                {
                    _text.text = damageString;
                }


            }, _damageValue, _tweenTime).SetEase(Ease.InSine)
        );

        if (_damageValue > 0)
        {
            _sequence.Join(
             this.gameObject.transform.DOJump(_randomPos, _jumpPower, 1, _tweenTime));
        };



        StartCoroutine(WaitForSeconds(1));
    }

    IEnumerator WaitForSeconds(int x)
    {
        yield return new WaitForSeconds(x);
        Destroy(this.gameObject);
    }

}
