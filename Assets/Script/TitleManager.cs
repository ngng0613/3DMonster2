using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;

    [SerializeField] Fade _fade;
    [SerializeField] string _fieldSceneName;
    [SerializeField] Vector3 _firstPlayerPos;

    public void Start()
    {
        GameManager.Instance.PlayeraPos = _firstPlayerPos;
    }

    public void ChangeScene()
    {
        _fade.AfterFunction = GoToField;
        _fade.FadeOut();
    }

    public void GoToField()
    {
        SceneManager.LoadScene(_fieldSceneName);
    }
}
