using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public List<GameObject> EventsAround;

    public void ListClear()
    {
        EventsAround.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            EventsAround.Add(other.gameObject.transform.parent.gameObject);
        }
      
    }

}
