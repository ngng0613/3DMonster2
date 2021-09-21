using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class DamageView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Canvas canvas;
    [SerializeField] Camera useCamera;
    int damageValue = 100;
    int displayValue;
    [SerializeField] float tweenTime = 0.3f;
    string damageMessage = "";

    public bool isActive { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            DOTween.To(() => displayValue, (x) => displayValue = x, damageValue, tweenTime).SetEase(Ease.InSine);
            if (damageMessage != "")
            {
                text.text = damageMessage + "\n" + displayValue;
            }
            else
            {
                text.text = displayValue + "";
            }

        }

    }

    public void Setup(int damageValue, string damageMessage, Color textColor, Camera camera)
    {

        useCamera = camera;
        canvas.worldCamera = useCamera;
        this.damageValue = damageValue;
        if (damageMessage != "")
        {
            this.damageMessage = damageMessage;
            text.text = damageMessage + "\n" + 0;
            text.color = textColor;
        }
        else
        {
            damageMessage = "";
            text.text = 0 + "";
            text.color = textColor;
        }
        text.transform.localScale = Vector3.one;

    }

}
