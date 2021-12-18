using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusView2 : MonoBehaviour
{
    bool isChoose = false;
    public Image monsterImage;
    public TextMeshProUGUI monsterName;
    public Vector3 defaultPos;


    private void Start()
    {
        defaultPos = this.transform.localPosition;
    }

    public void Setup(MonsterBase monster)
    {
        monsterImage.sprite = monster.Image;
        monsterName.text = monster.MonsterName;
    }

    /// <summary>
    /// 選択中か否か、状態を変更する
    /// </summary>
    /// <param name="state">変更後の状態</param>
    public void ChangeState(bool state)
    {
        isChoose = state;
        if (state)
        {
            this.transform.localPosition = defaultPos + new Vector3(0,150,0);
        }
        else
        {
            this.transform.localPosition = defaultPos;
        }
    }
    

}
