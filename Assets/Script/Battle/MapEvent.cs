using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : EventBase
{
    Player player;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("決定"))
            {
                player = other.GetComponent<Player>();
                LookAtPlayer();
                Debug.Log("こんにちは");

                eventTask.Invoke();
            }
        }
    }

    public void LookAtPlayer()
    {
        this.transform.localEulerAngles = new Vector3(0, Mathf.Tan((player.transform.position.x - this.transform.position.x)/ (player.transform.position.y - this.transform.position.y)), 0);
    }


}
