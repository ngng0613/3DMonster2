using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class PartOfMonsterList : MonoBehaviour, IPointerUpHandler
{
    int _id;
    public MonsterBase Monster;
    [SerializeField] GameObject _statusView;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _mpText;
    [SerializeField] float _displaySpeed = 0.5f;
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
    bool _isSet = false;
    [SerializeField] Canvas _canvas;
    int _defaultSortOrder = default;
    bool _isDisplay = false;
    Sequence _sequence;

    public int Id { get => _id; set => _id = value; }

    private void Start()
    {
        _sequence = DOTween.Sequence();
        if (_canvas != null)
        {
            _defaultSortOrder = _canvas.sortingOrder;
        }
    }


    public void Open()
    {

    }

    public void DisplayUpdate()
    {
        _image.sprite = Monster.Image;
        _nameText.text = Monster.MonsterName;
        _mpText.text = $"MP : {Monster.MaxMp.ToString()}" ;

    }

    public void OnPointerEnter()
    {
        _statusView.transform.DOScaleX(1.0f, _displaySpeed * Time.deltaTime);
    }

    public void OnPointerExit()
    {
        _statusView.transform.DOScaleX(0.0f, _displaySpeed * Time.deltaTime);
    }

    public void OnDrag()
    {
        if (Release != null)
        {
            Release(this);
        }
        this.gameObject.transform.position = Input.mousePosition;
        if (_canvas != null)
        {
            _canvas.sortingOrder = _defaultSortOrder + 1;

        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (_canvas != null)
        {
            _canvas.sortingOrder = _defaultSortOrder;
        }
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        _isSet = false;
        foreach (var hit in raycastResults)
        {
            Debug.Log(hit.gameObject.name);
            if (hit.gameObject.tag != null)
            {
                if (hit.gameObject.CompareTag(_slotTagName1))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(Monster, 1, this);
                    _isSet = true;

                }
                else if (hit.gameObject.CompareTag(_slotTagName2))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(Monster, 2, this);
                    _isSet = true;
                }
                else if (hit.gameObject.CompareTag(_slotTagName3))
                {
                    ///モンスタースロットにセットされた
                    this.gameObject.transform.position = hit.gameObject.transform.position;
                    SetMonsterSlot.Invoke(Monster, 3, this);
                    _isSet = true;
                }
                else if (hit.gameObject.CompareTag(this.gameObject.tag))
                {
                    hit.gameObject.GetComponent<PartOfMonsterList>().BackToBasePos();
                }
            }
        }
        if (_isSet == false)
        {
            BackToBasePos();
        }

    }

    public void BackToBasePos()
    {
        if (Release != null)
        {
            Release.Invoke(this);
        }
        _isSet = false;
        _sequence.Append(this.gameObject.transform.DOMove(BasePos, 1.0f / _moveSpeed));

    }
}
