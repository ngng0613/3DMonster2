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
    [SerializeField] bool _lookCamera = false;



    private void Update()
    {
        if (!_setupCompleted)
        {
            return;
        }
        _hpGauge.transform.localScale = new Vector3(_displayHp / _maxHp, 1, 1);


    }

    public void Setup(string monsterName, int maxHp, int currentHp, Camera cameraComponent, Sprite elementIcon)
    {
        _monsterName = monsterName;
        _nameText.text = monsterName;
        _maxHp = maxHp;
        _currentHp = currentHp;
        _displayHp = currentHp;

        _cameraComponent = cameraComponent;
        if (elementIcon != null)
        {
            _elementImage.sprite = elementIcon;
        }
        UpdateStatus(_currentHp);
        if (_cameraComponent != null && _lookCamera == true)
        {
            transform.rotation = _cameraComponent.transform.rotation;
        }
        _setupCompleted = true;
    }

    public void UpdateStatus(int currentHp)
    {
        _currentHp = currentHp;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => _displayHp, (x) => { _displayHp = x; _hpText.text = $"{(int)_displayHp} / {_maxHp} "; }, _currentHp, _tweenTime));

    }

}
