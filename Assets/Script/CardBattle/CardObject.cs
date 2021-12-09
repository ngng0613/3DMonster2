using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardObject : MonoBehaviour
{
    [SerializeField] CardData cardData;
    TextMeshProUGUI nameText;
    Image image;

    private void UpdateText()
    {
        nameText.text = cardData.cardName;
        image.sprite = cardData.mainImage;
    }
}
