using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] List<Banner> _bannerList = new List<Banner>();

    public delegate void AfterFuncDelegate();
    public AfterFuncDelegate AfterFunc;

    public void Setup()
    {
        for (int i = 0; i < _bannerList.Count; i++)
        {
            int x = i;
            _bannerList[i].OnClickEvent = () => HideCommands(x);
        }

    }

    public void Activate()
    {

    }

    public void Deactivate()
    {
        AfterFunc.Invoke();
        this.gameObject.SetActive(false);

    }

    public void RestCommand()
    {


    }

    /// <summary>
    /// 選択されたIndex以外のバナーを隠す
    /// </summary>
    /// <param name="index">選択したバナーのIndex</param>
    public void HideCommands(int index)
    {
        Debug.Log(index);
        for (int i = 0; i < _bannerList.Count; i++)
        {
            if (i == index)
            {
                continue;
            }
            _bannerList[i].Hide();
        }
    }

    public void DisplayCommands()
    {
        for (int i = 0; i < _bannerList.Count; i++)
        {
            _bannerList[i].MoveToNeutralPos();
        }
    }

}
