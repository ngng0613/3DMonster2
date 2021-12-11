using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardObject : MonoBehaviour
{
    [SerializeField] CardData cardData;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image image;

    Vector3 relativePos;
    Vector3 firstMousePos;

    public delegate void Func();
    Func handUpdateDelegate;

    Action playedAction;

    public CardData CardData { get => cardData;}
    public bool InHand = false;

    public void Setup(Func func)
    {
        handUpdateDelegate = func;
        UpdateText();
    }

    /// <summary>
    /// カードをプレイした際の処理を追加する
    /// </summary>
    /// <param name="func">追加したい関数</param>
    public void SetAction(Func func)
    {
        playedAction += () => func();
    }

    private void UpdateText()
    {
        nameText.text = CardData.cardName;
        image.sprite = CardData.mainImage;
    }

    IEnumerator PlayUpdate()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("解除");
                if (Input.mousePosition.y >= 600)
                {
                    Debug.Log($"{this.CardData.cardName}をプレイした");
                    if (playedAction != null)
                    {
                        playedAction.Invoke();
                    }
                }
                break;
            }
            yield return null;
        }
        yield return null;
    }

    public void OnPointerEnter()
    {
        this.gameObject.transform.DOLocalMoveY(156, 0.2f);
        this.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        canvas.sortingOrder = 1;
    }
    public void OnPointerExit()
    {
        this.gameObject.transform.DOLocalMoveY(0, 0.2f);
        this.gameObject.transform.DOScale(Vector3.one, 0.2f);
        canvas.sortingOrder = 0;
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            relativePos = this.gameObject.transform.position;
            firstMousePos = Input.mousePosition;
            StartCoroutine(PlayUpdate());
        }
    }

    public void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            this.gameObject.transform.position = relativePos + Input.mousePosition - firstMousePos;
        }
    }

    public void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            handUpdateDelegate.Invoke();
        }
    }

    public void ResetSortingOrder()
    {
        canvas.sortingOrder = 0;
    }
}
