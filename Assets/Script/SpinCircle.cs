using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCircle : MonoBehaviour
{
    Vector3 _pos;
    float _startTime = 0.0f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,30));
    }
}
