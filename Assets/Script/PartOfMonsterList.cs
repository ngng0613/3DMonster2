using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartOfMonsterList : MonoBehaviour,IPointerUpHandler
{


    public void OnDrag()
    {
        this.gameObject.transform.position = Input.mousePosition;




    }


    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            Debug.Log(hit.gameObject.name);
        }
    }
}
