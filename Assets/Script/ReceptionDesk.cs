using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionDesk : MonoBehaviour
{
    [SerializeField] MessageBox messageBox;

    [SerializeField] [TextArea] string[] message1;
    [SerializeField] [TextArea] string[] message2;

    [SerializeField] StageSelectUI stageSelectUi;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("決定"))
            {
            }
        }

    }
}
