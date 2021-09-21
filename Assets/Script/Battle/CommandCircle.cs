using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCircle : MonoBehaviour
{
    public Image maskImage;


    public void ChangeColor(Color color)
    {
        maskImage.color = color;
    }
}
