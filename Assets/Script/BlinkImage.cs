using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlinkImage : MonoBehaviour
{
    Image myImage;
    Color defaultColor;
    [SerializeField] float speed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        defaultColor = myImage.color; 
        myImage.DOFade(0.2f, speed).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    public void BlinkOnOff(bool isActive)
    {
        if (isActive)
        {
            myImage.DOFade(0.2f, speed).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            myImage.color = defaultColor;
            myImage.DOKill();
        }

    }
}
