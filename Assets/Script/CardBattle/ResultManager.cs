using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] Image _monsterImage;
    [SerializeField] CardObject[] _cardObjects = new CardObject[5];

    [SerializeField] Image _captureEnemyButtonImage;
    [SerializeField] MessageUi _releaseMessage;
    [SerializeField] MessageUi _capturedMessage;

    bool _captured = false;

    [SerializeField] string _mapSceneName;

    public void Setup(MonsterBase monster)
    {
        _enemyMonster = GameManager.Instance.NextBattleStage.EnemyMonster;
        _resultMoneyText.text = _enemyMonster.Money.ToString();
        _monsterImage.sprite = monster.Image;
        for (int i = 0; i < 5; i++)
        {
            _cardObjects[i].Data = monster.CardDatas[i];
            _cardObjects[i].UpdateText();
        }
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
        if (GameManager.Instance.MonsterList.Count >= GameManager.Instance.MonsterMaxCount)
        {
            _releaseMessage.Activate();
            return;
        }
        _captureEnemyButtonImage.color = Color.gray;
        GameManager.Instance.MonsterList.Add(_enemyMonster);
        Debug.Log($"{_enemyMonster.NickName}を捕獲");
        _capturedMessage.Activate();
        _captured = true;
    }

    public void BackToMap()
    {
        SceneManager.LoadScene(_mapSceneName);
    }

}
