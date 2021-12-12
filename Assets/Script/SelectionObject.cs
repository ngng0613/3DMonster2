using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionObject : MonoBehaviour
{
    [SerializeField] Image[] _boxImages = new Image[2];

    [SerializeField] Color[] _defaultColor = new Color[2];
    [SerializeField] Color[] _activeColor = new Color[2];

    public void Activate()
    {
        _boxImages[0].color = _activeColor[0];
        if (_boxImages[1] != null)
        {
            _boxImages[1].color = _activeColor[1];
        }

    }
    public void Inactivate()
    {
        _boxImages[0].color = _defaultColor[0];
        if (_boxImages[1] != null)
        {
            _boxImages[1].color = _defaultColor[1];
        }
    }
}
