using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] Fade fade;
    [SerializeField] Image buttonImage;
    [SerializeField] Color buttonDefaultColor;
    [SerializeField] Color buttonActiveColor;
    [SerializeField] CanvasScaler[] canvasScalerArray;


    [SerializeField] float fadeSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

        fade.FadeOut(fadeSpeed, SetInput);
    }

    void SetInput()
    {
        InputManager.InputEnter = ButtonAnimation;
        InputManager.setupCompleted = true;
    }

    void GameStart()
    {
        soundManager.StopBgm();
        fade.FadeIn(fadeSpeed, LoadScene);
    }

    void ButtonAnimation()
    {
        soundManager.PlaySe(SoundManager.SeList.GameStart);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(buttonImage.DOColor(buttonActiveColor, 0.1f))
                .Append(buttonImage.DOColor(buttonDefaultColor, 0.1f))
                .Append(buttonImage.DOColor(buttonActiveColor, 0.1f))
                .AppendCallback(() =>  StartCoroutine(LoginAnimation()));
    }

    IEnumerator LoginAnimation()
    {

        for (float i = 1.0f; i < 5; i+= 0.1f)
        {
            foreach (var canvasScaler in canvasScalerArray)
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
