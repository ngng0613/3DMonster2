using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager2 : MonoBehaviour
{
    bool _menuIsActive = false;
    [SerializeField] GameObject _objectsParent;
    [SerializeField] GameObject _header;
    [SerializeField] GameObject _footer;
    [SerializeField] GameObject _mainContents;
    RectTransform _rectTransform;

    [SerializeField] List<SelectionObject> _selectionObjects;
    [SerializeField] List<MenuContents> _menuContentsList;

    [SerializeField] int _selectedContentsIndex = 0;
    int _maxMenuLength;

    public delegate void Func();
    Func _afterClosed;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private void Start()
    {
        _rectTransform = _mainContents.GetComponent<RectTransform>();
    }


    public void Setup(Func AfterClosed)
    {
        SetInput();
        this._afterClosed = AfterClosed;
    }

    void SetInput()
    {
        InputManager.ResetInputSettings();
        InputManager.InputCancel = () => CloseMenu();
    }

    public void Activate()
    {
        OpenMenu();
        Player.CanMove = false;
    }


    void OpenMenu()
    {
        //メニューアニメーションに必要な高さ
        //float scale = mainContents.transform.localScale.y;

        
        _objectsParent.SetActive(true);
        _menuIsActive = true;
        


    }

    void CloseMenu()
    {
        _objectsParent.SetActive(false);
        _menuIsActive = false;
        InputManager.ResetInputSettings();
        Player.CanMove = true;
        _afterClosed.Invoke();

    }

    void InputDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:

                if (_selectedContentsIndex > 0)
                {
                    _selectedContentsIndex--;
                }
                else
                {
                    _selectedContentsIndex = _maxMenuLength - 1;
                }

                break;
            case Direction.Down:

                if(_selectedContentsIndex < _maxMenuLength)
                {
                    _selectedContentsIndex++;
                }
                else
                {
                    _selectedContentsIndex = 0;
                }

                break;
            case Direction.Left:
                break;
            case Direction.Right:
                break;
            default:
                break;
        }

    }



    void SelectContents()
    {
        _menuContentsList[_selectedContentsIndex].Setup(CloseMenu);
        _menuContentsList[_selectedContentsIndex].Activate();


    }

    void UpdateMenuList()
    {

    }


}
