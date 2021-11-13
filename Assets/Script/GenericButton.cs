using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GenericButton : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] Image backgroundImage;

    [SerializeField] Color activeColor;

    Coroutine coroutine;
    [SerializeField] bool isBlink = true;
    [SerializeField] float tweenSpeed = 1.0f;
    [SerializeField] float blinkSpeed = 0.5f;
    Sequence sequence;
    Color defaultColor;

    public bool IsBlink { get => isBlink; set => isBlink = value; }

    private void Start()
    {
        IsBlink = false;
        defaultColor = backgroundImage.color;
        StartCoroutine(Blink());

    }

    public void OnClick()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (isActive == false)
        {
            isActive = true;
            backgroundImage.color = activeColor;
        }
        else
        {
            isActive = false;
            backgroundImage.color = Color.white;
        }
        coroutine = StartCoroutine(Animation());
        IsBlink = false;
    }

    IEnumerator Animation()
    {
        while (this.transform.localScale.x >= 0.65f)
        {
            this.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f) * tweenSpeed;
            yield return null;
        }
        while (this.transform.localScale.x <= 1.3f)
        {
            this.transform.localScale += new Vector3(0.06f, 0.06f, 0.06f) * tweenSpeed;
            yield return null;
        }
        while (this.transform.localScale.x >= 1.0f)
        {
            this.transform.localScale -= new Vector3(0.07f, 0.07f, 0.07f) * tweenSpeed;
            yield return null;
        }
        this.transform.localScale = Vector3.one;
        yield return null;
    }

    IEnumerator Blink()
    {
        while (true)
        {
            while (backgroundImage.color != Color.white)
            {
                if (!IsBlink)
                {
                    backgroundImage.color = defaultColor;
                    break;
                }
                Color temp = backgroundImage.color;
                temp.r += (1 - defaultColor.r) / blinkSpeed;
                temp.g += (1 - defaultColor.g) / blinkSpeed;
                temp.b += (1 - defaultColor.b) / blinkSpeed;
                if (temp.r > 1)
                {
                    temp.r = 1;
                }
                if (temp.g > 1)
                {
                    temp.g = 1;
                }
                if (temp.b > 1)
                {
                    temp.b = 1;
                }
                backgroundImage.color = temp;
                yield return new WaitForSeconds(blinkSpeed / 100.0f);
            }
            while (backgroundImage.color.r > defaultColor.r || backgroundImage.color.g > defaultColor.g || backgroundImage.color.b > defaultColor.b)
            {
                if (!IsBlink)
                {
                    backgroundImage.color = defaultColor;
                    break;
                }
                Color temp = backgroundImage.color;
                temp.r -= (1 - defaultColor.r) / blinkSpeed;
                temp.g -= (1 - defaultColor.g) / blinkSpeed;
                temp.b -= (1 - defaultColor.b) / blinkSpeed;
                backgroundImage.color = temp;
                yield return new WaitForSeconds(blinkSpeed / 100.0f);
            }

            yield return null;
        }

    }

}
