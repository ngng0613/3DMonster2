using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;



public class MessageBox : MonoBehaviour
{
    [SerializeField] GameObject _messageBox;
    public bool IsActive = false;

    [TextArea] public string[] TextArray;
    [SerializeField] public int ViewCharCount = 0;
    [SerializeField] public TextMeshProUGUI TextView;
    [SerializeField] public float DisplaySpeed = 0.05f;
    [SerializeField] public Vector3 DisplayPos;
    [SerializeField] public Vector3 HidePos;
    [SerializeField] public float TweenSpeed = 0.3f;

    public delegate void Func();
    Func _afterFunction;

    public void Setup(string[] textArray, Func AfterFunction)
    {
        if (IsActive == true)
        {
            return;
        }
        this.TextArray = textArray;
        this._afterFunction = AfterFunction;
        _messageBox.transform.localPosition = HidePos;
        _messageBox.SetActive(true);
    }

    public void Activate()
    {
        if (IsActive == true)
        {
            return;
        }
        IsActive = true;
        Player.CanMove = false;
        InputManager.ResetInputSettings();
        _messageBox.transform.DOLocalMoveY(DisplayPos.y, TweenSpeed);
        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        for (int i = 0; i < TextArray.Length; i++)
        {
            yield return null;
            ViewCharCount = 0;
            for (int k = 0; k < TextArray[i].Length; k++)
            {
                TextView.text = TextArray[i].Substring(0, ViewCharCount);
                ViewCharCount++;

                //決定ボタンで一気に表示機能：保留
                /*
                if (FuncFunc() == true)
                {
                    viewCharCount = text[i].Length;
                    textView.text = text[i].Substring(0, viewCharCount);
                    k = text[i].Length - 1;
                }
                */

                yield return new WaitForSeconds(DisplaySpeed);
            }
            TextView.text = TextArray[i].Substring(0, ViewCharCount);

            yield return new WaitUntil(FuncFunc);   //以下の処理が終わったら次の文へ
        }
        _messageBox.transform.DOLocalMoveY(HidePos.y, TweenSpeed);
        
        Player.CanMove = true;
        
        if (_afterFunction != null)
        {
            _afterFunction.Invoke();
        }

        IsActive = false;
        yield break;
    }

    bool FuncFunc()
    {
        if (Input.GetButtonDown("決定"))
        {
            return true;

        }
        return false;

    }
}
