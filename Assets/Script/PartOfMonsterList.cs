using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PartOfMonsterList : MonoBehaviour, IPointerUpHandler
{
    public MonsterBase _monster;
    [SerializeField] string _slotTagName1 = "";
    [SerializeField] string _slotTagName2 = "";
    [SerializeField] string _slotTagName3 = "";
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _nametext;

    public delegate void SetPanelDelegate(MonsterBase monster, int slotId);
    public SetPanelDelegate SetMonsterSlot;

    public void Open()
    {

    }

    public void DisplayUpdate()
    {
        _image.sprite = _monster.Image;
    }

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
            if (hit.gameObject.tag != null)
            {
                if (hit.gameObject.CompareTag(_slotTagName1))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(_monster,1);

                }
                else if (hit.gameObject.CompareTag(_slotTagName2))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(_monster,2);
                }
                else if (hit.gameObject.CompareTag(_slotTagName3))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(_monster,3);
                }
            }

        }
    }
}
