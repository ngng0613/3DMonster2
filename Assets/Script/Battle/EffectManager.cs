using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject effectObject;
    public Vector3 producePosition { get; set; }

    public enum DirectionToProduce
    {
        PlayerSide,
        EnemySide
    }

    public void ProduceEffect(GameObject newEffect, Vector3 effectPosition)
    {
        effectObject = Instantiate(newEffect);
        effectObject.transform.position = effectPosition;
    }
    public void ProduceEffect(GameObject newEffect, Vector3 effectPosition, DirectionToProduce direction)
    {
        if (newEffect != null)
        {
            effectObject = Instantiate(newEffect);
            effectObject.transform.position = effectPosition;
            if (direction == DirectionToProduce.EnemySide)
            {
                Vector3 rotation = new Vector3(0, 180, 0);
                effectObject.transform.localEulerAngles = rotation;
            }
        }

    }

    public void DestroyObject()
    {
        Destroy(effectObject);
    }
}
