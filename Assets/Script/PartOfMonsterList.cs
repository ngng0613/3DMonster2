using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class PartOfMonsterList : MonoBehaviour, IPointerUpHandler
{
    public MonsterBase Monster;
    [SerializeField] string _slotTagName1 = "";
    [SerializeField] string _slotTagName2 = "";
    [SerializeField] string _slotTagName3 = "";
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _nametext;
    public Vector3 BasePos;
    [SerializeField] float _moveSpeed = 1.0f;

    public delegate void SetPanelDelegate(MonsterBase monster, int slotId, PartOfMonsterList partOfMonsterList);
    public SetPanelDelegate SetMonsterSlot;
    public delegate void ReleaseDelegate(PartOfMonsterList part);
    public ReleaseDelegate Release;
    bool isSet = false;

    Sequence _sequence;

    private void Start()
    {
        _sequence = DOTween.Sequence();
    }


    public void Open()
    {

    }

    public void DisplayUpdate()
    {
        _image.sprite = Monster.Image;
    }

    public void OnClick()
    {
        _sequence.Pause();
        _sequence.Kill();
    }

    public void OnDrag()
    {
        Release(this);
        this.gameObject.transform.position = Input.mousePosition;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        isSet = false;
        foreach (var hit in raycastResults)
        {
            Debug.Log(hit.gameObject.name);
            if (hit.gameObject.tag != null)
            {
                if (hit.gameObject.CompareTag(_slotTagName1))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(Monster, 1,this);
                    isSet = true;

                }
                else if (hit.gameObject.CompareTag(_slotTagName2))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(Monster, 2, this);
                    isSet = true;
                }
                else if (hit.gameObject.CompareTag(_slotTagName3))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(Monster, 3, this);
                    isSet = true;
                }
                else if (hit.gameObject.CompareTag(this.gameObject.tag))
                {
                    hit.gameObject.GetComponent<PartOfMonsterList>().BackToBasePos();
                }
            }
        }
        if (isSet == false)
        {
            BackToBasePos();
        }
    }

    public void BackToBasePos()
    {
        Release.Invoke(this);
        isSet = false;
        _sequence.Append(this.gameObject.transform.DOMove(BasePos, 1.0f / _moveSpeed));
        
    }
}
