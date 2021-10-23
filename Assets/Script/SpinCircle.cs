using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCircle : MonoBehaviour
{
    private Vector3 m_pos;
    float startTime = 0.0f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,30));
    }
}
