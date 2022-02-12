using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject _resultTitle;
    [SerializeField] GameObject _resultPanel;
    Sequence _sequence;
    [SerializeField] Vector2 _titlePos;
    [SerializeField] float _displaySpeed = 1.0f;
    MonsterBase _enemyMonster;
    [SerializeField] TextMeshProUGUI _resultMoneyText;

    [SerializeField] Image _captureEnemyButtonImage;

    bool _captured = false;

    public void Setup(MonsterBase monster)
    {
        _enemyMonster = monster;
        _resultMoneyText.text = _enemyMonster.Money.ToString();
    }

    private void Start()
    {
        _sequence = DOTween.Sequence();
    }
    public void AnimationStart()
    {
        _sequence.Append(_resultTitle.transform.DOLocalMoveX(_titlePos.x, _displaySpeed))
                 .Insert(0, _resultPanel.transform.DOScaleY(1.0f, _displaySpeed));

    }

    public void CaptureMonster()
    {
        if (_captured == true)
        {
            return;
        }
        _captureEnemyButtonImage.color = Color.gray;
        GameManager.Instance.MonsterList.Add(_enemyMonster);
    }

}
