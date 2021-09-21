using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionObject : MonoBehaviour
{
    [SerializeField] Image[] boxImages = new Image[2];

    [SerializeField] Color[] defaultColor = new Color[2];
    [SerializeField] Color[] activeColor = new Color[2];

    public void Activate()
    {
        boxImages[0].color = activeColor[0];
        if (boxImages[1] != null)
        {
            boxImages[1].color = activeColor[1];
        }

    }
    public void Inactivate()
    {
        boxImages[0].color = defaultColor[0];
        if (boxImages[1] != null)
        {
            boxImages[1].color = defaultColor[1];
        }
    }
}
