using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleMessage : MonoBehaviour
{
    [SerializeField] GameObject messagePrefab;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Vector3 messagePos;
    [SerializeField] Image backgroundImage;
    Sequence sequence;
    [SerializeField] float scalingTime;

    public void UpdateMessage(string messageText)
    {
        sequence = DOTween.Sequence();
        if (this.gameObject.transform.localScale.y < 1)
        {
            sequence.Append(this.gameObject.transform.DOScaleY(0.0f, 0.0f));
        }

        this.messageText.text = messageText;

        sequence.Append(this.gameObject.transform.DOScaleY(1.0f, scalingTime));
    }
    public void CloseMessage()
    {
        sequence = DOTween.Sequence();

        sequence.Append(this.gameObject.transform.DOScaleY(0.0f, scalingTime));

    }
}
