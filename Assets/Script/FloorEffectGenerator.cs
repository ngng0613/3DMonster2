using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEffectGenerator : MonoBehaviour
{
    [SerializeField] FloorEffect[] floorEffect;
    [SerializeField] Canvas generateCanvas;
    [SerializeField] float generateSpeed = 3f;
    float timeCount = 0;
    Transform generatePos;
    int generateCount = 0;

    private void Start()
    {
        generateCanvas = GameObject.FindWithTag("FloorCanvas").GetComponent<Canvas>();
    }

    public void OnUpdate(Transform generateTransform)
    {
        generatePos = generateTransform;
        timeCount += Time.deltaTime;
        if (timeCount >= 1.0f / generateSpeed)
        {
            timeCount = 0;
            Generate(this.generatePos.position);
        }
    }

    // Update is called once per frame
    void Generate(Vector3 generatePos)
    {
        FloorEffect newEffect = Instantiate(floorEffect[generateCount]);
        newEffect.transform.position = generatePos;
        newEffect.transform.position += new Vector3(0, 0.15f, 0);
        newEffect.transform.SetParent(generateCanvas.transform);
        generateCount++;
        if (floorEffect.Length <= generateCount)
        {
            generateCount = 0;
        }

    }
}
