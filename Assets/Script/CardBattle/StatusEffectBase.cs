using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffectBase : MonoBehaviour
{
    public int Id;
    public string Name = default;
    int count = 0;

    public Sprite Icon;

    public int Count { get => count; set { count = value; Debug.Log("変更されました=>" + value); } }

}
