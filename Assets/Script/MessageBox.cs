using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;



public class MessageBox : MonoBehaviour
{
    [SerializeField] GameObject messageBox;
    public bool isActive = false;

    [TextArea] public string[] textArray;
    [SerializeField] public int viewCharCount = 0;
    [SerializeField] public TextMeshProUGUI textView;
    [SerializeField] public float displaySpeed = 0.05f;
    [SerializeField] public Vector3 displayPos;
    [SerializeField] public Vector3 hidePos;
    [SerializeField] public float tweenSpeed = 0.3f;

    public delegate void Func();
    Func AfterFunction;

    public void Setup(string[] textArray, Func AfterFunction)
    {
        if (isActive == true)
        {
            return;
        }
        this.textArray = textArray;
        this.AfterFunction = AfterFunction;
        messageBox.transform.localPosition = hidePos;
        messageBox.SetActive(true);
    }

    public void Activate()
    {
        if (isActive == true)
        {
            return;
        }
        isActive = true;
        Player.canMove = false;
        InputManager.ResetInputSettings();
        messageBox.transform.DOLocalMoveY(displayPos.y, tweenSpeed);
        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        for (int i = 0; i < textArray.Length; i++)
        {
            yield return null;
            viewCharCount = 0;
            for (int k = 0; k < textArray[i].Length; k++)
            {
                textView.text = textArray[i].Substring(0, viewCharCount);
                viewCharCount++;

                //決定ボタンで一気に表示機能：保留
                /*
                if (FuncFunc() == true)
                {
                    viewCharCount = text[i].Length;
                    textView.text = text[i].Substring(0, viewCharCount);
                    k = text[i].Length - 1;
                }
                */

                yield return new WaitForSeconds(displaySpeed);
            }
            textView.text = textArray[i].Substring(0, viewCharCount);

            yield return new WaitUntil(FuncFunc);   //以下の処理が終わったら次の文へ
        }
        messageBox.transform.DOLocalMoveY(hidePos.y, tweenSpeed);
        
        Player.canMove = true;
        
        if (AfterFunction != null)
        {
            AfterFunction.Invoke();
        }

        isActive = false;
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
