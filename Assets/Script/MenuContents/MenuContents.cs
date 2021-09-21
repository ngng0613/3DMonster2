using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuContents : MonoBehaviour
{
    public delegate void Func();

    public abstract void Setup(Func func);
    public abstract void Activate(); 
    
}