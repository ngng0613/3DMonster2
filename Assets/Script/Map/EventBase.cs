﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBase : MonoBehaviour
{
    public UnityEvent EventTask = new UnityEvent();

    public virtual void Setup()
    {
                
    }
}
