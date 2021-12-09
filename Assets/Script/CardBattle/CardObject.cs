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
    TextMeshProUGUI nameText;
    Image image;

    private void UpdateText()
    {
        nameText.text = cardData.cardName;
        image.sprite = cardData.mainImage;
    }

    public void OnPointerEnter()
    {
        this.gameObject.transform.DOLocalMoveY(64, 0.2f);
        canvas.sortingOrder = 1;
    }
    public void OnPointerExit()
    {
        this.gameObject.transform.DOLocalMoveY(0, 0.2f);
        canvas.sortingOrder = 0;
    }
}
