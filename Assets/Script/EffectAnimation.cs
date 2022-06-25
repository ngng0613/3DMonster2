using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    [SerializeField] List<AnimationData> _animationList = new List<AnimationData>();
}

[System.Serializable]
public class AnimationData
{
    public string Name;
    public Sprite Sprite;
    public Vector2 ImageCutSetting = new Vector2();
}