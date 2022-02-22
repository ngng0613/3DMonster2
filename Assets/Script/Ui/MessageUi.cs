using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUi : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    Vector2 _defaultPos;
    [SerializeField] float _moveLength = 100f;
    [SerializeField] float _displaySpeed = 1.0f;

    private void Start()
    {
        _defaultPos = this.gameObject.transform.position;
    }

    public void Activate()
    {
        Debug.Log("はい");
        //this.gameObject.transform.position = new Vector2();
        StartCoroutine(ActivateCoroutine());
    }

    public IEnumerator ActivateCoroutine()
    {
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += _displaySpeed * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= _displaySpeed * Time.deltaTime;

            yield return null;
        }
    }
}
