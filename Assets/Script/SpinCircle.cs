using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCircle : MonoBehaviour
{
    [SerializeField] float _spinSpeed = 0.5f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _spinSpeed * Time.deltaTime));
    }
}
