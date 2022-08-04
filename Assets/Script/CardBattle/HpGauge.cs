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

    int _maxHp = 0;
    int _currentHp = 0;
    float _displayHp = 0;
    int _currentMp = 0;
    [SerializeField] Camera _cameraComponent;

    [SerializeField] bool _updateGauge = false;
    [SerializeField] float _tweenTime = 1.0f;
    [SerializeField] bool _lookCamera = false;

    private void Update()
    {
        if (!_updateGauge)
        {
            return;
        }
        _hpGauge.transform.localScale = new Vector3(_displayHp / _maxHp, 1, 1);
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="name">表示する名前</param>
    /// <param name="maxHp">最大HP</param>
    public void SetData(string name, int maxHp)
    {
        _nameText.text = name;
        _maxHp = maxHp;
    }

    /// <summary>
    /// プレイヤーの残りHPを参照し、HPの値を更新する
    /// </summary>
    public void UpdateHp()
    {
        _currentHp = GameManager.Instance.PlayerCurrentHp;

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() => { _updateGauge = true; })
            .Append(DOTween.To(() => _displayHp, (x) => { _displayHp = x; _hpText.text = $"{(int)_displayHp} / {_maxHp} "; }, _currentHp, _tweenTime))
            .OnComplete(() => { _updateGauge = false; });
    }
    /// <summary>
    /// プレイヤーの残りMPを参照し、MPの値を更新する
    /// </summary>
    public void UpdateMp()
    {
        _currentMp = GameManager.Instance.PlayerCurrentMp;
        _mpText.text = _currentMp.ToString();
    }
    /// <summary>
    /// 指定されたモンスターの残りHPを参照し、HPの値を更新する
    /// </summary>
    /// <param name="monster">参照するモンスター</param>
    public void UpdateHp(MonsterBase monster)
    {
        _currentHp = monster.CurrentHp;

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() => { _updateGauge = true; })
            .Append(DOTween.To(() => _displayHp, (x) => { _displayHp = x; _hpText.text = $"{(int)_displayHp} / {_maxHp} "; }, _currentHp, _tweenTime))
            .OnComplete(() => { _updateGauge = false; });
    }
    /// <summary>
    /// 指定されたモンスターの残りMPを参照し、MPの値を更新する
    /// </summary>
    /// <param name="monster">参照するモンスター</param>
    public void UpdateMp(MonsterBase monster)
    {
        _currentMp = monster.CurrentMp;
        _mpText.text = _currentMp.ToString();
    }

}
