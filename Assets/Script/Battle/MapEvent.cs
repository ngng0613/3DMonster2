using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : EventBase
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("決定"))
            {
                Debug.Log("こんにちは");

                eventTask.Invoke();
            }
        }
    }
}
