using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fade : MonoBehaviour
{
    [SerializeField] GameObject _parentObject;
    [SerializeField] Image _mainImage;
    [SerializeField] float _fadeSpeed = 1.0f;
    [SerializeField] Image _hexImage;
    [SerializeField] Material _hexMaterial;
    [SerializeField] Color _hexFadeColor;
    [SerializeField] Image _loadImage;

    public Action AfterFunction;

    public void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        Color color = _mainImage.color;
        color.a = 0;
        _mainImage.color = color;
    }

    public void StartAnimation()
    {
        _parentObject.SetActive(true);
        _hexImage.color = _hexFadeColor;
        StartCoroutine(FadeAnimation());
    }

    IEnumerator FadeAnimation()
    {
        while (true)
        {
            Color color = _mainImage.color;
            color.a += 1f * _fadeSpeed * Time.deltaTime;
            _mainImage.color = color;
            if (_hexMaterial.HasProperty("_FadeFloat"))
            {
                _hexMaterial.SetFloat("_FadeFloat", color.a);
            }
            if (color.a >= 1)
            {
                _hexImage.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
        AfterFunction.Invoke();
    }
}
