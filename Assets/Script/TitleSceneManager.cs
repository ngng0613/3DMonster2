using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;
    [SerializeField] Fade _fade;
    [SerializeField] Image _buttonImage;
    [SerializeField] Color _buttonDefaultColor;
    [SerializeField] Color _buttonActiveColor;
    [SerializeField] CanvasScaler[] _canvasScalerArray;


    [SerializeField] float _fadeSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

        _fade.FadeOut(_fadeSpeed, SetInput);
    }

    void SetInput()
    {
        InputManager.InputEnter = ButtonAnimation;
        InputManager.setupCompleted = true;
    }

    void GameStart()
    {
        _soundManager.StopBgm();
        _fade.FadeIn(_fadeSpeed, LoadScene);
    }

    void ButtonAnimation()
    {
        _soundManager.PlaySe(SoundManager.SeList.GameStart);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_buttonImage.DOColor(_buttonActiveColor, 0.1f))
                .Append(_buttonImage.DOColor(_buttonDefaultColor, 0.1f))
                .Append(_buttonImage.DOColor(_buttonActiveColor, 0.1f))
                .AppendCallback(() =>  StartCoroutine(LoginAnimation()));
    }

    IEnumerator LoginAnimation()
    {

        for (float i = 1.0f; i < 5; i+= 0.1f)
        {
            foreach (var canvasScaler in _canvasScalerArray)
            {
                canvasScaler.scaleFactor = i;
            }
    
            Debug.Log(i);
            yield return 0.05f;
        }
        GameStart();

        yield break;

    }

    void LoadScene()
    {
        SceneManager.LoadScene("Front");
    }
}
