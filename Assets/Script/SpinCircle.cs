using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinCircle : MonoBehaviour
{
    private Vector3 pos;
    public float speed = 1.0f;
    [SerializeField] Image image;
    [SerializeField] MonsterBase monster;
    public MonsterBase Monster { get => monster; private set => monster = value; }

    [SerializeField] float circleLength = 45.0f;
    public float CircleLength { get => circleLength; private set => circleLength = value; }

    private void Start()
    {
        Application.targetFrameRate = 60;
        this.image.fillAmount = CircleLength / 360;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.1f * speed));

    }
}
