using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HpGauge : MonoBehaviour
{
    public bool IsActive { get; set; } = true;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] Image _hpGauge;
    [SerializeField] Image _elementImage;

    string _monsterName = "";
    int _maxHp = 0;
    int _currentHp = 0;
    float _displayHp = 0;
    [SerializeField] Camera _cameraComponent;

    [SerializeField] bool _setupCompleted = false;
    [SerializeField] float _tweenTime = 1.0f;

    private void Update()
    {
        if (!_setupCompleted)
        {
            return;
        }
        _hpGauge.transform.localScale = new Vector3(_displayHp / _maxHp, 1, 1);
        if (_cameraComponent != null)
        {
            this.transform.rotation = _cameraComponent.transform.rotation;
        }

    }


    public void Setup(string monsterName, int maxHp, int currentHp, Camera cameraComponent, Sprite elementIcon)
    {
        this._monsterName = monsterName;
        _nameText.text = monsterName;
        this._maxHp = maxHp;
        this._currentHp = currentHp;
        _displayHp = currentHp;

        this._cameraComponent = cameraComponent;
        _elementImage.sprite = elementIcon;
        UpdateStatus(this._currentHp);
        _setupCompleted = true;
    }

    public void UpdateStatus(int currentHp)
    {
        this._currentHp = currentHp;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => this._displayHp, (x) => { this._displayHp = x; _hpText.text = $"{(int)_displayHp} / {_maxHp} "; }, this._currentHp, _tweenTime));

    }

    public void Display(bool onOff)
    {

    }
}
