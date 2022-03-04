using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameResultCanvas : MonoBehaviour
{
    [SerializeField] GameObject _resultTitle;
    [SerializeField] GameObject _resultPanel;
    Sequence _sequence;
    [SerializeField] Vector2 _titlePos;
    [SerializeField] float _displaySpeed = 1.0f;
    [SerializeField] TextMeshProUGUI _resultBattleCount;
    [SerializeField] TextMeshProUGUI _resultCaptureCount;

    [SerializeField] Image[] _monsterImageArray = new Image[6];
    [SerializeField] CardObject[] _cardObjects = new CardObject[5];

    bool _captured = false;

    [SerializeField] string _mapSceneName;

    private void Start()
    {
        DisplayUpdate();

    }
    public void DisplayUpdate()
    {
        _sequence = DOTween.Sequence();
        for (int i = 0; i < GameManager.Instance.MonsterList.Count; i++)
        {
            _monsterImageArray[i].sprite = GameManager.Instance.MonsterList[i].Image;
            _monsterImageArray[i].color = Color.white;
        }
        _resultBattleCount.text = GameManager.Instance.BattleCount.ToString();
        _resultCaptureCount.text = GameManager.Instance.CaptureCount.ToString();
    }


    public void AnimationStart()
    {
        _sequence.Append(_resultTitle.transform.DOLocalMoveX(_titlePos.x, _displaySpeed));
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(_mapSceneName);
    }

}
