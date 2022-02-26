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
    [SerializeField] TextMeshProUGUI _mpText;
    [SerializeField] TextMeshProUGUI _maxMpText;

    string _monsterName = "";
    int _maxHp = 0;
    int _currentHp = 0;
    float _displayHp = 0;
    int _currentMp = 0;
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

    public void Setup(MonsterBase monster, Camera cameraComponent, Sprite elementIcon)
    {
        _monsterName = monster.NickName;
        _nameText.text = _monsterName;
        _maxHp = monster.MaxHp;
        _currentHp = monster.CurrentHp;
        _displayHp = _currentHp;
        _currentMp = monster.CurrentMp;
        if (_mpText != null)
        {
            _mpText.text = _currentMp.ToString();
        }
        if (_maxMpText != null)
        {
            _maxMpText.text = monster.MaxMp.ToString();
        }

        _cameraComponent = cameraComponent;
        if (elementIcon != null)
        {
            _elementImage.sprite = elementIcon;
        }
        UpdateHp(_currentHp);
        if (_cameraComponent != null && _lookCamera == true)
        {
            transform.rotation = _cameraComponent.transform.rotation;
        }
        _setupCompleted = true;
    }

    public void UpdateHp(int currentHp)
    {
        _currentHp = currentHp;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => _displayHp, (x) => { _displayHp = x; _hpText.text = $"{(int)_displayHp} / {_maxHp} "; }, _currentHp, _tweenTime));
    }
    public void UpdateMp(int currentMp)
    {
        _currentMp = currentMp;
        _mpText.text = _currentMp.ToString();
    }

}
