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
    [SerializeField]TextMeshProUGUI nameText;
    [SerializeField]Image image;

    Vector3 relativePos;
    Vector3 firstMousePos;

    public delegate void Func();
    Func handUpdateDelegate;
        
    public void Setup(Func func)
    {
        handUpdateDelegate = func;
        UpdateText();
    }

    private void UpdateText()
    {
        nameText.text = cardData.cardName;
        image.sprite = cardData.mainImage;
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
        relativePos = this.gameObject.transform.position;
        firstMousePos = Input.mousePosition;
        Debug.Log($"relative = {relativePos},  firstPos = {firstMousePos}");
    }

    public void OnMouseDrag()
    {
        this.gameObject.transform.position = relativePos + Input.mousePosition - firstMousePos;
    }

    public void OnMouseUp()
    {
        handUpdateDelegate.Invoke();
    }

    public void ResetSortingOrder()
    {
        canvas.sortingOrder = 0;
    }
}
