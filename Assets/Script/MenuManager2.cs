using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager2 : MonoBehaviour
{
    bool menuIsActive = false;
    [SerializeField] GameObject objectsParent;
    [SerializeField] GameObject header;
    [SerializeField] GameObject footer;
    [SerializeField] GameObject mainContents;
    RectTransform rectTransform;

    [SerializeField] List<SelectionObject> selectionObjects;
    [SerializeField] List<MenuContents> menuContentsList;

    [SerializeField] int selectedContentsIndex = 0;
    int maxMenuLength;

    public delegate void Func();
    Func AfterClosed;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private void Start()
    {
        rectTransform = mainContents.GetComponent<RectTransform>();
    }


    public void Setup(Func AfterClosed)
    {
        SetInput();
        this.AfterClosed = AfterClosed;
    }

    void SetInput()
    {
        InputManager.ResetInputSettings();
        InputManager.InputCancel = () => CloseMenu();
    }

    public void Activate()
    {
        OpenMenu();
        Player.canMove = false;
    }


    void OpenMenu()
    {
        //メニューアニメーションに必要な高さ
        //float scale = mainContents.transform.localScale.y;

        
        objectsParent.SetActive(true);
        menuIsActive = true;
        


    }

    void CloseMenu()
    {
        objectsParent.SetActive(false);
        menuIsActive = false;
        InputManager.ResetInputSettings();
        Player.canMove = true;
        AfterClosed.Invoke();

    }

    void InputDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:

                if (selectedContentsIndex > 0)
                {
                    selectedContentsIndex--;
                }
                else
                {
                    selectedContentsIndex = maxMenuLength - 1;
                }

                break;
            case Direction.Down:

                if(selectedContentsIndex < maxMenuLength)
                {
                    selectedContentsIndex++;
                }
                else
                {
                    selectedContentsIndex = 0;
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
        menuContentsList[selectedContentsIndex].Setup(CloseMenu);
        menuContentsList[selectedContentsIndex].Activate();


    }

    void UpdateMenuList()
    {

    }


}
